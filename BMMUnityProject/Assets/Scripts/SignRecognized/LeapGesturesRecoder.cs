using Leap;
using Leap.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapGesturesRecoder : MonoBehaviour
{
    private LeapServiceProvider serviceProvider;

    //用于记录LeapController
    private void Awake()
    {
        serviceProvider = this.GetComponent<LeapServiceProvider>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (serviceProvider.GetLeapController().IsConnected)
            {
                var hands = serviceProvider.GetLeapController().Frame().Hands;
                for (int i = 0; i < hands.Count; i++)
                {
                    Hand hand = hands[0];
                    //Debug.Log("PalmPos" + hand.PalmPosition);

                    Debug.Log("PalmDir" + hand.PalmNormal);
                    Debug.Log(LeapRecognizeUtil.FigurePalmDirection(hand.PalmNormal));

                }
                //LeapRecognizeUtil.LeapGestureSerialize();
                var str = FileUtil.ReadFile(Environment.CurrentDirectory + "/Datas/GestureDatas/", "test", FileUtil.FileType.Json);
                var items = LeapRecognizeUtil.LeapGestureDeserialize(str);

                Debug.Log(FileUtil.ReadFile(Environment.CurrentDirectory + "/Datas/GestureDatas/", "test", FileUtil.FileType.Json));
            }
        }
    }
}
