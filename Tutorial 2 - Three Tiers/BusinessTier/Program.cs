// Alex Starling - Distributed Computing - 2021
using System;
using System.ServiceModel;

namespace BusinessTier
{
    /// <summary>
    /// file name: Program.cs
    /// author: Kade McGarraghy (reference: Tutorial 1 Worksheet)
    /// purpose: starts up and keeps business server running
    /// date last modified: 23/05/21
    /// </summary>

    class Program
    {
        static void Main(string[] args)
        {
   
            Console.WriteLine("Welcome");
            var tcp = new NetTcpBinding();

            var host = new ServiceHost(typeof(BusinessServer));
            host.AddServiceEndpoint(typeof(BusinessServerInterface), tcp, "net.tcp://localhost:8101/BusinessService");
            host.Open();
  
            Console.WriteLine("System Online");
            Console.ReadLine();
            //Close the host
            host.Close();
        }
    }
}
