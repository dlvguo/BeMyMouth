using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;

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
        if (serviceProvider.GetLeapController().IsConnected == false)
        {
            Debug.Log("FUCK");
        }
    }
}
