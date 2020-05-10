using Leap;
using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecoderPanel : BasePanel
{

    public InputField gestureInputField;
    public Button tipBtu;
    public Button exitBtn;

    public GameObject leapRecoderModelOB;
    private GameObject leapRecoderModel;

    //用于记录LeapController
    private void Awake()
    {
    }


    void Start()
    {
        tipBtu.gameObject.SetActive(false);
        //关闭
        tipBtu.onClick.AddListener(() => tipBtu.gameObject.SetActive(false));
        exitBtn.onClick.AddListener(() => OnCancelBtnClick());
        leapRecoderModel = Instantiate(leapRecoderModelOB);
        Transform tran = leapRecoderModel.transform;
        leapRecoderModel.transform.SetParent(GameObject.Find("Main Camera").transform);
        leapRecoderModel.transform.localPosition = tran.position;
        leapRecoderModel.SetActive(true);
    }



    #region 初始模块
    //返回上个页面 
    private void OnCancelBtnClick()
    {
        uiMng.PopPanel();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        this.gameObject.SetActive(true);
        if (leapRecoderModel)
            leapRecoderModel.SetActive(true);

    }

    public override void OnResume()
    {
        base.OnResume();
        gameObject.SetActive(true);
    }

    public override void OnPause()
    {
        gameObject.SetActive(false);
    }

    public override void OnExit()
    {
        base.OnExit();
        leapRecoderModel.SetActive(false);

        gameObject.SetActive(false);
    }
    #endregion 
}
