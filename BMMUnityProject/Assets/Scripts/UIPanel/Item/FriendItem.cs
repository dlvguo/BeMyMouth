using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendItem : BaseItem
{
    public Text nicknameText;
    public Image lineDot;
    public Image notificationDot;
    public int Id { get; set; }
    public string Nickname { get; set; }

    /// <summary>
    /// 显示通知
    /// </summary>
    public void ShowNotification()
    {
        notificationDot.enabled = true;
    }

    /// <summary>
    /// 关闭消息通知
    /// </summary>
    public void CloseNotifaction()
    {
        notificationDot.enabled = false;
    }

    /// <summary>
    /// 展示信息
    /// </summary>
    public void ShowInformation()
    {
        nicknameText.text = Nickname;
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    /// <summary>
    /// 朋友点击跳到聊天窗口
    /// </summary>
    private void OnButtonClick()
    {
        uiMng.PushPanel(UIPanelType.ChatPanel);
        ChatPanel chatPanel = uiMng.GetPanel(UIPanelType.ChatPanel) as ChatPanel;
        chatPanel.NowChatId = Id;
        chatPanel.SetFrName(Nickname);
        chatPanel.DestroyChatItem();
        CloseNotifaction();
        //TODO 注意修改消息显示 因为只显示朋友发送的消息 考虑改下 历史消息也没有
        List<string> messages = facade.GetMessage(Id);
        foreach (string s in messages)
        {
            chatPanel.InsFrChatItem(s);
        }
    }
}