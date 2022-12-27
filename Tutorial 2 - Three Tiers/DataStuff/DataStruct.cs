using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStuff
{
    /// <summary>
    /// file name: DataStruct.cs
    /// author: Kade McGarraghy (reference: Tutorial 1 Worksheet)
    /// purpose: represents one user's details
    /// date last modified: 23/05/21
    /// </summary>

    internal class DataStruct
    {
        public string firstname;
        public string lastname;
        public uint pin;
        public uint acctNo;
        public int balance;

        public DataStruct()
        {
            acctNo = 0;
            pin = 0;
            balance = 0;
            firstname = "";
            lastname = "";
        }
    }
}
