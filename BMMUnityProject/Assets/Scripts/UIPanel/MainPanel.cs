using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : BasePanel
{
    private Text nickName;
    private GetFriendListRequest getFriendListRequest;

    [SerializeField] private VerticalLayoutGroup parent;
    //朋友表格
    [SerializeField] private GameObject frItem;
    //申请表格
    [SerializeField] private GameObject aPItem;
    [SerializeField] private RectTransform content;

    private bool isTimeToInsFrItem;
    private bool isTimeToShowNotifications;

    [SerializeField] private Button sfButton;
    [SerializeField] private Button msgButton;
    [SerializeField] private Button setInfoButton;
    [SerializeField] private Button teachButton;


    private List<FriendItem> friendItems = new List<FriendItem>();
    private List<int> notificationsIds = new List<int>();


    /// <summary>
    /// 添加消息
    /// </summary>
    /// <param name="data"></param>
    public void AddNotificationsIdAndShow(string data)
    {
        string[] strs = data.Split(',');

        foreach (string ids in strs)
        {
            print(ids);
            notificationsIds.Add(int.Parse(ids));
        }
        isTimeToShowNotifications = true;
    }

    /// <summary>
    /// 显示消息
    /// </summary>
    private void ShowNotifications()
    {
        foreach (FriendItem fi in friendItems)
        {
            if (notificationsIds.Contains(fi.Id))
            {
                fi.ShowNotification();
                notificationsIds.Remove(fi.Id);
            }
        }

        notificationsIds.Clear();
    }

    private int syncShowNotificationId = -1;

    public void SyncShowNotification(int id)
    {
        syncShowNotificationId = id;
    }

    public void ShowNotification(int id)
    {
        foreach (FriendItem fi in friendItems)
        {
            if (fi.Id == id)
            {
                fi.ShowNotification();
            }
        }
    }

    //初始化面板
    public override void InitPanelThings()
    {
        nickName = transform.Find("puser/name").GetComponent<Text>();
        nickName.text = facade.GetNickname();
        getFriendListRequest = GetComponent<GetFriendListRequest>();
        msgButton = transform.Find("msgButton").GetComponent<Button>();
        msgButton.onClick.AddListener(() => { uiMng.PushPanel(UIPanelType.ChatPanel); });
        setInfoButton = transform.Find("InfoSetButton").GetComponent<Button>();
        setInfoButton.onClick.AddListener(() => uiMng.PushPanel(UIPanelType.SetInfoPanel));
        //添加
        teachButton = transform.Find("GestureStudyButton").GetComponent<Button>();
        teachButton.onClick.AddListener(() => uiMng.PushPanel(UIPanelType.TeachPanel));
        //寻找朋友Item
        sfButton.onClick.AddListener(() => { uiMng.PushPanel(UIPanelType.SearchFriendPanel); });
        transform.Find("quitButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            if (uiMng.panelStack.Count == 3)
            {
                uiMng.PopPanel();
                uiMng.PopPanel();
            }
            else
            {
                uiMng.PopPanel();
            }
        });
    }
    private bool isInsName = false;
    //注意更新必须在Update中 不然刷新不上去
    public void SetShowNickName()
    {
        isInsName = true;
    }

    /// <summary>
    /// 刷新朋友面板的时候
    /// </summary>
    public void OnGetFriendListResponse()
    {
        isTimeToInsFrItem = true;
    }

    /// <summary>
    /// 提示消息
    /// </summary>
    /// <param name="data"></param>
    public void OnGetNotificationReponse(string data)
    {
        AddNotificationsIdAndShow(data);
    }

    /// <summary>
    /// 获取消息提醒
    /// </summary>
    /// <param name="data"></param>
    public void OnSendApplyNoticeResponse(string data)
    {
        string[] strs = data.Split(',');
        syncNickName = strs[0];
        syncId = strs[1];
    }

    //同步名称
    private string syncNickName = string.Empty;
    private string syncId = string.Empty;

    /// <summary>
    /// 刷新请求
    /// </summary>
    /// <param name="nickName"></param>
    /// <param name="id"></param>
    private void InsAPItem(string nickName, string id)
    {
        GameObject g = Instantiate(aPItem);
        g.transform.SetParent(parent.transform);
        content.sizeDelta = new Vector2(content.sizeDelta.x, +content.sizeDelta.y + 65);
        g.GetComponent<ApplyForItem>().Structure(id, nickName, uiMng, facade);
    }

    private void Update()
    {
        if (isTimeToInsFrItem)
        {
            InstantiateFriendItem();
            isTimeToInsFrItem = false;
        }

        if (isTimeToShowNotifications)
        {
            ShowNotifications();
            isTimeToShowNotifications = false;
        }

        //刷新通知 如果有消息同时的话
        if (syncShowNotificationId != -1)
        {
            ShowNotification(syncShowNotificationId);
            syncShowNotificationId = -1;
        }

        if (syncNickName != string.Empty || syncId != string.Empty)
        {
            InsAPItem(syncNickName, syncId);
            syncNickName = string.Empty;
            syncId = string.Empty;
        }

        //更新下
        if (isInsName)
        {
            nickName.text = facade.GetNickname();
            isInsName = false;
        }
    }
    public void UpdateLineDot()
    {
    }

    //实例话朋友item
    public void InstantiateFriendItem()
    {
        if (friendItems.Count > 0)
        {
            content.BroadcastMessage("Destroy");
            friendItems.Clear();
        }

        foreach (Friend fr in facade.GetFriends())
        {
            GameObject g = Instantiate(frItem);
            g.transform.SetParent(parent.transform);
            g.transform.localScale = new Vector3(1f, 1f, 1f);
#if UNITY_ANDROID || UNITY_STANDALONE_WIN || UNITY_EDITOR
            content.sizeDelta = new Vector2(content.sizeDelta.x, +content.sizeDelta.y + 150);
#endif
#if MAYBENO
            content.sizeDelta = new Vector2(content.sizeDelta.x, +content.sizeDelta.y + 65);
#endif
            FriendItem fi = g.GetComponent<FriendItem>();
            fi.Nickname = fr.Nickname;
            fi.ShowInformation();
            fi.Id = fr.Id;
            fi.UIMng = uiMng;
            fi.Facade = facade;
            friendItems.Add(fi);
        }
    }

    //更新名称
    public void InsNickName()
    {
        nickName.text = facade.GetNickname();
    }


    /// <summary>
    /// 程序进入的时候
    /// </summary>
    public override void OnEnter()
    {
        getFriendListRequest.SendRequest(facade.GetUserID());
        base.OnEnter();
        gameObject.SetActive(true);

        //        msgButton.onClick.Invoke();
    }


    public override void OnResume()
    {
        //        gameObject.SetActive(true);
    }

    public override void OnPause()
    {
        //        gameObject.SetActive(false);
    }

    public override void OnExit()
    {
        base.OnExit();
        gameObject.SetActive(false);
    }
}