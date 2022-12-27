using DataServerInterface;
using Server;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace BusinessTier
{
    /// <summary>
    /// file name: BusinessServer.cs
    /// author: Kade McGarraghy 
    /// purpose: implementation of services for user to get database information from business server which accesses data server
    /// date last modified: 23/05/21
    /// </summary>

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class BusinessServer : BusinessServerInterface
    {
        private Server.DataServerInterface foob;
        private uint logNumber;

        public BusinessServer()
        {
            var tcp = new NetTcpBinding();
            var URL = "net.tcp://localhost:8100/DataService";
            ChannelFactory<Server.DataServerInterface> foobFactory = new ChannelFactory<Server.DataServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
        }

        public int GetNumEntries()
        {
            int numEntries = foob.GetNumEntries();

            Log("Returned the number of entries (" + numEntries + ")");

            return numEntries;
        }

        public void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName)
        {
            foob.GetValuesForEntry(index, out acctNo, out pin, out bal, out fName, out lName);

            Log("Returned values of account " + acctNo + " at index " + index + ". First name: " + fName + ". Last name: " + lName);
        }

        public int SearchByLastname(string searchTerm)
        {
            int index = -1;

            for (int i = 0; i < GetNumEntries(); i++)
            {
                foob.GetValuesForEntry(i, out var accNo, out var pin, out var bal, out var fName, out var lName);
                if (searchTerm.Equals(lName))
                {
                    index = i;
                    i = GetNumEntries();
                    Log("Searched for last name: " + searchTerm + ". Found account at index " + index);
                }
            }

            if (index == -1)
            {
                Log("Searched for last name: " + searchTerm + ". Account not found.");
            }

            return index;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Log(string logString)
        {
            logNumber = logNumber + 1;

            System.Console.WriteLine(logString);
            System.Console.WriteLine("Tasks performed so far: " + logNumber);


        }
    }
}
