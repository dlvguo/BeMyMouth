using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum UserType
{
    Normal,
    Deaf
}

public class ChatPanel : BasePanel
{

    //当前聊天用户ID
    public int NowChatId { get; set; } = -1;
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

    //手模型
    public GameObject leapCtrlModel;
    private GameObject leapModel;

    public GameObject leapController;


#if UNITY_STANDALONE_WIN  //手语需要

    public GameObject qingKong;
    public GameObject allSend;

    public GameObject message;
#endif

    private Transform mainCameraTransform;
    private Canvas canvas;


    public GameObject leapMotionController;

    private Text kuSelf;
    private Text kuSystem;
    private UserType userType;

    //俩个Button
    public Button voiceButton;
    public Button handButton;
    public Button ctButton;

    private void Awake()
    {

        mainCameraTransform = GameObject.Find("Main Camera").transform;
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        userType = UserType.Normal;
#if UNITY_STANDALONE_WIN || DEBUG
        //初始化Model
        leapModel = Instantiate(leapCtrlModel);
        Transform tran = leapModel.transform;
        leapModel.transform.SetParent(mainCameraTransform);
        leapModel.transform.localPosition = tran.position;
        leapModel.SetActive(false);

        kuSelf = GameObject.FindWithTag("kuSelf").GetComponent<Text>();
        kuSystem = GameObject.FindWithTag("kuSystem").GetComponent<Text>();
        qingKong.SetActive(false);
        allSend.SetActive(false);
        message.SetActive(false);

#endif
    }

    public override void OnExit()
    {
        gameObject.SetActive(false);
    }

    public override void OnEnter()
    {
        gameObject.SetActive(true);
    }

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

        if (syncSetTypeButton)
        {
            SetTypeButton(userType);
            syncSetTypeButton = false;
        }
    }

    //删除聊天消息
    public void DestroyChatItem()
    {
        if (chatSelfItems.Count == 0 && chatFrItems.Count == 0)
            return;
        BroadcastMessage("Destroy");//TODO 删除应该是有问题的
    }


    public override void InitPanelThings()
    {
        //添加返回按钮
        transform.Find("frName/back").GetComponent<Button>().onClick.AddListener(() => { uiMng.PopPanel(); });
        ctButton.GetComponent<Button>().onClick.AddListener(ChangeChatUserType);
        frName = transform.Find("frName/Text").GetComponent<Text>();
        msIF = transform.Find("ms/msIF").GetComponent<InputField>();
        transform.Find("ms/sendBtn").GetComponent<Button>().onClick.AddListener(OnSendButtonClick);
        scmRequest = GetComponent<SendToSaveChatMessageRequest>();
        keyboardSelector = GetComponent<KeyboardSelector>();
        base.InitPanelThings();
    }

    //设置朋友名称
    public void SetFrName(string name)
    {
        frName.text = name;
    }

    //更新下Size
    public void InitContentSize()
    {
        content.sizeDelta = Vector2.zero;

    }

    //发送消息
    private void OnSendButtonClick()
    {
        if (string.IsNullOrEmpty(msIF.text) || NowChatId == 0)
        {
            uiMng.ShowPanelMessage(UIPanelType.ChatPanel, "对方不在线,这是哥在测试");//TODO 聊天窗口测试用的
            return;
        }

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        string data = msIF.text + ',' + NowChatId;
        InsSelfChatItem(msIF.text);
        scmRequest.SendRequest(data);
        GetComponentInChildren<ClearInputFiled>().ResetIF();
    }

    //接收朋友消息
    public void OnReciveChatMessageResponse(string data)
    {
        string[] strs = data.Split(',');
        foreach (var message in strs)
        {
            insChatFrItemDatas.Add(message);
        }
        isTimeToInsChatFrItem = true;
    }

    //同步消息
    private string syncInsFrChatItemMessage;

    public void SyncInsFrChatItem(string message)
    {
        syncInsFrChatItemMessage = message;
    }

    bool syncSetTypeButton = false;
    //改变聊天用户模式
    public void ChangeChatUserType()
    {
        if (userType == UserType.Normal)
        {
            userType = UserType.Deaf;
        }
        else
        {
            userType = UserType.Normal;

        }
        msIF.text = string.Empty;
        syncSetTypeButton = true;
        //ctButton.OnUserTypeChange(userType);
        ChangeChatItemsButtonType(userType);
    }

    private void SetTypeButton(UserType ut)
    {
        if (ut == UserType.Deaf)
        {
            voiceButton.gameObject.SetActive(false);
            handButton.gameObject.SetActive(true);
        }
        else
        {
            handButton.gameObject.SetActive(false);
            voiceButton.gameObject.SetActive(true);
        }
    }


    public UserType GetUserType()
    {
        return userType;
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
        g.GetComponent<ChatFriendItem>().Structure(message, uiMng, facade, userType);
        chatFrItems.Add(g.GetComponent<ChatFriendItem>());
        StartCoroutine("InsSrollBar");
    }

    //刷新聊天记录
    public void InsSelfChatItem(string message)
    {
        GameObject g = Instantiate(selfItem);
        g.transform.SetParent(parent.transform);
        g.transform.localScale = new Vector3(1f, 1f, 1f);
        content.sizeDelta = new Vector2(content.sizeDelta.x, +content.sizeDelta.y + 170);

        g.GetComponent<ChatSelfItem>().Structure(message, uiMng, facade, userType);
        chatSelfItems.Add(g.GetComponent<ChatSelfItem>());
        StartCoroutine("InsSrollBar");
    }

    //更改Button模式
    public void ChangeChatItemsButtonType(UserType ut)
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
#if UNITY_STANDALONE_WIN 
    //TODO 估计是手语版本聊天窗口
    public void OnHandClick()
    {
        message.SetActive(true);
        Invoke("MesDestroy", 1);
        if (leapMotionController)
            return;
        qingKong.SetActive(true);
        allSend.SetActive(true);
        leapModel.SetActive(true);
        //TODO这里更改
        //leapMotionController = Instantiate(leapController);
        //Transform tran = leapMotionController.transform;
        //leapMotionController.transform.SetParent(mainCameraTransform);
        //leapMotionController.transform.localPosition = tran.position;
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        kuSelf.text = "使用自制手势库";
        kuSystem.text = "使用默认手势库";
    }

    //清空
    public void OnCleanClick()
    {
        qingKong.SetActive(true);
        allSend.SetActive(true);
        leapMotionController = Instantiate(leapController);
        Transform tran = leapMotionController.transform;
        leapMotionController.transform.SetParent(mainCameraTransform);
        leapMotionController.transform.localPosition = tran.position;
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
    }

    public void MesDestroy()
    {
        message.SetActive(false);
    }
#endif
}