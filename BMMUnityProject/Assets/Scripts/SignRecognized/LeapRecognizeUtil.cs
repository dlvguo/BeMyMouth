using Leap;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LeapRecognizeUtil
{
    //计算手指双向
    public static PalmDirection FigurePalmDirection(Leap.Vector vector)
    {
        double x = Mathf.Abs(vector.x), y = Mathf.Abs(vector.y), z = Mathf.Abs(vector.z);
        // x最大说明是Left和Right
        if (x > y && x > z)
        {
            if (vector.x > 0)
                return PalmDirection.Left;
            return PalmDirection.Right;
        }
        // y最大说明是Front和Behind
        else if (y > z && y > x)
        {
            if (vector.y > 0)
                return PalmDirection.Front;
            return PalmDirection.Behind;

        }
        //z最大说明是Up和Down
        else
        {
            if (vector.z > 0)
                return PalmDirection.Down;
            return PalmDirection.Up;
        }
    }

    /// <summary>
    /// 一个手势一个Json 这样方便存储
    /// </summary>
    /// <param name="entity"></param>
    public static void LeapGestureSerialize(LeapGestureEntity entity, string gesname)
    {
        //LeapGestureEntity leapGestureEntity = new LeapGestureEntity()
        //{
        //    HandType = HandType.DoubleHand,
        //    LeftPalmDirection = PalmDirection.Behind,
        //    RightPalmDirection = PalmDirection.Front,
        //    LeftFingerDisatance = new List<float>(new float[] { 1f, 2f, 3f }),
        //    RightFingersDist = new List<float>(new float[] { 1.2f, 3.2f, 4.3f })
        //};
        //LeapGestureEntity leapGestureEntity2 = new LeapGestureEntity()
        //{
        //    HandType = HandType.DoubleHand,
        //    LeftPalmDirection = PalmDirection.Behind,
        //    RightPalmDirection = PalmDirection.Front,
        //    LeftFingerDisatance = new List<float>(new float[] { 1f, 2f, 3f }),
        //    RightFingersDist = new List<float>(new float[] { 1.2f, 3.2f, 4.3f })
        //};
        //List<LeapGestureEntity> l = new List<LeapGestureEntity>();
        //l.Add(leapGestureEntity);
        //l.Add(leapGestureEntity2);
        if (entity != null)
        {
            string json = JsonConvert.SerializeObject(entity);
            FileUtil.SaveFile(Environment.CurrentDirectory + "/Datas/GestureDatas/", gesname, json, FileUtil.FileType.Json);
        }
    }

    public static LeapGestureEntity FigureAverage(List<LeapGestureEntity> leapGestureEntities)
    {
        LeapGestureEntity leapGestureEntity = new LeapGestureEntity()
        {
            HandType = leapGestureEntities[0].HandType,
            LeftPalmDirection = leapGestureEntities[0].LeftPalmDirection,
            RightPalmDirection = leapGestureEntities[0].RightPalmDirection,
            LeftFingersDist = new List<float>(leapGestureEntities[0].LeftFingersDist),
            RightFingersDist = new List<float>(leapGestureEntities[0].RightFingersDist),
            LeftFingersExtenison = new List<bool>(leapGestureEntities[0].LeftFingersExtenison),
            RightFingersExtenison = new List<bool>(leapGestureEntities[0].RightFingersExtenison)
        };

        //计算各个角度是平均值
        int count = leapGestureEntities.Count;
        if (leapGestureEntity.HandType == HandType.DoubleHand)
        {
            for (int i = 1; i < leapGestureEntities.Count; i++)
            {
                for (int j = 0; j < leapGestureEntities[i].LeftFingersDist.Count; j++)
                {
                    leapGestureEntity.LeftFingersDist[j] += leapGestureEntities[i].LeftFingersDist[j];
                    leapGestureEntity.RightFingersDist[j] += leapGestureEntities[i].RightFingersDist[j];
                }
            }
        }
        else if (leapGestureEntity.HandType == HandType.LeftHand)
        {
            for (int i = 1; i < leapGestureEntities.Count; i++)
            {
                for (int j = 0; j < leapGestureEntities[i].LeftFingersDist.Count; j++)
                {
                    leapGestureEntity.LeftFingersDist[j] += leapGestureEntities[i].LeftFingersDist[j];
                }
            }
        }
        else
        {
            for (int i = 1; i < leapGestureEntities.Count; i++)
            {
                for (int j = 0; j < leapGestureEntities[i].LeftFingersDist.Count; j++)
                {
                    leapGestureEntity.LeftFingersDist[j] += leapGestureEntities[i].LeftFingersDist[j];
                }
            }
        }
        if (leapGestureEntity.LeftFingersDist.Count > 0)
        {
            for (int j = 0; j < leapGestureEntity.LeftFingersDist.Count; j++)
            {
                leapGestureEntity.LeftFingersDist[j] /= count;
            }

        }
        if (leapGestureEntity.RightFingersDist.Count > 0)
        {
            for (int j = 0; j < leapGestureEntity.RightFingersDist.Count; j++)
            {
                leapGestureEntity.RightFingersDist[j] /= count;
            }
        }
        return leapGestureEntity;
    }

    //根据Json获取LeapGesEntity
    public static List<LeapGestureEntity> LeapGesturesDeserialize(string json)
    {
        var leapGestures = JsonConvert.DeserializeObject<List<LeapGestureEntity>>(json);
        return leapGestures;
    }

    //序列化
    public static LeapGestureEntity LeapGestureDeserialize(string json)
    {
        var leapGesture = JsonConvert.DeserializeObject<LeapGestureEntity>(json);
        return leapGesture;
    }

    public static List<float> FigureFingersDist(Hand hand)
    {
        List<float> dists = new List<float>();
        var fingers = hand.Fingers;
        //计算为 手掌->1/2/3/4/5 
        for (int i = 0; i < fingers.Count; i++)
        {
            float dist = Vector3.Distance(LVConvertToUV3(hand.PalmPosition), LVConvertToUV3(fingers[i].TipPosition));
            dists.Add(dist);
        }

        //计算各手指之间距离 1->2/3/4/5 2->3/4/5 3->4/5 4->5
        for (int i = 0; i < fingers.Count - 1; i++)
        {
            for (int j = i + 1; j < fingers.Count; j++)
            {
                float dist = Vector3.Distance(LVConvertToUV3(fingers[i + 1].TipPosition), LVConvertToUV3(fingers[i].TipPosition));
                dists.Add(dist);
            }
        }
        return dists;
    }

    /// <summary>
    /// 将Leap的Vector转成Unity V3
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static Vector3 LVConvertToUV3(Vector a)
    {
        Vector3 vector = new Vector3(a.x, a.y, a.z);
        return vector;
    }

}
