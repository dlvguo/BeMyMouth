using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

//UI管理
public class UIManager : BaseManager
{
    private Dictionary<UIPanelType, string> panelPathDict; //存储所有面板Prefab的路径
    private Dictionary<UIPanelType, BasePanel> panelDict; //保存所有实例化面板的游戏物体身上的BasePanel组件
    private Dictionary<string, Sprite> lineDotDict;
    public Stack<BasePanel> panelStack;

    private UIPanelType panelTypeToPush = UIPanelType.None;
    private List<UIPanelType> panelTypesToPush = null;
    private bool isSyncPopPanel = false;

    private bool isSyncShowPanelMessage = false;
    private UIPanelType syncSplPanelType;
    private string syncSplmsg;

    private bool isSyncShowIFWarningMessage = false;
    private InputField syncSIWMiF;
    private string syncSIWMmsg;

    private string dotOnPath = "Img/doton";
    private string dotOffPath = "Img/dotoff";

    public Sprite GetLineDot(string lineState)
    {
        return lineDotDict.TryGet(lineState);
    }

    private void PreLoadLineDot()
    {
        if (lineDotDict == null)
        {
            lineDotDict = new Dictionary<string, Sprite>();
        }

        Sprite onlineDot = GameObject.Instantiate(Resources.Load(dotOnPath)) as Sprite;
        Sprite offlineDot = GameObject.Instantiate(Resources.Load(dotOffPath)) as Sprite;

        lineDotDict.Add("online", onlineDot);
        lineDotDict.Add("offline", offlineDot);
    }

    public UIManager(Facade facade) : base(facade)
    {
        ParseUIPanelTypeJson();
        PreLoadLineDot();
    }

    private Transform canvasTransform;

    //初始化
    public override void OnInit()
    {
        base.OnInit();
        PushPanel(UIPanelType.LoginPanel);
    }

    //获取画布坐标
    private Transform CanvasTransform
    {
        get
        {
            if (canvasTransform == null)
            {
                canvasTransform = GameObject.Find("Canvas").transform;
            }
            return canvasTransform;
        }
    }


    //public UIManager()
    //{
    //    ParseUIPanelTypeJson();
    //}

    //放进面板中
    public void PushPanel(UIPanelType panelType)
    {
        if (panelStack == null)
            panelStack = new Stack<BasePanel>();

        //判断一下栈里面是否有页面
        if (panelStack.Count > 0)
        {
            BasePanel topPanel = panelStack.Peek();
            if (topPanel)
                topPanel.OnPause();
            else
                panelStack.Pop();
        }

        BasePanel panel = GetPanel(panelType);
        panel.OnEnter();
        panelStack.Push(panel);
        //        Debug.Log(panel.name + "pushed");
    }

    public override void Update()
    {
        if (panelTypesToPush != null)
        {
            foreach (var panelType in panelTypesToPush)
            {
                PushPanel(panelType);
            }

            panelTypesToPush = null;
        }

        if (panelTypeToPush != UIPanelType.None)
        {
            PushPanel(panelTypeToPush);
            panelTypeToPush = UIPanelType.None;
        }

        if (isSyncPopPanel)
        {
            PopPanel();
            isSyncPopPanel = false;
        }

        if (isSyncShowPanelMessage)
        {
            ShowPanelMessage(syncSplPanelType, syncSplmsg);
        }

        if (isSyncShowIFWarningMessage)
        {
            SyncShowIFWarningMessage(syncSIWMiF, syncSIWMmsg);
        }


        //        DictDisp();
    }

    //同步多个面板
    public void PushPanelsSync(List<UIPanelType> panelTypes)
    {
        panelTypesToPush = panelTypes;
    }


    //同步放入面板
    public void PushPanelSync(UIPanelType uiPanelType)
    {
        panelTypeToPush = uiPanelType;
    }

    //同步删除面板
    public void SyncPopPanel()
    {
        isSyncPopPanel = true;
    }


    //这个主线程的方法 因此需要在Update更新
    public void PopPanel()
    {
        if (panelStack == null)
        {
            panelStack = new Stack<BasePanel>();
        }

        if (panelStack.Count <= 0)
        {
            return;
        }

        //关闭栈顶页面的显示
        BasePanel topPanel = panelStack.Pop();
        Debug.Log(topPanel.name + "depushed");
        topPanel.OnExit();

        if (panelStack.Count <= 0) return;
        BasePanel topPanel2 = panelStack.Peek();
        topPanel2.OnResume();
    }


    public void PreLoadPanel(UIPanelType panelType)
    {
        LoadPanel(panelType).gameObject.SetActive(false);
    }

    public void SyncShowIFWarningMessage(InputField inputField, string msg)
    {
        syncSIWMiF = inputField;
        syncSIWMmsg = msg;
    }

    public void ShowIFWarningMessage(InputField inputField, string msg)
    {
        inputField.text = "";
        inputField.placeholder.GetComponent<Text>().fontStyle = FontStyle.Normal;
        inputField.placeholder.GetComponent<Text>().color = Color.red;
        inputField.placeholder.GetComponent<Text>().text = msg;
    }

    public void SyncShowPanelMessage(UIPanelType panelType, string msg)
    {
        syncSplPanelType = panelType;
        syncSplmsg = msg;
        isSyncShowPanelMessage = true;
    }

    public void ShowPanelMessage(UIPanelType panelType, string msg)
    {
        BasePanel panel = GetPanel(panelType);
        panel.GetShowText().text = msg;
    }

    private BasePanel LoadPanel(UIPanelType panelType)
    {
        string path = panelPathDict.TryGet(panelType);
        GameObject instPanel = GameObject.Instantiate(Resources.Load(path)) as GameObject;
        instPanel.transform.SetParent(CanvasTransform, false);
        instPanel.GetComponent<BasePanel>().UIMng = this;
        instPanel.GetComponent<BasePanel>().Facade = facade;
        instPanel.GetComponent<BasePanel>().InitPanelThings();
        panelDict.Add(panelType, instPanel.GetComponent<BasePanel>());
        return instPanel.GetComponent<BasePanel>();
    }

    public BasePanel GetPanel(UIPanelType panelType)
    {
        if (panelDict == null)
        {
            panelDict = new Dictionary<UIPanelType, BasePanel>();
        }

        //BasePanel panel;
        //panelDict.TryGetValue(panelType, out panel);
        BasePanel panel = panelDict.TryGet(panelType);
        if (panel == null)
        {
            //            Debug.Log(panelType.ToString());
            //如果找不到，那么就找这个面板的prefab的路径，然后去根据prefab去实例化面板
            //string path;
            //panelPathDict.TryGetValue(panelType, out path);
            return LoadPanel(panelType);
        }
        else
        {
            return panel;
        }
    }

    //移除字典
    public void RemovePanelDict(UIPanelType uIPanelType)
    {
        if (panelDict == null)
        {
            panelDict = new Dictionary<UIPanelType, BasePanel>();
        }
        panelDict.Remove(uIPanelType);

    }

    [Serializable]
    class UIPanelTypeJson
    {
        public List<UIPanelInfo> infoList;
    }

    /// <summary>
    /// 解析panel路径json
    /// </summary>
    private void ParseUIPanelTypeJson()
    {
        panelPathDict = new Dictionary<UIPanelType, string>();
        TextAsset ta = Resources.Load<TextAsset>("UIPanelType");
        UIPanelTypeJson jsonObject = JsonUtility.FromJson<UIPanelTypeJson>(ta.text);
        foreach (UIPanelInfo info in jsonObject.infoList)
        {
#if UNITY_ANDROID
            info.path = info.path.Insert(7, "AN");
#endif

            panelPathDict.Add(info.panelType, info.path);
        }
    }
}