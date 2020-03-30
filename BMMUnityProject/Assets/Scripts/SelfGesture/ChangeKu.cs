using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeKu : MonoBehaviour
{
    private RecordLoader leapRecorder;
    private HandPool handPool;

    private void Update()
    {
        if (GameObject.Find("LeapController(Clone)"))
        {
            leapRecorder = GameObject.FindWithTag("trainer").GetComponent<RecordLoader>();
            handPool = GameObject.FindWithTag("leap").GetComponent<HandPool>();
        }
    }

    public void OnClickSelf()
    {
        leapRecorder.AnimiFile = "自制";
        leapRecorder.load();
    }

    public void OnClickSystem()
    {
        leapRecorder.AnimiFile = "GestureFile";
        leapRecorder.load();
    }
}
