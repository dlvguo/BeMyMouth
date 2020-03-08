using BMMServer.Models;
using BMMServer.Servers;
using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BMMServer.Services
{
    public class UserCtrlService : BaseCtrlService
    {

        public UserCtrlService()
        {
            controllerCode = ControllerCode.User;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="data"></param>
        /// <param name="client"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public string Login(string data, Client client, Server server)
        {
            Console.WriteLine("Login方法被调用了!" + "when " + DateTime.Now);
            string[] strs = data.Split(',');
            bool isHave = true;// userDAO.VerifyUser(client.MySQLconn, strs[0], strs[1]);
            if (isHave)
            {
                // TODO User user = userDAO.GetUserInfoByUserName(client.MySQLconn, strs[0]);
                User user = new User();
                string newData = user.Id.ToString() + ',' + user.UserName + ',' + user.NickName + ',' +
                                 user.IsLoginFirst.ToString() + ',' + ((int)ReturnCode.Success).ToString();
                client.ClientUserId = user.Id;
                server.AddOnLineUserId(user.Id);
                return newData;
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString();
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="data"></param>
        /// <param name="client"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public string Register(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            string username = strs[0];
            string password = strs[1];
            bool isSuccess = true;// userDAO.AddUSer(client.MySQLconn, username, password);
            if (isSuccess)
            {
                return ((int)ReturnCode.Success).ToString();
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString();
            }
        }

        /// <summary>
        /// 用户名重复验证
        /// </summary>
        /// <param name="data"></param>
        /// <param name="client"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public string VerifyRepeat(string data, Client client, Server server)
        {
            Console.WriteLine(data);
            bool isRepate = false;//= userDAO.VerifyRepate(client.MySQLconn, data);
            if (isRepate)
            {
                return ((int)ReturnCode.Fail).ToString();
            }
            else
            {
                return ((int)ReturnCode.Success).ToString();
            }
        }

        /// <summary>
        /// 设置首次登陆
        /// </summary>
        /// <param name="data"></param>
        /// <param name="client"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public string SetFirstLoginInformation(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            int id = int.Parse(strs[0]);
            string nickname = strs[1];
            bool isSuccess = false;//= userDAO.SetFirstLoginInformationById(client.MySQLconn, id, nickname);
            if (isSuccess)
            {
                return ((int)ReturnCode.Success).ToString();
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString();
            }
        }

        /// <summary>
        /// 查找好友
        /// </summary>
        /// <param name="data"></param>
        /// <param name="client"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public string SearchFriend(string data, Client client, Server server)
        {
            string s = "";// userDAO.GetUsersByNickname(client.MySQLconn, data);
            if (string.IsNullOrEmpty(s))
            {
                return "r";
            }

            return s + '|' + data;
        }

        /// <summary>
        /// 同意添加好友
        /// </summary>
        /// <param name="data"></param>
        /// <param name="client"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public string ApplyForAddFriend(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            int id = 0;//= userDAO.GetUserInfoByUserName(client.MySQLconn, strs[0]).Id;
            Client targetClient = server.GetOnlineClientById(id);
            if (targetClient == null)
            {
                return null;
            }

            string broadcastData = strs[1] + ',' + strs[2];
            client.BroadcastMessage(targetClient, RequestCode.SendApplyNotice, broadcastData);
            return null;
        }

        /// <summary>
        /// 加好友
        /// </summary>
        /// <param name="data"></param>
        /// <param name="client"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public string AddFriend(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            int oppositeId = int.Parse(strs[0]);
            int selfId = int.Parse(strs[1]);
            //userDAO.AddFriendById(client.MySQLconn, oppositeId, selfId);
            Client targetClient = server.GetOnlineClientById(oppositeId);
            client.BroadcastMessage(targetClient, RequestCode.GetFriendList,
                GetFriendList(oppositeId.ToString(), targetClient, server));
            client.BroadcastMessage(client, RequestCode.GetFriendList,
                GetFriendList(selfId.ToString(), client, server));
            /*无效*/
            //GetFriendList(client.ClientUserId.ToString(), client, server);
            //if (targetClient != null)
            //{
            //    GetFriendList(targetClient.ClientUserId.ToString(), targetClient, server);
            //}
            return null;
        }

        /// <summary>
        /// 获取好友列表
        /// </summary>
        /// <param name="data"></param>
        /// <param name="client"></param>
        /// <param name="server"></param>
        /// <returns></returns>
        public string GetFriendList(string data, Client client, Server server)
        {
            Console.WriteLine(data);
            StringBuilder sb = new StringBuilder();
            int id = int.Parse(data);
            string s = string.Empty;//= userDAO.GetFriendsIdById(client.MySQLconn, id);
            if (!string.IsNullOrEmpty(s))
            {
                string[] strs = s.Split(',');
                foreach (string ids in strs)
                {
                    sb.Append(ids);
                    sb.Append(',');
                    //sb.Append(userDAO.GetNicknameById(client.MySQLconn, int.Parse(ids)));
                    sb.Append(',');
                    if (server.IsOnline(int.Parse(ids)))
                    {
                        sb.Append('1');
                    }
                    else
                    {
                        sb.Append('0');
                    }

                    sb.Append('|');
                }
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
                return ((int)ReturnCode.Success).ToString() + '#' + sb.ToString();
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString();
            }
        }
    }
}

