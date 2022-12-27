using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIClasses
{
    /// <summary>
    /// file name: PortList.cs 
    /// author: Kade McGarraghy
    /// purpose: represents blockchain transaction with sender, receiver amount
    /// date last modified: 25/5/21
    public class Transaction
    {
        public uint toWalletID;
        public uint fromWalletID;
        public float amount;
    }
}