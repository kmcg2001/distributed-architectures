using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankClasses
{
    // File name: TransactionDataStruct.cs
    // Author: Kade McGarraghy
    // Purpose: data construct to transaction details
    // Date last modified: 

    public class TransactionDataStruct
    {
        public uint id;
        public uint sendingAccountID;
        public uint receivingAccountID;
        public uint amount;
    }
}
