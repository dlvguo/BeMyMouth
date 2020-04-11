using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using UnityEngine.UI;

public class LoginPanel : BasePanel
{
    private InputField usernameIF;
    private InputField passwordIF;
    private LoginRequest loginRequest;


    private string syncShowText = "";


    private void Update()
    {
        if (syncShowText != "")
        {
            showText.text = syncShowText;
            syncShowText = "";
        }
    }

    public override void InitPanelThings()
    {
        //        transform.Find("BackButton").GetComponent<Button>().onClick.AddListener(OnBackButtonClick);
        transform.Find("LoginButton").GetComponent<Button>().onClick.AddListener(OnLoginButtonClick);
        transform.Find("createButton").GetComponent<Button>().onClick
            .AddListener(() => { uiMng.PushPanel(UIPanelType.RegPanel); });
        transform.Find("quitButton").GetComponent<Button>().onClick.AddListener(() => { Application.Quit(); });
        usernameIF = transform.Find("usernameIF").GetComponent<InputField>();
        passwordIF = transform.Find("passwordIF").GetComponent<InputField>();

        loginRequest = GetComponent<LoginRequest>();

        base.InitPanelThings();
    }


    private void OnLoginButtonClick()
    {
        string msg = "";
        if (string.IsNullOrEmpty(usernameIF.text))
        {
            uiMng.ShowIFWarningMessage(usernameIF, "用户名不能为空");
            msg += "用户名不能为空";
        }

        if (string.IsNullOrEmpty(passwordIF.text))
        {
            uiMng.ShowIFWarningMessage(passwordIF, "密码不能为空");

            msg += " 密码不能为空";
        }

        if (msg != "")
        {
            return;
        }

        loginRequest.SendRequest(usernameIF.text, passwordIF.text);
    }

    public void OnLoginResponse(ReturnCode returntCode, string nickname)
    {
        if (returntCode == ReturnCode.Success)
        {
            if (facade.IsFirstTimeLogin())
            {
                //首次登录注册
                uiMng.PushPanelSync(UIPanelType.PreMenuPanel);
            }
            else
            {
                List<UIPanelType> uiPanelTypes = new List<UIPanelType>();
                uiPanelTypes.Add(UIPanelType.MainPanel);
                //TODO 讲道理是不需要聊天窗口的 因为有个聊天注册必须ChatPlane实例话才可以 于是先注册一个 再删除一个
                uiPanelTypes.Add(UIPanelType.ChatPanel);
                uiMng.PushPanelsSync(uiPanelTypes);
                uiMng.SyncPopPanel();
                //uiMng.GetPanel(UIPanelType.ChatPanel);//目的初始化下ChatPlane
            }
        }
        else
        {
            syncShowText = "用户名或密码错误，请重新输入";
        }
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
        gameObject.SetActive(false);
    }
}