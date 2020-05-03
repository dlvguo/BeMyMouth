using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

//注册请求
public class RegisterRequest : BaseRequest
{
    //注册面板
    private RegPanel regPanel;
    public override void Awake()
    {
        controllerCode = ControllerCode.User;
        requestCode = RequestCode.Register;
        regPanel = GetComponent<RegPanel>();
        base.Awake();
    }
    //发送请求
    public void SendRequest(string mailAddres, string password)
    {
        string data = mailAddres + ',' + password;
        base.SendRequest(data);
    }
    //获取反馈
    public override void OnResPonse(string data)
    {
        ReturnCode returnCode = ReturnCode.Success;
        regPanel.OnRegReponse(returnCode);
    }
}
