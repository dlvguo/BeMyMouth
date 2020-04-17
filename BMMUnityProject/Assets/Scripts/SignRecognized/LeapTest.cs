using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap;
using System;

public class LeapTest : MonoBehaviour
{
    private LeapServiceProvider serviceProvider;

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
            Debug.Log("FUCK");
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
            }
        }
    }
}
