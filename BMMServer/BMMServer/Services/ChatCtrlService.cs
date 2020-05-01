using System;
using System.Collections.Generic;
using System.Text;
using BMMServer.Servers;
using Common;

namespace BMMServer.Services
{
    public class ChatCtrlService : BaseCtrlService
    {
        public ChatCtrlService()
        {
            controllerCode = ControllerCode.Chat;
        }

        //TODO 聊天记录未保存
        public string SendAndSaveChatMessage(string data, Client client, Server server)
        {

            string[] strs = data.Split(',');
            int id = int.Parse(strs[1]);
            // TODO 保存聊天记录
            //chatDAO.SaveMessage(client.MySQLconn, strs[0], client.ClientUserId, id, DateTime.Now);
            Client targetClient = server.GetOnlineClientById(id);
            {
                if (targetClient != null)
                {
                    string broadcastData = strs[0] + ',' + client.ClientUserId.ToString();
                    client.BroadcastMessage(targetClient, RequestCode.SendMessage, broadcastData);
                }
            }
            return null;
        }
    }
}
