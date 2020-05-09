using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;

[System.Serializable]
public class PlayAnima : MonoBehaviour {

    private AniManager aniManager;

    private InputField Input;

    private Canvas canvas;

    private void Start()
    {
        aniManager = GameObject.Find("AnimManager").GetComponent<AniManager>();
        Input = GetComponentInChildren<InputField>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        //this.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void Update()
    {
        if(aniManager.IsPlay == false )
        {
            gameObject.tag = "Button";
        }
    }

    public void OnClick()
    {
        if (aniManager.IsPlay == false)
        {
            aniManager.ProcessQueue(Input.text.ToCharArray());
            this.gameObject.tag = "OnPlay";
        }
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
    }
}
