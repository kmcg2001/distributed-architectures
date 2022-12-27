using APIClasses;
using MinerServer.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web.Http;

namespace MinerServer.Controllers
{
    /// <summary>
    /// file name: MinerController.cs
    /// author: Kade McGarraghy
    /// purpose: api method to make new transaction/mine new block
    /// date: 24/5/21
    /// </summary>
    public class MinerController : ApiController
    {
        
        private static Queue<Transaction> transactions = new Queue<Transaction>();
        private bool runProcessThread = true;
        private delegate void ThreadDelegate();
        [Route("PostTransaction/")]
        [Route("PostTransaction/{fromWalletID}/{toWalletID}/{amount}")]
        [HttpGet]

        // posts transaction by adding block to chain if transaction is valid
        public bool PostTransaction(uint fromWalletID, uint toWalletID, uint amount)
        {
            Transaction transaction = new Transaction();
            transaction.toWalletID = toWalletID;
            transaction.fromWalletID = fromWalletID;
            transaction.amount = amount;
            transactions.Enqueue(transaction);
            System.Diagnostics.Debug.WriteLine("to wallet : " + toWalletID + " from wallet: " + fromWalletID + " amount: " + amount);

            if (runProcessThread)
            {
                ThreadDelegate threadDelegate = DoTransaction;
                threadDelegate.BeginInvoke(null, null);
            }

            return true;
        }

        /// <summary>
        /// helper method -> innner workings of processing transaction
        /// </summary>
        private void DoTransaction()
        {
            string url = "http://localhost:31551/"; // setting url for ASP.NET web api connection
            RestClient client = new RestClient(url); // making connection

            while (transactions.Count > 0)
            {
                bool transactionDone = false;
                Transaction transaction = transactions.Dequeue();

                while (!transactionDone)
                {
   
                    RestRequest request = new RestRequest("Blockchain/Balance/" + transaction.fromWalletID);  // set up api method request // add JSON object in body to be sent when api method is called, user token makes sure they are validated to use service
                    IRestResponse response = client.Get(request); // call api method
                    float balance = 0;
                    try 
                    {
                        balance = JsonConvert.DeserializeObject<float>(response.Content);
                    }
                    catch (JsonSerializationException e)
                    {
                        System.Diagnostics.Debug.WriteLine("ERROR: Timed out, generating hash took too long."); 
                    }
                    
                    if ((balance >= transaction.amount) || transaction.fromWalletID == 0)
                    {
                        Block newBlock = new Block();

                        request = new RestRequest("Blockchain/Get");  // set up api method request // add JSON object in body to be sent when api method is called, user token makes sure they are validated to use service
                        response = client.Get(request); // call api method

                        List<Block> currBlockchain = JsonConvert.DeserializeObject<List<Block>>(response.Content);

                        newBlock.id = (uint)currBlockchain.Count;
                        newBlock.fromWalletID = transaction.fromWalletID;
                        newBlock.toWalletID = transaction.toWalletID;
                        newBlock.amount = transaction.amount;
                        newBlock.prevBlockHash = currBlockchain[currBlockchain.Count - 1].currBlockHash;


                        SHA256 sha256 = SHA256.Create();
                        uint offset = 1;
                        string hash;

                        do
                        {
                            offset++;

                            string transactionData = newBlock.id.ToString() + newBlock.fromWalletID.ToString() + newBlock.toWalletID.ToString() + newBlock.amount.ToString() + offset.ToString();
      
                            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(transactionData));
                            hash = BitConverter.ToUInt64(hashBytes, 0).ToString();
                        }
                        while ((!hash.StartsWith("12345")) || (offset % 5 != 0));

                        newBlock.currBlockHash = hash;
                        newBlock.blockOffset = offset;

                        request = new RestRequest("Blockchain/NewBlock/");  // set up api method request 
                        request.AddJsonBody(newBlock); // // add JSON object (serialized block object) in body to be sent when api method is called
                        response = client.Post(request); // call api method
                        transactionDone = JsonConvert.DeserializeObject<bool>(response.Content);

                    }
                    else
                    {
                        transactionDone = true;
                    }

                }

                if (transactions.Count == 0)
                {
                    runProcessThread = false;
                }
            }

        }
    }
}
