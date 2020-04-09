using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using UnityEngine.UI;

/// <summary>
/// 面板管理  控制所有Manger
/// </summary>
public class Facade : MonoBehaviour
{
    private static Facade _instance;


    private UIManager uiMng;
    private RequestManager requestMng;
    private ClientManager clientMng;
    private UserManager userMng;
    private MessagesManager mesMng;
    private SwitchManager swiMng;
    public static Facade Instance
    {
        get { return _instance; }
    }
    private void Awake()
    {
        _instance = this;
        if (_instance == null)
        {
            new GameObject().AddComponent<Facade>();
            _instance = this;
        }
    }
    private void Start()
    {
        InitManager();
    }

    //每帧更新Manger
    private void Update()
    {
        UpdateManager();
    }

    private void OnDestroy()
    {
        DestroyManager();
    }

    //初始化面板
    private void InitManager()
    {
        uiMng = new UIManager(this);
        requestMng = new RequestManager(this);
        clientMng = new ClientManager(this);
        userMng = new UserManager(this);
        mesMng = new MessagesManager(this);
        swiMng = new SwitchManager(this);
        uiMng.OnInit();
        requestMng.OnInit();
        clientMng.OnInit();
    }

    private void UpdateManager()
    {
        uiMng.Update();
        requestMng.Update();
        clientMng.Update();
        userMng.Update();
        mesMng.Update();
        swiMng.Update();
    }

    private void DestroyManager()
    {
        uiMng.OnDestroy();
        requestMng.OnDestroy();
        clientMng.OnDestroy();
        userMng.OnDestroy();
        mesMng.OnDestroy();
        swiMng.OnDestroy();
    }

    public void Quit()
    {
        Application.Quit();
    }

    //取消请求
    public void RemoveRequest(RequestCode requestCode)
    {
        requestMng.RemoveRequest(requestCode);
    }

    /// <summary>
    /// 处理回应
    /// </summary>
    /// <param name="requestCode">请求代码</param>
    /// <param name="data"></param>
    public void HandleResponse(RequestCode requestCode, string data)
    {
        requestMng.HandleResPonse(requestCode, data);
    }

    /// <summary>
    /// 添加请求
    /// </summary>
    /// <param name="requestCode"></param>
    /// <param name="request"></param>
    public void AddRequest(RequestCode requestCode, BaseRequest request)
    {
        requestMng.AddRequest(requestCode, request);
    }

    /// <summary>
    /// 发送请求
    /// </summary>
    /// <param name="controllerCode"></param>
    /// <param name="requestCode"></param>
    /// <param name="data"></param>
    public void SendRequest(ControllerCode controllerCode, RequestCode requestCode, string data)
    {
        clientMng.SendRequest(controllerCode, requestCode, data);
    }


    /// <summary>
    /// 设置用户信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="username"></param>
    /// <param name="nickname"></param>
    /// <param name="ifl"></param>
    public void SetUserInformation(int id, string username, string nickname, bool ifl)
    {
        userMng.SetUserInformation(id, username, nickname, ifl);
    }

    public bool IsFirstTimeLogin()
    {
        return userMng.IsFirstTimeLogin();
    }

    /// <summary>
    /// 设置用户昵称
    /// </summary>
    /// <param name="nickname"></param>
    public void SetNickName(string nickname)
    {
        userMng.SetNickName(nickname);
    }

    /// <summary>
    /// 获取用户ID
    /// </summary>
    /// <returns></returns>
    public int GetUserID()
    {
        return userMng.GetUserID();
    }

    /// <summary>
    /// 获取用户名
    /// </summary>
    /// <returns></returns>
    public string GetNickname()
    {
        return userMng.GetNickname();
    }

    /// <summary>
    /// 添加朋友信息
    /// </summary>
    /// <param name="fr"></param>
    public void AddFriendInformation(Friend fr)
    {
        userMng.AddFriendInformation(fr);
    }

    /// <summary>
    /// 清空朋友信息
    /// </summary>
    public void ClearFriendInformation()
    {
        userMng.ClearFriendInformation();
    }

    /// <summary>
    /// 获取朋友们
    /// </summary>
    /// <returns></returns>
    public List<Friend> GetFriends()
    {
        return userMng.GetFriends();
    }

    /// <summary>
    /// 添加消息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="m"></param>
    public void AddMessage(int id, string m)
    {
        mesMng.AddMessage(id, m);
    }

    /// <summary>
    /// 获取消息 有点BUG 就是这个消息只有记录对方的
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<string> GetMessage(int id)
    {
        return mesMng.GetMessage(id);
    }

    /// <summary>
    /// 更新消息 //TODO 可能理解错误
    /// </summary>
    /// <param name="message"></param>
    public void SyncInsFrChatItem(string message)
    {
        (uiMng.GetPanel(UIPanelType.ChatPanel) as ChatPanel).SyncInsFrChatItem(message);
    }

    /// <summary>
    /// 获取当前聊天ID
    /// </summary>
    /// <returns></returns>
    public int NowChatID()
    {
        return (uiMng.GetPanel(UIPanelType.ChatPanel) as ChatPanel).NowChatId;
    }

    public void SyncShowNotification(int id)
    {
        (uiMng.GetPanel(UIPanelType.MainPanel) as MainPanel).SyncShowNotification(id);
    }

    public BaseRequest GetRequest(RequestCode requestCode)
    {
        return requestMng.GetRequest(requestCode);
    }

    public void ChangeType()
    {
        swiMng.ChangeType();
    }

    public new SwitchManager.UserType GetType()
    {
        return swiMng.GetType();
    }
}