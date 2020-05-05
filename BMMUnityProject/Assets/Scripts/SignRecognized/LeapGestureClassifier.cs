using Leap;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapGestureClassifier
{
    //根据左右手分类 再根据手掌方向分类 最后Leap实体对应识别的文字
    Dictionary<HandType, Dictionary<PalmDirection, List<KeyValuePair<string, LeapGestureEntity>>>> gestureNameDict;

    //延迟单例模式
    private static Lazy<LeapGestureClassifier> instance = new Lazy<LeapGestureClassifier>();

    public static LeapGestureClassifier GetInstance => instance.Value;

    //获取手势库
    public LeapGestureClassifier()
    {
        InitGesDict();
    }

    /// <summary>
    /// 初始化手势库
    /// </summary>
    void InitGesDict()
    {
        //gestureNameDict = new Dictionary<string, LeapGestureEntity>();
        gestureNameDict = new Dictionary<HandType, Dictionary<PalmDirection, List<KeyValuePair<string, LeapGestureEntity>>>>();
        var filenames = FileUtil.GetDirectoryFilesNames(Environment.CurrentDirectory + "/Datas/GestureDatas/", FileUtil.FileType.Json);
        //初始化字典
        foreach (var filename in filenames)
        {
            var json = FileUtil.ReadFile(Environment.CurrentDirectory + "/Datas/GestureDatas/", filename, FileUtil.FileType.Json);
            var entity = LeapRecognizeUtil.LeapGestureDeserialize(json);
            //gestureNameDict.Add(filename, entity);
            Dictionary<PalmDirection, List<KeyValuePair<string, LeapGestureEntity>>> palmDict;
            //存在
            if (gestureNameDict.TryGetValue(entity.HandType, out palmDict))
            {
                List<KeyValuePair<string, LeapGestureEntity>> entityGes;
                PalmDirection palmDirection = entity.HandType == HandType.RightHand ? entity.RightPalmDirection : entity.LeftPalmDirection;
                //左右手
                if (palmDict.TryGetValue(palmDirection, out entityGes))
                {
                    KeyValuePair<string, LeapGestureEntity> keyValuePair = new KeyValuePair<string, LeapGestureEntity>(filename, entity);
                    entityGes.Add(keyValuePair);
                    palmDict[palmDirection] = entityGes;
                }
                else
                {
                    entityGes = new List<KeyValuePair<string, LeapGestureEntity>>();
                    KeyValuePair<string, LeapGestureEntity> keyValuePair = new KeyValuePair<string, LeapGestureEntity>(filename, entity);
                    entityGes.Add(keyValuePair);
                    palmDict.Add(palmDirection, entityGes);
                }
                gestureNameDict[entity.HandType] = palmDict;
            }
            //不存在的话
            else
            {
                palmDict = new Dictionary<PalmDirection, List<KeyValuePair<string, LeapGestureEntity>>>();
                List<KeyValuePair<string, LeapGestureEntity>> entityGes = new List<KeyValuePair<string, LeapGestureEntity>>();
                KeyValuePair<string, LeapGestureEntity> keyValuePair = new KeyValuePair<string, LeapGestureEntity>(filename, entity);
                entityGes.Add(keyValuePair);
                PalmDirection palmDirection = entity.HandType == HandType.RightHand ? entity.RightPalmDirection : entity.LeftPalmDirection;
                palmDict.Add(palmDirection, entityGes);
                gestureNameDict.Add(entity.HandType, palmDict);
            }
        }
    }

    /// <summary>
    /// 手势识别
    /// </summary>
    /// <param name="entity"></param>
    public void GesRecognize(LeapGestureEntity entity)
    {
        Dictionary<PalmDirection, List<KeyValuePair<string, LeapGestureEntity>>> palmDict;
        if (gestureNameDict.TryGetValue(entity.HandType, out palmDict))
        {
            List<KeyValuePair<string, LeapGestureEntity>> entityGes;
            PalmDirection palmDirection = entity.HandType == HandType.RightHand ? entity.RightPalmDirection : entity.LeftPalmDirection;
            //左右手
            if (palmDict.TryGetValue(palmDirection, out entityGes))
            {
                //找到手势
                //计算最小值
                string gesName = string.Empty;
                float dist = float.MaxValue;
                foreach (var entges in entityGes)
                {
                    var ent = entges.Value;
                    float temp = LeapRecognizeUtil.FigureListDist(entity.RightFingersDist, ent.RightFingersDist);
                    temp += LeapRecognizeUtil.FigureListDist(entity.LeftFingersDist, ent.LeftFingersDist);
                    if (temp < dist)
                    {
                        gesName = entges.Key;
                        dist = temp;
                    }
                }
                Debug.Log(string.Format("最小距离:{0} 识别手势:{1}", dist, gesName));
            }
            else
                Debug.Log("未找到手掌方向字典");

        }
        else
            Debug.Log("未找到手类型字典");
    }


    /// <summary>
    /// 手势识别
    /// </summary>
    /// <param name="entity"></param>
    public string GesRecognized(LeapGestureEntity entity)
    {
        Dictionary<PalmDirection, List<KeyValuePair<string, LeapGestureEntity>>> palmDict;
        if (gestureNameDict.TryGetValue(entity.HandType, out palmDict))
        {
            List<KeyValuePair<string, LeapGestureEntity>> entityGes;
            PalmDirection palmDirection = entity.HandType == HandType.RightHand ? entity.RightPalmDirection : entity.LeftPalmDirection;
            //左右手
            if (palmDict.TryGetValue(palmDirection, out entityGes))
            {
                //找到手势
                //计算最小值
                string gesName = string.Empty;
                float dist = float.MaxValue;
                foreach (var entges in entityGes)
                {
                    var ent = entges.Value;
                    float temp = LeapRecognizeUtil.FigureListDist(entity.RightFingersDist, ent.RightFingersDist);
                    temp += LeapRecognizeUtil.FigureListDist(entity.LeftFingersDist, ent.LeftFingersDist);
                    if (temp < dist)
                    {
                        gesName = entges.Key;
                        dist = temp;
                    }
                }
                Debug.Log(string.Format("最小距离:{0} 识别手势:{1}", dist, gesName));
                return gesName;
            }
            else
                Debug.Log("未找到手掌方向字典");

        }
        else
            Debug.Log("未找到手类型字典");
        return string.Empty;
    }
}
