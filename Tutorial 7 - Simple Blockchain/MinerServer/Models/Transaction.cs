using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MinerServer.Models
{
    public class Transaction
    {
        public uint toWalletID;
        public uint fromWalletID;
        public uint amount;
    }
}