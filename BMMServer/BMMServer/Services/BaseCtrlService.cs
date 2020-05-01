using BMMServer.Servers;
using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BMMServer.Services
{
    public class BaseCtrlService
    {
        protected ControllerCode controllerCode = ControllerCode.None;
        public ControllerCode ControllerCode
        {
            get
            {
                return controllerCode;
            }
        }

        public virtual string DefaultHandale(string data, Client client, Server server)
        {
            return null;
        }
    }
}
