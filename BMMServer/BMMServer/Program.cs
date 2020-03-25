using System;
using System.Linq;
using BMMServer.DBS;
using BMMServer.Servers;

namespace BMMServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server("127.0.0.1", 7788);//172.18.177.108//127.0.0.1
            server.Start();
            Console.ReadKey();
        }
    }
}
