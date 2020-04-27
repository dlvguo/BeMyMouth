using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Common;
using UnityEngine;

//客户端连接管理
public class ClientManager : BaseManager
{
    public ClientManager(Facade facade) : base(facade)
    {
    }

    private const string _ip = "127.0.0.1"; //47.106.254.223//127.0.0.1
    private const int _port = 7788;
    private Socket clientSocket;
    private Message msg = new Message();

    public override void OnInit()
    {
        base.OnInit();
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            clientSocket.Connect(_ip, _port);
            Start();
        }

        catch (Exception e)
        {
            Debug.Log("Wrong:无法连接到服务器端，请检查您的网络.." + e);
        }
    }
    private void Start()
    {
        if (clientSocket == null || clientSocket.Connected == false)
        {
            Debug.Log("dwa");
            return;
        }
        clientSocket.BeginReceive(msg.Data, msg.dynamicLength, msg.RemainSize, SocketFlags.None, ReciveCallBack, null);
    }

    //接收回调
    private void ReciveCallBack(IAsyncResult ar)
    {

        try
        {
            if (clientSocket == null || clientSocket.Connected == false)
            {
                return;
            }
            int count = clientSocket.EndReceive(ar);
            msg.ReadMessage(count, OnProcessDataCallBack);
            Start();

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

    }

    //处理回调反馈
    private void OnProcessDataCallBack(RequestCode requestCode, string data)
    {
        facade.HandleResponse(requestCode, data);
    }

    public void SendRequest(ControllerCode controllerCode, RequestCode requestCode, string data)
    {
        byte[] bytes = Message.PackData(controllerCode, requestCode, data);
        clientSocket.Send(bytes);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        try
        {
            clientSocket.Close();
        }
        catch (Exception e)
        {
            Debug.Log("无法关闭与服务器的连接" + e);
        }
    }
}