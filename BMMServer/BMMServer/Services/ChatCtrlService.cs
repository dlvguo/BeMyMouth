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

        //TO聊天记录未保存
        public string SendAndSaveChatMessage(string data, Client client, Server server)
        {

            string[] strs = data.Split(',');
            int id = int.Parse(strs[1]);
            // TODO
            //chatDAO.SaveMessage(client.MySQLconn, strs[0], client.ClientUserId, id, DateTime.Now);
            Client targetClient = server.GetOnlineClientById(id);
            {
                if (targetClient != null)
                {
                    string broadcastData = strs[0] + ',' + client.ClientUserId.ToString();
                    client.BroadcastMessage(targetClient, RequestCode.SendMessage, broadcastData);
                }
            }
            //if (targetClient == null)
            //{
            //    //TODO 
            //}
            //string message = strs[0] + ',' + client.ClientUserId.ToString();
            //client.BroadcastMessage(targetClient, RequestCode.ReciveChatMessage, message);
            return null;
        }
    }
}
