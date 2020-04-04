using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

//消息管理
public class MessagesManager : BaseManager
{
    public MessagesManager(Facade facade) : base(facade)
    {
    }

    //    private Dictionary<int,string> messagesDic=new Dictionary<int, string>();
    private RepeatableDictionary<int, string> messagesRdic = new RepeatableDictionary<int, string>();

    public void AddMessage(int id, string m)
    {
        messagesRdic.Add(id, m);
        ProcessMessage(id, m);
    }

    /// <summary>
    /// 获取消息
    /// </summary>
    /// <param name="id">用户ID</param>
    /// <returns></returns>
    public List<string> GetMessage(int id)
    {
        return messagesRdic.GetAllValue(id);
    }

    /// <summary>
    /// 处理消息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="m"></param>
    private void ProcessMessage(int id, string m)
    {
        if (facade.NowChatID() == id)
        {
            ShowMessage(m);
        }
        else
        {
            ShowNotification(id);
        }
    }

    /// <summary>
    /// 展示消息
    /// </summary>
    /// <param name="message"></param>
    private void ShowMessage(string message)
    {
        facade.SyncInsFrChatItem(message);
    }

    /// <summary>
    /// 展示通知
    /// </summary>
    /// <param name="id"></param>
    private void ShowNotification(int id)
    {
        facade.SyncShowNotification(id);
    }
}