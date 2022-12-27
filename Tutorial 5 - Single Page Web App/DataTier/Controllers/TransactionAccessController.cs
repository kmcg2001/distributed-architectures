using BankClasses;
using DataTier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DataTier.Controllers
{
    // File name: TransactionAccessController.cs
    // Author: Kade McGarraghy
    // Purpose: API Controller for accessing bank transactions
    // Date last modified: 24/05/21

    public class TransactionAccessController : ApiController
    {

        private BankDB.TransactionAccessInterface transactions = BankModel.bankData.GetTransactionInterface();

        /// <summary>
        /// gets transaction from id
        /// </summary>
        /// <param name="transactionID"></param>
        /// <returns></returns>
        [Route("api/Transaction/{transactionID}")]
        [HttpGet]
        public TransactionDataStruct GetTransactionDetails(uint transactionID) 
        {
            TransactionDataStruct transactionData = new TransactionDataStruct();
           
            try
            {
                transactions.SelectTransaction(transactionID);

                transactionData.id = transactionID;

         
                transactionData.receivingAccountID = transactions.GetRecvrAcct();
                transactionData.sendingAccountID = transactions.GetSendrAcct();
                transactionData.amount = transactions.GetAmount();
            }
            catch (Exception e)
            {
                transactionData.receivingAccountID = 0;
                transactionData.sendingAccountID = 0;
                transactionData.amount = 0;
            }
        

            return transactionData;
        }

        /// <summary>
        /// creates a transaction between accounts
        /// </summary>
        /// <param name="senderID"></param>
        /// <param name="receiverID"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        [Route("api/CreateTransaction/")]
        [Route("api/CreateTransaction/{senderID}/{receiverID}/{amount}")]
        [HttpGet]
        public TransactionDataStruct CreateTransaction(uint senderID, uint receiverID, uint amount) 
        {
            TransactionDataStruct transactionData = new TransactionDataStruct();
            BankDB.AccountAccessInterface accounts = BankModel.bankData.GetAccountInterface();

            try
            {
                accounts.SelectAccount(senderID);
                uint senderBalance = accounts.GetBalance();

                if ((senderID > 0) && (receiverID > 0)) // if both users are valid
                {

                    if (senderBalance >= amount)
                    {

                        uint transactionID = transactions.CreateTransaction();
                        transactions.SelectTransaction(transactionID);

                        transactions.SetSendr(senderID);
                        transactions.SetRecvr(receiverID);
                        transactions.SetAmount(amount);

                        transactionData.id = transactionID;
                        transactionData.receivingAccountID = transactions.GetRecvrAcct();
                        transactionData.sendingAccountID = transactions.GetSendrAcct();
                        transactionData.amount = transactions.GetAmount();
                    }
                }

                else
                {
                    transactionData.id = 0;
                    transactionData.receivingAccountID = 0;
                    transactionData.sendingAccountID = 0;
                    transactionData.amount = 0;
                }
            }
        
            catch (Exception e)
            {
                transactionData.id = 0;
                transactionData.receivingAccountID = 0;
                transactionData.sendingAccountID = 0;
                transactionData.amount = 0;
            }

            return transactionData;
        }

        /// <summary>
        /// retrieves all currently pending bank transactions
        /// </summary>
        /// <returns></returns>
        [Route("api/Transactions")]
        [HttpGet]
        public List<uint> GetTransactions() 
        {
            return transactions.GetTransactions();
        }

        /// <summary>
        /// gets all of an accounts transactions
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>

        public List<TransactionDataStruct> GetAccountTransactions(uint accountID)
        {
            List<uint> allTransactions = GetTransactions();
            List<TransactionDataStruct> accountTransactions = new List<TransactionDataStruct>();

            try
            {
                foreach (uint transactionID in allTransactions)
                {
                    TransactionDataStruct trans = GetTransactionDetails(transactionID);
                    if ((trans.receivingAccountID == accountID) || (trans.sendingAccountID == accountID))
                    {
                        accountTransactions.Add(trans);
                    }
                }
            }
            catch (Exception e)
            {

            }


            return accountTransactions;

        }
    }
}
