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


    /// <summary>
    /// 计算平均手势
    /// </summary>
    /// <param name="leapGestureEntities"></param>
    /// <returns></returns>
    public static LeapGestureEntity FigureAverageGesEntity(List<LeapGestureEntity> leapGestureEntities)
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
                for (int j = 0; j < leapGestureEntities[i].RightFingersDist.Count; j++)
                {
                    leapGestureEntity.RightFingersDist[j] += leapGestureEntities[i].RightFingersDist[j];
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

    /// <summary>
    /// 手指之间的距离
    /// </summary>
    /// <param name="hand"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 计算距离
    /// </summary>
    /// <param name="l1"></param>
    /// <param name="l2"></param>
    /// <returns></returns>
    public static float FigureListDist(List<float> l1, List<float> l2)
    {
        float dist = 0;
        for (int i = 0, j = 0; i < l1.Count && j < l2.Count; i++, j++)
        {
            dist += Mathf.Abs(l1[i] - l2[j]);
        }
        return dist;
    }

    //根据一帧创建LeapGesture
    public static LeapGestureEntity BuildLeapGestureEntity(Frame frame)
    {
        LeapGestureEntity leapGestureEntity = new LeapGestureEntity()
        {
            LeftFingersDist = new List<float>(),
            RightFingersDist = new List<float>(),
            LeftPalmDirection = PalmDirection.None,
            RightPalmDirection = PalmDirection.None,
            LeftFingersExtenison = new List<bool>(new bool[5] { false, false, false, false, false }),
            RightFingersExtenison = new List<bool>(new bool[5] { false, false, false, false, false })
        };
        //获取手掌
        var hands = frame.Hands;
        if (hands.Count == 2)
        {
            leapGestureEntity.HandType = HandType.DoubleHand;
        }
        else
        {
            leapGestureEntity.HandType = hands[0].IsLeft ? HandType.LeftHand : HandType.RightHand;
        }
        //TODO 暂时只计算5个手指 和手掌方向的向量
        for (int i = 0; i < hands.Count; i++)
        {
            Hand hand = hands[i];
            if (hand.IsLeft)
            {
                leapGestureEntity.LeftPalmDirection = LeapRecognizeUtil.FigurePalmDirection(hand.PalmNormal);
                leapGestureEntity.LeftFingersDist = LeapRecognizeUtil.FigureFingersDist(hand);
                for (int j = 0; j < 5; j++)
                {
                    leapGestureEntity.LeftFingersExtenison[i] = hand.Fingers[i].IsExtended;
                }
            }
            else
            {
                leapGestureEntity.RightPalmDirection = LeapRecognizeUtil.FigurePalmDirection(hand.PalmNormal);
                leapGestureEntity.RightFingersDist = LeapRecognizeUtil.FigureFingersDist(hand);
                for (int j = 0; j < 5; j++)
                {
                    leapGestureEntity.RightFingersExtenison[i] = hand.Fingers[i].IsExtended;
                }
            }
        }
        Debug.Log("RecodeGes");
        return leapGestureEntity;
    }

}
