using APIClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BlockchainServer.Models
{
    /// <summary>
    /// file name: Blockchain.cs
    /// author: Kade McGarraghy
    /// purpose: represents chain of blocks and contains methods to view/edit/maintain blockchain
    /// date: 24/5/21
    /// </summary>
    public class Blockchain
    {
        public static List<Block> blocks = new List<Block>();

        /// <summary>
        /// sets up blockchain with first block
        /// </summary>
        public Blockchain()
        {
            Block firstBlock = new Block();

            firstBlock.id = 0;
            firstBlock.toWalletID = 0;
            firstBlock.fromWalletID = 0;
            firstBlock.amount = 0;
            firstBlock.prevBlockHash = "";
            firstBlock.blockOffset = 1;
            firstBlock = HashBlock(firstBlock);

            blocks.Add(firstBlock);
        }

        /// <summary>
        /// adds block to blockchain
        /// </summary>
        /// <param name="block"></param>
        /// <returns></returns>
        
        public bool AddBlock(Block block)
        {
            bool validBlock = true;

            if (block.id <= GetHighestBlockID() || block.id < 1)
            {
                validBlock = false;
                System.Diagnostics.Debug.WriteLine("ERROR: id" + block.id);
            }

            if (block.amount <= 0)
            {
                validBlock = false;
                System.Diagnostics.Debug.WriteLine("ERROR: block amount: " + block.amount);
            }

            if (((int) block.blockOffset % 5) != 0 || block.blockOffset < 0)
            {
                validBlock = false;
                System.Diagnostics.Debug.WriteLine("ERROR: offset:" + block.blockOffset);
            }

            if (block.prevBlockHash != blocks[(int) block.id - 1].currBlockHash)
            {
                validBlock = false;
                System.Diagnostics.Debug.WriteLine("ERROR: invalid hash, current hash:" + block.currBlockHash + " // previous hash: " + block.prevBlockHash + " // block offset: " + block.blockOffset);
            }

            if (!ValidateHash(block)) // if validating hash FAILS
            {
                validBlock = false;
                System.Diagnostics.Debug.WriteLine("ERROR: invalid hash, current hash:" + block.currBlockHash + " // previous hash: " + block.prevBlockHash + " // block offset: " + block.blockOffset);
            }

            if (validBlock)
            {
                blocks.Add(block);
            }

            return validBlock;

        }
        /// <summary>
        /// returns highest block id
        /// </summary>
        /// <returns></returns>
        public uint GetHighestBlockID()
        {
            return (uint) blocks.Count - 1;
        }

        /// <summary>
        /// validates block hash
        /// </summary>
        /// <returns></returns>
        private bool ValidateHash(Block block)
        {
            bool validated = false;
            int offset = 1;
            string hash;

            if (block.currBlockHash.StartsWith("12"))
            {
                SHA256 sha256 = SHA256.Create();

                do
                {
                    offset++;

                    string transactionData = block.id.ToString() + block.fromWalletID.ToString() + block.toWalletID.ToString() + block.amount.ToString() + block.blockOffset.ToString();

                    byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(transactionData));
                    hash = BitConverter.ToUInt64(hashBytes, 0).ToString();
                }
                while ((!hash.StartsWith("12345")) || (offset % 5 != 0));

                if (block.currBlockHash == hash)
                {
                    validated = true;
                }
                
            }

            return validated;
        }

        /// <summary>
        /// gets list of accounts by id and list of their corresponding balances
        /// </summary>
        /// <param name="accountIDs"></param>
        /// <param name="accountBalances"></param>
        private void GetAccounts(out List<uint> accountIDs, out List<float> accountBalances)
        {
            accountIDs = new List<uint>();
            accountBalances = new List<float>();
            foreach (Block block in blocks)
            {

                // FROM WALLET IDs
                if (!accountIDs.Contains(block.fromWalletID)) // if account ID DOESN'T already exist in system
                {
                    accountIDs.Add(block.fromWalletID);
                    accountBalances.Add(block.amount * -1); // will subtract amount from account's balance
                }
                else // if account ID already exists in system
                {
                    int accountIndex = accountIDs.IndexOf(block.fromWalletID);
                    accountBalances[accountIndex] = accountBalances[accountIndex] - block.amount; // will subtract amount from account's balance
                }

                // TO WALLET IDs
                if (!accountIDs.Contains(block.toWalletID))  // if account ID DOESN'T already exist in system
                {
                    accountIDs.Add(block.toWalletID);
                    accountBalances.Add(block.amount); // will add amount to account's balance
                }
                else // if account ID already exists in system
                {
                    int accountIndex = accountIDs.IndexOf(block.toWalletID);
                    accountBalances[accountIndex] = accountBalances[accountIndex] + block.amount; // will add amount to account's balance
                }
            }
        }

        /// <summary>
        /// gets account balance for given account id
        /// </summary>
        /// <param name="inAccountID"></param>
        /// <returns></returns>
        public float GetAccountBalance(uint inAccountID)
        {
            List<uint> accountIDs = new List<uint>();
            List<float> accountBalances = new List<float>();
            GetAccounts(out accountIDs, out accountBalances);

            int accountIndex = -1;
            foreach (uint accountID in accountIDs)
            {
                accountIndex++;

                if (accountID == inAccountID)
                {
                    return accountBalances[accountIndex];
                }

            }

            return 0; // if no account found with given id
        }

        /// <summary>
        /// gets current state of blockchain
        /// </summary>
        /// <returns></returns>
        public ChainState GetState()
        {
            ChainState chainState = new ChainState();

            chainState.numBlocks = (uint) blocks.Count();

            List<uint> accountIDs = new List<uint>();
            List<float> accountBalances = new List<float>();
            GetAccounts(out accountIDs, out accountBalances);
            chainState.accountIDs = accountIDs;
            chainState.accountBalances = accountBalances;

            chainState.totalAmount = 0;
            int index = -1;
            foreach (float balance in chainState.accountBalances)
            {
                index++;
                if (index != 0)
                {
                    chainState.totalAmount += balance;
                }   
            }

            if (chainState == null || accountIDs == null)
            {
                chainState.totalAmount = 0;
                chainState.numBlocks = 1;
                chainState.accountIDs = new List<uint>();
                chainState.accountIDs = new List<uint>();
            }

            return chainState;

        }

        /// <summary>
        /// hashes block and returns completed block
        /// </summary>
        /// <param name="newBlock"></param>
        /// <returns></returns>
        public Block HashBlock(Block newBlock)
        {
            SHA256 sha256 = SHA256.Create();
            string transactionData;
            byte[] hashBytes;
            do
            {
                newBlock.blockOffset++;

                transactionData = newBlock.id.ToString() + newBlock.fromWalletID.ToString() + newBlock.toWalletID.ToString() + newBlock.amount.ToString() + newBlock.blockOffset.ToString();

                hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(transactionData));
                newBlock.currBlockHash = BitConverter.ToUInt64(hashBytes, 0).ToString();

            }
            while ((!newBlock.currBlockHash.StartsWith("12345")) || (newBlock.blockOffset % 5 != 0));

            return newBlock;
        }


    }
}