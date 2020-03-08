using System;
using System.Linq;
using BMMServer.DBS;

namespace BMMServer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (BeMyMouthDB db = new BeMyMouthDB())
            {
                Console.WriteLine(db.Users.Count());
                Console.WriteLine(db.Messages.Count());
                Console.WriteLine(db.Friends.Count());
                Console.Read();
            }
        }
    }
}
