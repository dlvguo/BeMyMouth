using BMMServer.Servers;
using Common;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BMMServer.Services
{
    public class ServiceManger
    {
        private Dictionary<ControllerCode, BaseCtrlService> serviceDict = new Dictionary<ControllerCode, BaseCtrlService>();
        private Server server;

        public ServiceManger(Server server)
        {
            this.server = server;
            InitService();
        }

        //初始化Service
        private void InitService()
        {
            serviceDict.Add(ControllerCode.User, new UserCtrlService());
            serviceDict.Add(ControllerCode.Chat, new ChatCtrlService());

        }
        //处理请求
        public void HandleRequest(ControllerCode controllerCode, RequestCode requestCode, string data, Client client)
        {
            //TODO根据这里添加需求即可
            BaseCtrlService controller;
            bool isGet = serviceDict.TryGetValue(controllerCode, out controller);
            if (!isGet)
            {
                Console.WriteLine("Cant Get " + requestCode + " Handler");
                return;
            }
            //使用反射获取方法
            string methodName = Enum.GetName(typeof(RequestCode), requestCode);
            MethodInfo mi = controller.GetType().GetMethod(methodName);
            if (mi == null)
            {
                Console.WriteLine("[warning]no method:<" + controller.GetType() + "." + methodName + ">");
            }
            Console.WriteLine(data);
            object[] parameters = new object[] { data, client, server };
            object o = mi.Invoke(controller, parameters);
            if (o == null || string.IsNullOrEmpty(o as string))
            {
                return;
            }
            server.SendResponse(client, requestCode, o as string);
        }
    }
}
