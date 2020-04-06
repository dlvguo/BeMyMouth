using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

/// <summary>
/// 获取申请好友回应
/// </summary>
public class SendApplyNoticeResponse : BaseRequest
{
    private MainPanel mainPanel;
    private void Awake()
    {
        mainPanel = GetComponent<MainPanel>();
        requestCode = RequestCode.SendApplyNotice;
        base.Awake();
    }
    public override void OnResPonse(string data)
    {
        mainPanel.OnSendApplyNoticeResponse(data);
    }
}
