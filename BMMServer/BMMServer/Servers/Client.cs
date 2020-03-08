using Common;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace BMMServer.Servers
{
    public class Client
    {
        private Socket clientSocket;
        private Server server;

        public int ClientUserId { get; set; }


        private Message msg = new Message();

        public Client()
        {
        }

        public Client(Socket clientSocket, Server server)
        {
            this.clientSocket = clientSocket;
            this.server = server;
        }


        public void Start()
        {
            if (clientSocket == null || clientSocket.Connected == false)
            {
                Console.WriteLine("未找到客户端或客户端远端连接已关闭");
                return;
            }

            clientSocket.BeginReceive(msg.data, msg.DynamicLength, msg.RemainSize, SocketFlags.None, ReciveCallBack,
                null);
        }

        /// <summary>
        /// 客户端接收消息后回调处理
        /// </summary>
        /// <param name="ar"></param>
        public void ReciveCallBack(IAsyncResult ar)
        {
            try
            {
                if (clientSocket == null || clientSocket.Connected == false)
                {
                    Console.WriteLine("未找到客户端或客户端远端连接已关闭");
                    return;
                }

                int count = clientSocket.EndReceive(ar);
                if (count == 0)
                {
                    Close();
                }

                msg.ReadMessage(count, OnProcessMessage);
                Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Close();
            }
        }

        /// <summary>
        /// 向客户端发送消息
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="data"></param>
        public void Send(RequestCode requestCode, string data)
        {
            try
            {
                Console.WriteLine("向客户端发送了一条消息:" + data + "    >>>" + requestCode + "<<<");
                byte[] bytes = Message.PackData(requestCode, data);
                clientSocket.Send(bytes);
            }
            catch (Exception e)
            {
                Console.WriteLine("无法发送消息:" + e);
            }
        }

        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="controllerCode"></param>
        /// <param name="requestCode"></param>
        /// <param name="data"></param>
        private void OnProcessMessage(ControllerCode controllerCode, RequestCode requestCode, string data)
        {
            server.HandleRequest(controllerCode, requestCode, data, this);
        }

        /// <summary>
        /// 关闭客户端连接
        /// </summary>
        private void Close()
        {
            server.UserOffline(ClientUserId);
            server.RemoveClient(this);
            if (clientSocket != null)
            {
                clientSocket.Close();
                Console.WriteLine("一个远程连接已关闭");
            }
        }

        /// <summary>
        /// 广播消息
        /// </summary>
        /// <param name="client"></param>
        /// <param name="requestCode"></param>
        /// <param name="data"></param>
        public void BroadcastMessage(Client client, RequestCode requestCode, string data)
        {
            server.SendResponse(client, requestCode, data);
        }
    }
}