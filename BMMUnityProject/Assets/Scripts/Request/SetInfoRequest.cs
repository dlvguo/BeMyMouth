using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class SetInfoRequest : BaseRequest
{
    private SetInfoPanel setPanel;

    public override void Awake()
    {
        controllerCode = ControllerCode.User;
        requestCode = RequestCode.SetInfo;
        setPanel = GetComponent<SetInfoPanel>();
        base.Awake();
    }

    public void SendRequest(string id, string nickName, string password)
    {
        //账号和密码
        string data = id + ',' + nickName + ',' + password;
        base.SendRequest(data);
    }

    //事件驱动框架设计
    public override void OnResPonse(string data)
    {

        //ReturnCode returnCode = (ReturnCode) int.Parse(data);
        ReturnCode returnCode = ReturnCode.Success;
        setPanel.OnSetInfoReponse(returnCode);
    }
}
