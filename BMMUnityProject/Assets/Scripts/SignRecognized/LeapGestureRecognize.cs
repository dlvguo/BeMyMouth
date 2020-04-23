using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapGestureRecognize : MonoBehaviour
{
    // Start is called before the first frame update

    LeapGestureClassifier gestureClassifier;

    void Start()
    {
        gestureClassifier = LeapGestureClassifier.GetInstance;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
