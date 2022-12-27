using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DataStuff;
using DataServerInterface;

namespace Server
{
    /// <summary>
    /// file name: DataServer.cs
    /// author: Kade McGarraghy (reference: Tutorial 1 Worksheet)
    /// purpose: implementation of services for user to get database information from server
    /// date last modified: 23/05/21
    /// </summary>

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class DataServer : DataServerInterface
    {


        private readonly DatabaseClass db = DatabaseClass.Instance;
        public DataServer()
        {
            
        }

        /// <summary>
        /// gets num of entries in the database
        /// </summary>
        /// <returns></returns>
        public int GetNumEntries()
        { 
            return db.GetNumRecords();
        }

        /// <summary>
        /// gets values of data object's attributes at given index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="acctNo"></param>
        /// <param name="pin"></param>
        /// <param name="bal"></param>
        /// <param name="fName"></param>
        /// <param name="lName"></param>
        public void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal,
        out string fName, out string lName)
        {

            if (index < 0 || index >= db.GetNumRecords()) // if index is negative or higher than num records range
            {
                Console.WriteLine("Error: Client tried to access record with an out of range index.");
                throw new FaultException<IndexOutOfRangeFault>(new IndexOutOfRangeFault()
                { 
                    Issue = "Index not in range."
                });
            }
            else
            {
                acctNo = db.GetAcctNoByIndex(index);
                pin = db.GetPINByIndex(index);
                bal = db.GetBalanceByIndex(index);
                fName = db.GetFirstnameByIndex(index);
                lName = db.GetLastnameByIndex(index);
            }



        }
    }
}
