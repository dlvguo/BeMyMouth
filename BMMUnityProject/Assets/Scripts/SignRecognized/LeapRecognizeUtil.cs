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

}
