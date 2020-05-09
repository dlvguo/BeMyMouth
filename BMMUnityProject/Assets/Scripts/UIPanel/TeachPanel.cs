using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeachPanel : BasePanel
{

    public InputField gestureInputField;

    //提示按钮
    public Button tipBtu;
    public Button exitBtu;
    public GameObject yesImg, noImg;
    public AniManager aniManager;
    private Canvas canvas;
    public GameObject leapStudyModelOB;
    private GameObject leapStudyModel;

    // Start is called before the first frame update
    void Start()
    {
        yesImg.SetActive(false);
        noImg.SetActive(false);
        tipBtu.gameObject.SetActive(false);
        //关闭
        tipBtu.onClick.AddListener(() => tipBtu.gameObject.SetActive(false));
        exitBtu.onClick.AddListener(() => OnCancelBtnClick());
        aniManager = GameObject.Find("AnimManager").GetComponent<AniManager>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        leapStudyModel = Instantiate(leapStudyModelOB);
        Transform tran = leapStudyModel.transform;
        leapStudyModel.transform.SetParent(GameObject.Find("Main Camera").transform);
        leapStudyModel.transform.localPosition = tran.position;
        leapStudyModel.SetActive(false);
        leapStudyModel.GetComponentInChildren<LeapGesturesTeacher>().AddListener(OnReconized);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (gestureInputField.text == string.Empty)
            {
                tipBtu.gameObject.SetActive(true);
            }
            else
            {
                leapStudyModel.SetActive(true);
                //TODO这里更改
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
            }
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            if (gestureInputField.text.Length > 0 && !aniManager.IsPlay)
            {
                aniManager.ProcessQueue(gestureInputField.text.ToCharArray());
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
            }
        }
    }

    public void OnReconized(string str)
    {
        if (str.Equals(gestureInputField.text))
        {
            yesImg.SetActive(true);
            yes = true;
        }
        else
        {
            yes = false;
            noImg.SetActive(true);

        }
        Invoke("ImgQuit", 2);
        leapStudyModel.SetActive(false);
    }

    bool yes = false;
    public void ImgQuit()
    {
        if (yes)
        {
            yesImg.SetActive(false);
        }
        else
        {
            noImg.SetActive(false);
        }
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
        //uiMng.RemovePanelDict(UIPanelType.TeachPanel);
        //Destroy(this.gameObject);
        gameObject.SetActive(false);

    }
    #endregion 
}
