using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

/*
    class: Program.cs
    author: Kade McGarraghy
    purpose: runs authentication server
    date last modified: 27/4/21
*/


namespace Authenticator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Authentication System");
            var tcp = new NetTcpBinding(); // binds tcp interface
            var host = new ServiceHost(typeof(AuthServer)); // creates service host
            host.AddServiceEndpoint(typeof(AuthServerInterface), tcp, "net.tcp://localhost/AuthenticationService"); // other layers will connect to this endpoint to use this service
            host.Open(); 
            Console.WriteLine("Authentication System Online");

           /* bool correctType = false;
            string instruction = "How often (every X minutes) do you want to clear the saved tokens?";
            string outputStr = instruction;
            int xMinutes = 10;
            while (!correctType)
            {
                Console.WriteLine(outputStr);
                string numOperandsInput = Console.ReadLine();
                if (Int32.TryParse(numOperandsInput, out int res)) // makes sure number entered is integer
                {
                    int newNum = Int32.Parse(numOperandsInput);
                    if (newNum > 0) // makes sure number entered is a positive number > 0 (it's a calculator, needs at least 1 operand)
                    {
                        xMinutes = newNum;
                        correctType = true;
                    }

                }
                outputStr = "You need to enter a positive integer number!" + "\n" + instruction;
            }

            AuthServer authServer = new AuthServer();
            authServer.SetXMinutes(xMinutes);*/

            Console.WriteLine("");
            Console.ReadLine(); // keeps the server open
            host.Close();  // closes the host
        }


    }
}
