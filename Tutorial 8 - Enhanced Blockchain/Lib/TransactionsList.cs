using APIClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib

{    /// <summary>
     /// file name: BlockchainServiceInterface.cs 
     /// author: Kade McGarraghy
     /// purpose: represents queue of transactions so transactions can be easily added/deleted from list based on need
     /// date last modified: 25/5/21
    public class TransactionsList
    {
        public static Queue<Transaction> transactions = new Queue<Transaction>();

        public static void AddTransaction(Transaction newTransaction)
        {
            transactions.Enqueue(newTransaction);
        }
    }
}
