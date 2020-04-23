using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapGestureClassifier
{
    //根据左右手分类 再根据手掌方向分类 最后Leap实体对应识别的文字
    Dictionary<HandType, Dictionary<PalmDirection, Dictionary<string, LeapGestureEntity>>> gestureNameDict;

    private static Lazy<LeapGestureClassifier> instance = new Lazy<LeapGestureClassifier>();

    public static LeapGestureClassifier GetInstance => instance.Value;

    //获取手势库
    public LeapGestureClassifier()
    {
        //gestureNameDict = new Dictionary<string, LeapGestureEntity>();
        var filenames = FileUtil.GetDirectoryFilesNames(Environment.CurrentDirectory + "/Datas/GestureDatas/", FileUtil.FileType.Json);
        foreach (var filename in filenames)
        {
            var json = FileUtil.ReadFile(Environment.CurrentDirectory + "/Datas/GestureDatas/", filename, FileUtil.FileType.Json);
            var entity = LeapRecognizeUtil.LeapGestureDeserialize(json);
            //gestureNameDict.Add(filename, entity);
        }
    }
}
