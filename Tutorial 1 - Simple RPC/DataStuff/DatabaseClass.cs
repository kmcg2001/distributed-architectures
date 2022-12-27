using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStuff
{
    /// <summary>
    /// file name: DatabaseClass.cs
    /// author: Kade McGarraghy (reference: Tutorial 1 Worksheet)
    /// purpose: implementation for database -> holds data records
    /// date last modified: 23/05/21
    /// </summary>

    public class DatabaseClass
    {
        private List<DataStruct> database;
        private EntryGenerator generator;

        public static DatabaseClass Instance { get; } = new DatabaseClass();


        public DatabaseClass()
        {
            database = new List<DataStuff.DataStruct>();
            generator = new EntryGenerator();

            for (int i = 0; i < 100; i++)
            {
                DataStruct data = new DataStruct();
                generator.GetNextAccount(out data.pin, out data.acctNo, out data.firstname, out data.lastname, out data.balance);
                database.Add(data);
            }

        }

        public uint GetAcctNoByIndex(int index)
        {
            return database[index].acctNo;
        }
        public uint GetPINByIndex(int index)
        {
            return database[index].pin;
        }
        public string GetFirstnameByIndex(int index)
        {
            return database[index].firstname;
        }
        public string GetLastnameByIndex(int index)
        {
            return database[index].lastname;
        }

        public int GetBalanceByIndex(int index)
        {
            return database[index].balance;
        }

        public int GetNumRecords()
        {
            return database.Count();
        }

    }

}
