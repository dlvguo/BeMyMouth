using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChatPanel : BasePanel
{
    //当前聊天用户ID
    public int NowChatId { get; set; }
    //聊天用户名称
    private Text frName;
    //消息输入框
    private InputField msIF;
    //发送消息
    private SendToSaveChatMessageRequest scmRequest;
    private KeyboardSelector keyboardSelector;
    private List<ChatSelfItem> chatSelfItems = new List<ChatSelfItem>();
    private List<ChatFriendItem> chatFrItems = new List<ChatFriendItem>();

    //用于实例话朋友消息
    public GameObject frItem;
    //用于实例话自己消息
    public GameObject selfItem;
    //应该是滑动条布局
    public RectTransform content;
    //消息滑动条
    public Scrollbar chatScrollBar;

    //滑动条
    public VerticalLayoutGroup parent;
    //更改类型
    public ChangeTypeButton ctButton;

    public GameObject LeapController;
#if UNITY_STANDALONE_WIN  //手语需要

    public GameObject qingKong;
    public GameObject allSend;

    public GameObject message;
#endif

    private Transform mainCameraTransform;
    private Canvas canvas;

    public GameObject LeapMotionController;

    private Text kuSelf;
    private Text kuSystem;

    private void Awake()
    {

        mainCameraTransform = GameObject.Find("Main Camera").transform;
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        //chatScrollBar.onValueChanged.AddListener((v) => StartCoroutine("InsSrollBar"));

#if UNITY_STANDALONE_WIN||DEBUG
        kuSelf = GameObject.FindWithTag("kuSelf").GetComponent<Text>();
        kuSystem = GameObject.FindWithTag("kuSystem").GetComponent<Text>();
        qingKong.SetActive(false);
        allSend.SetActive(false);
        message.SetActive(false);
#endif
        //        msIF.onValueChanged.AddListener((data) =>
        //        {
        //            (transform as RectTransform).localPosition += new Vector3(0, 300, 0);
        //        });
    }

    public override void OnExit()
    {
        gameObject.SetActive(false);
    }

    public override void OnEnter()
    {
        gameObject.SetActive(true);
    }
    //
    //    public override void OnPause()
    //    {
    //        gameObject.SetActive(false);
    //    }
    //
    //    public override void OnResume()
    //    {
    //        gameObject.SetActive(true);
    //    }

    public UIManager GetUiMng()
    {
        return uiMng;
    }

    public Facade GetFacade()
    {
        return facade;
    }

    private List<string> insChatFrItemDatas = new List<string>();
    private bool isTimeToInsChatFrItem;

    private void Update()
    {
        if (isTimeToInsChatFrItem)
        {
            foreach (var message in insChatFrItemDatas)
            {
                InsFrChatItem(message);
            }

            isTimeToInsChatFrItem = false;
        }

        if (!string.IsNullOrEmpty(syncInsFrChatItemMessage))
        {
            InsFrChatItem(syncInsFrChatItemMessage);
            syncInsFrChatItemMessage = null;
        }
    }

    //删除聊天消息
    public void DestroyChatItem()
    {
        if (chatSelfItems.Count == 0 || chatFrItems.Count == 0)
            return;
        BroadcastMessage("Destroy");//TODO 删除应该是有问题的
    }


    public override void InitPanelThings()
    {
        transform.Find("frName/back").GetComponent<Button>().onClick.AddListener(() => { uiMng.PopPanel(); });
        ctButton = transform.Find("ms/cb").GetComponent<ChangeTypeButton>();
        ctButton.GetComponent<Button>().onClick.AddListener(ChangeType);
        frName = transform.Find("frName/Text").GetComponent<Text>();
        msIF = transform.Find("ms/msIF").GetComponent<InputField>();
        transform.Find("ms/sendBtn").GetComponent<Button>().onClick.AddListener(OnSendButtonClick);
        scmRequest = GetComponent<SendToSaveChatMessageRequest>();
        keyboardSelector = GetComponent<KeyboardSelector>();
        base.InitPanelThings();
    }

    public void SetFrName(string name)
    {
        frName.text = name;
    }

    private void OnSendButtonClick()
    {
        if (string.IsNullOrEmpty(msIF.text) || NowChatId == 0)
        {
            uiMng.ShowPanelMessage(UIPanelType.ChatPanel, "对方不在线,这是哥在测试");//TODO 应该是测试用的
            return;
        }

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        string data = msIF.text + ',' + NowChatId;
        InsSelfChatItem(msIF.text);
        scmRequest.SendRequest(data);
        GetComponentInChildren<ClearInputFiled>().ResetIF();
    }

    public void OnReciveChatMessageResponse(string data)
    {
        string[] strs = data.Split(',');
        foreach (var message in strs)
        {
            insChatFrItemDatas.Add(message);
        }

        isTimeToInsChatFrItem = true;
    }

    private string syncInsFrChatItemMessage;

    public void SyncInsFrChatItem(string message)
    {
        syncInsFrChatItemMessage = message;
    }

    public void ChangeType()
    {
        facade.ChangeType();
        ctButton.ChangeButton(facade.GetUserType());
        ChangeButton(facade.GetUserType());
    }


    public SwitchManager.UserType GetUserType()
    {
        return facade.GetUserType();
    }

    public void Display()
    {
        uiMng.PushPanel(UIPanelType.DisplayPanel);
    }

    public void InsFrChatItem(string message)
    {
        GameObject g = Instantiate(frItem);
        g.transform.SetParent(parent.transform);
        g.transform.localScale = new Vector3(1f, 1f, 1f);
        content.sizeDelta = new Vector2(content.sizeDelta.x, +content.sizeDelta.y + 170);
        g.GetComponent<ChatFriendItem>().Structure(message, uiMng, facade, facade.GetUserType());
        chatFrItems.Add(g.GetComponent<ChatFriendItem>());
        StartCoroutine("InsSrollBar");
    }

    public void InsSelfChatItem(string message)
    {
        GameObject g = Instantiate(selfItem);
        g.transform.SetParent(parent.transform);
        g.transform.localScale = new Vector3(1f, 1f, 1f);
        content.sizeDelta = new Vector2(content.sizeDelta.x, +content.sizeDelta.y + 170);

        g.GetComponent<ChatSelfItem>().Structure(message, uiMng, facade, facade.GetUserType());
        chatSelfItems.Add(g.GetComponent<ChatSelfItem>());
        StartCoroutine("InsSrollBar");
    }

    public void ChangeButton(SwitchManager.UserType ut)
    {
        foreach (var sitem in chatSelfItems)
        {
            sitem.ChangeButton(ut);
        }

        foreach (var fitem in chatFrItems)
        {
            fitem.ChangeButton(ut);
        }
    }

    /// <summary>
    /// 刷新下SrollBar 用协程是因为物体绘制要等一帧之后才可以
    /// </summary>
    IEnumerator InsSrollBar()
    {
        yield return new WaitForEndOfFrame();

        chatScrollBar.value = 0;//TODO 滑动条问题要更新 contentSize有问题需要改改
    }
#if UNITY_STANDALONE_WIN //TODO 估计是手语版本聊天窗口
    public void OnHandClick()
    {
        message.SetActive(true);
        Invoke("MesDestroy", 1);
        if (LeapMotionController)
            return;
        qingKong.SetActive(true);
        allSend.SetActive(true);
        LeapMotionController = Instantiate(LeapController);
        Transform tran = LeapMotionController.transform;
        LeapMotionController.transform.SetParent(mainCameraTransform);
        LeapMotionController.transform.localPosition = tran.position;
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        kuSelf.text = "使用自制手势库";
        kuSystem.text = "使用默认手势库";
    }

    public void OnCleanClick()
    {
        qingKong.SetActive(true);
        allSend.SetActive(true);
        LeapMotionController = Instantiate(LeapController);
        Transform tran = LeapMotionController.transform;
        LeapMotionController.transform.SetParent(mainCameraTransform);
        LeapMotionController.transform.localPosition = tran.position;
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
    }

    public void MesDestroy()
    {
        message.SetActive(false);
    }
#endif
}