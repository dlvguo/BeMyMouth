using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMMServer.Servers
{
    public class Message
    {
        public byte[] data { get; } = new byte[1024];
        private int dynamicLength = 0;

        public int DynamicLength => dynamicLength;

        public int RemainSize => data.Length - dynamicLength;

        /// <summary>
        /// 读取网络消息
        /// </summary>
        /// <param name="newDataAmount"></param>
        /// <param name="processDataCallBack"></param>
        public void ReadMessage(int newDataAmount, Action<ControllerCode, RequestCode, string> processDataCallBack)
        {
            dynamicLength += newDataAmount;
            while (true)
            {
                if (dynamicLength <= 4) return;
                int count = BitConverter.ToInt32(data, 0);
                if (dynamicLength - 4 >= count)
                {
                    ControllerCode controllerCode = (ControllerCode)BitConverter.ToInt32(data, 4);
                    RequestCode requestCode = (RequestCode)BitConverter.ToInt32(data, 8);
                    string s = Encoding.UTF8.GetString(data, 12, count - 8);
                    processDataCallBack(controllerCode, requestCode, s);
                    Array.Copy(data, count + 4, data, 0, dynamicLength - 4 - count);
                    dynamicLength -= count + 4;
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 包装消息
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] PackData(RequestCode requestCode, string data)
        {
            byte[] requestCodeBytes = BitConverter.GetBytes((int)requestCode);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            int dataAmount = requestCodeBytes.Length + dataBytes.Length;
            byte[] dataAmountBytes = BitConverter.GetBytes(dataAmount);

            byte[] newBytes = dataAmountBytes.Concat(requestCodeBytes).Concat(dataBytes).ToArray<byte>();

            return newBytes;
        }
    }
}
