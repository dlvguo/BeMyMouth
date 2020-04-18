using Newtonsoft.Json;
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

    public static void TestString()
    {
        var txt = TextUtil.ReadText("Datas/1");
        LeapGestureSerialize();
    }

    public static void LeapGestureSerialize(LeapGestureEntity entity = null)
    {
        LeapGestureEntity leapGestureEntity = new LeapGestureEntity()
        {
            HandType = HandType.DoubleHand,
            LeftPalmDirection = PalmDirection.Behind,
            RightPalmDirection = PalmDirection.Front,
            LeftFingerDisatance = new List<float>(new float[] { 1f, 2f, 3f }),
            RightFingersDist = new List<float>(new float[] { 1.2f, 3.2f, 4.3f })
        };
        LeapGestureEntity leapGestureEntity2 = new LeapGestureEntity()
        {
            HandType = HandType.DoubleHand,
            LeftPalmDirection = PalmDirection.Behind,
            RightPalmDirection = PalmDirection.Front,
            LeftFingerDisatance = new List<float>(new float[] { 1f, 2f, 3f }),
            RightFingersDist = new List<float>(new float[] { 1.2f, 3.2f, 4.3f })
        };
        List<LeapGestureEntity> l = new List<LeapGestureEntity>();
        l.Add(leapGestureEntity);
        l.Add(leapGestureEntity2);

        string json1 = JsonConvert.SerializeObject(l);
        string path = Application.dataPath + "/Resources/Datas/GestureDatas/test.json";
        Debug.Log(path);

        Debug.Log(json1);

        var item = JsonConvert.DeserializeObject<List<LeapGestureEntity>>(json1);

    }

}
