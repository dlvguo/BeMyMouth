using Leap;
using Leap.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeapGesturesTeacher : MonoBehaviour
{
    //获取LeapMotion相关信息
    private LeapServiceProvider serviceProvider;

    //记录最后一帧的手势
    private long lastFrameId = 0;

    private event Action<string> OnRecognized;
    private LeapGestureEntity lastLeapGesEntity;


    //正确和错误的图片显示
    LeapGestureClassifier gestureClassifier;

    //用于记录LeapController
    private void Awake()
    {
    }


    void Start()
    {
        serviceProvider = this.GetComponent<LeapServiceProvider>();
        gestureClassifier = LeapGestureClassifier.GetInstance;
    }

    private float timer = 0;

    // Update is called once per frame
    void Update()
    {
        //Leap连接才可以
        if (GetLeapController().IsConnected)
        {
            if (timer > 3)
            {
                //手势识别
                var frame = GetLeapController().Frame();
                if (FilterGes(frame))
                    RecognizeGes(frame);
                timer = 0;
            }
            timer += Time.deltaTime;
        }
    }

    private bool FilterGes(Frame frame)
    {
        if (frame.Hands.Count == 0 || frame.Id == lastFrameId)
            return false;
        return true;
    }



    #region 手势识别测试

    void RecognizeGes(Frame frame)
    {
        var entity = LeapRecognizeUtil.BuildLeapGestureEntity(frame);
        lastLeapGesEntity = entity;
        var str = gestureClassifier.GesRecognized(entity);
        //触发事件
        OnRecognized.Invoke(str);
    }
    #endregion

    public void AddListener(Action<string> action)
    {
        Debug.Log("添加手势识别事件成功");
        OnRecognized += action;
    }

    //获取LeapCtrl
    Controller GetLeapController()
    {
        return serviceProvider.GetLeapController();
    }
}
