using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DataStuff;

namespace Server
{
    /// <summary>
    /// file name: Program.cs
    /// author: Kade McGarraghy (reference: Tutorial 1 Worksheet)
    /// purpose: implementation of services for user to get database information from server
    /// date last modified: 23/05/21
    /// </summary>

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("WELCOME! You have somehow made it to the server.");
            ServiceHost host;
            NetTcpBinding tcp = new NetTcpBinding();
            host = new ServiceHost(typeof(DataServer));

             host.AddServiceEndpoint(typeof(DataServerInterface.DataServerInterface), tcp,
            "net.tcp://0.0.0.0:8100/DataService"); // 0.0.0.0 tells .net to accept any interface

            host.Open();
            Console.WriteLine("System Online");
            Console.ReadLine();

            host.Close();

        }
    }
}
