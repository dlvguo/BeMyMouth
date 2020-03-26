using BMMServer.Services;
using Common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace BMMServer.Servers
{
    public class Server
    {
        private IPEndPoint iPEndPoint;
        private Socket serverSocket;
        private List<Client> clientList = new List<Client>();
        private List<int> onLineUserIdList = new List<int>();
        private Dictionary<int, Client> UserIdClientDict = new Dictionary<int, Client>();
        private ServiceManger serviceManger;

        public Server()
        {

        }

        public Server(string ipStr, int port)
        {
            SetIpAndPort(ipStr, port);
            serviceManger = new ServiceManger(this);
        }

        /// <summary>
        /// 处理用户下线
        /// </summary>
        /// <param name="id"></param>
        public void UserOffline(int id)
        {
            onLineUserIdList.Remove(id);
            UserIdClientDict.Remove(id);
        }

        /// <summary>
        /// 获取在线客户端id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Client GetOnlineClientById(int id)
        {
            Client client;
            UserIdClientDict.TryGetValue(id, out client);
            if (client == null)
            {
                foreach (Client c in clientList)
                {
                    if (c.ClientUserId == id)
                    {
                        client = c;
                        UserIdClientDict.Add(id, client);
                        return client;
                    }
                }
            }

            if (client == null)
            {
                return null;
            }
            else
            {
                return client;
            }
        }

        /// <summary>
        /// 用户上线添加id
        /// </summary>
        /// <param name="id"></param>
        public void AddOnLineUserId(int id)
        {
            onLineUserIdList.Add(id);
        }

        /// <summary>
        /// 判断是否在线
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsOnline(int id)
        {
            if (onLineUserIdList.Contains(id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetIpAndPort(string ipStr, int port)
        {
            iPEndPoint = new IPEndPoint(IPAddress.Parse(ipStr), port);
        }

        public void Start()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(iPEndPoint);
            serverSocket.Listen(0);
            serverSocket.BeginAccept(AcceptCallBack, null);
            Console.WriteLine("服务器已启动(ﾉ>ω<)ﾉ...");
        }

        private void AcceptCallBack(IAsyncResult ar)
        {
            Socket clientSocket = serverSocket.EndAccept(ar);
            Client client = new Client(clientSocket, this);
            Console.WriteLine("新的客户端接入了！");

            client.Start();
            clientList.Add(client);

            serverSocket.BeginAccept(AcceptCallBack, null);
        }

        public void RemoveClient(Client client)
        {
            lock (clientList)
            {
                clientList.Remove(client);
            }
        }

        /// <summary>
        /// 返回响应
        /// </summary>
        /// <param name="client"></param>
        /// <param name="requestCode"></param>
        /// <param name="data"></param>
        public void SendResponse(Client client, RequestCode requestCode, string data)
        {
            try
            {
                client.Send(requestCode, data);
            }
            catch (Exception e)
            {
                Console.WriteLine("在Send Response时发生了错误:" + e);
            }
        }

        /// <summary>
        /// 分发请求
        /// </summary>
        /// <param name="controllerCode"></param>
        /// <param name="requestCode"></param>
        /// <param name="data"></param>
        /// <param name="client"></param>
        public void HandleRequest(ControllerCode controllerCode, RequestCode requestCode, string data, Client client)
        {
            serviceManger.HandleRequest(controllerCode, requestCode, data, client);
        }
    }
}
