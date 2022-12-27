using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DataStuff;
using DataServerInterface;
using System.Runtime.CompilerServices;

namespace Server
{
    /// <summary>
    /// file name: DataServer.cs
    /// author: Kade McGarraghy (reference: Tutorial 1 Worksheet)
    /// purpose: implementation of services for user to get database information from server
    /// date last modified: 23/05/21
    /// </summary>

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class DataServer : DataServerInterface.DataServerInterface
    {
        private readonly DatabaseClass db = DatabaseClass.Instance;
        private static uint logNumber = 0;
        public DataServer()
        {
            
        }


        /// <summary>
        /// gets num of entries in the database
        /// </summary>
        /// <returns></returns>
        
        public int GetNumEntries()
        {
            int numEntries = db.GetNumRecords();

            Log("Returned the number of entries (" + numEntries + ")");

            return numEntries;
        }

        /// <summary>
        /// gets values for account in database by index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="acctNo"></param>
        /// <param name="pin"></param>
        /// <param name="bal"></param>
        /// <param name="fName"></param>
        /// <param name="lName"></param>

        public void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName)
        {
            // Check if index is out-of-range, if it is return an error
            if (index < 0 || index >= db.GetNumRecords())
            {
                Console.WriteLine("Client tried to get a record that was out of range...");
                throw new FaultException<IndexOutOfRangeFault>(new IndexOutOfRangeFault()
                { Issue = "Index was not in range..." });
            }
            acctNo = db.GetAcctNoByIndex(index);
            pin = db.GetPINByIndex(index);
            bal = db.GetBalanceByIndex(index);
            fName = db.GetFirstnameByIndex(index);
            lName = db.GetLastnameByIndex(index);

            Log("Returned values of account " + acctNo + " at index " + index + ". First name: " + fName + ". Last name: " + lName + "with balance of" + bal);
        }

        /// <summary>
        /// method to send every log's output to the console
        /// </summary>
        /// <param name="logString"></param>

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Log(string logString)
        {
            logNumber = logNumber + 1;

            System.Console.WriteLine(logString);
            System.Console.WriteLine("Tasks performed so far: " + logNumber);


        }
    }
}
