using APIClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    /// <summary>
    /// file name: BlockchainService.cs 
    /// author: Kade McGarraghy
    /// purpose: retrieves blockchain info for multiple clients
    /// date last modified: 25/5/21

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false, InstanceContextMode = InstanceContextMode.Single)]

    public class BlockchainService : BlockchainServiceInterface
    {
        /// <summary>
        /// sets up blockchain ONCE
        /// </summary>
        /// <returns></returns>
        public BlockchainService()
        {
            if (Blockchain.blocks.Count == 0)
            {
                Blockchain.AddFirstBlock();
            }
        }


        /// <summary>
        /// gets current blockchain
        /// </summary>
        /// <returns></returns>
        public List<Block> GetCurrentBlockchain()
        {
            return Blockchain.blocks;
        }

        /// <summary>
        /// gets account balances for given id
        /// </summary>
        /// <returns></returns>
        public float GetAccountBalance(uint accountID)
        {
            return Blockchain.GetAccountBalance(accountID);
        }

        /// <summary>
        /// gets most recent block in chain
        /// </summary>
        /// <returns></returns>
        public Block GetCurrentBlock()
        {
            return Blockchain.blocks[Blockchain.blocks.Count - 1];
        }

        /// <summary>
        /// receives new transaction from a client to add to transaction queue
        /// </summary>
        /// <returns></returns>
        public void ReceiveNewTransaction(Transaction newTransaction)
        {
            TransactionsList.AddTransaction(newTransaction);
        }

    }
}
