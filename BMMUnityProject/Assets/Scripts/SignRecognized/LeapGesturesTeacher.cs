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

    //记录是否暂停记录
    private bool pause = false;

    //记录最后一帧的手势
    private long lastFrameId = 0;

    public InputField gestureInputField;

    public Button tipBtu;
    public Button exitBtu;

    //正确和错误的图片显示
    public GameObject yesImg, noImg;
    private List<LeapGestureEntity> leapGestureEntities;
    LeapGestureClassifier gestureClassifier;

    //用于记录LeapController
    private void Awake()
    {
        leapGestureEntities = new List<LeapGestureEntity>();
        serviceProvider = this.GetComponent<LeapServiceProvider>();
    }


    void Start()
    {
        var item = LeapGestureClassifier.GetInstance;
        yesImg.SetActive(false);
        noImg.SetActive(false);
        tipBtu.gameObject.SetActive(false);
        //关闭
        tipBtu.onClick.AddListener(() => tipBtu.gameObject.SetActive(false));
        gestureClassifier = LeapGestureClassifier.GetInstance;
        exitBtu.onClick.AddListener(() => Facade.Instance.PopUIPanel());
    }

    // Update is called once per frame
    void Update()
    {
        //Leap连接才可以
        if (GetLeapController().IsConnected)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (gestureInputField.text == string.Empty)
                {
                    tipBtu.gameObject.SetActive(true);
                }
                else
                {
                    RecognizeGes(GetLeapController().Frame());
                }
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                if (gestureInputField.text == string.Empty)
                {
                    tipBtu.gameObject.SetActive(true);
                }
                else
                {

                }
            }
        }
    }


    #region 手势识别测试

    void RecognizeGes(Frame frame)
    {
        var entity = LeapRecognizeUtil.BuildLeapGestureEntity(frame);
        var str = gestureClassifier.GesRecognized(entity);
        if (str.Equals(gestureInputField.text))
        {
            yesImg.SetActive(true);
        }
        else
        {
            noImg.SetActive(true);
        }

    }

    #endregion


    //记录手势
    void RecodeGes(Frame frame)
    {
        if (frame.Id == lastFrameId)
            return;
        lastFrameId = frame.Id;
        if (frame.Hands.Count > 0)
            leapGestureEntities.Add(BuildLeapGestureEntity(frame));
    }

    //根据一帧创建LeapGesture
    public LeapGestureEntity BuildLeapGestureEntity(Frame frame)
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
        return leapGestureEntity;
    }
    //获取LeapCtrl
    Controller GetLeapController()
    {
        return serviceProvider.GetLeapController();
    }
}
