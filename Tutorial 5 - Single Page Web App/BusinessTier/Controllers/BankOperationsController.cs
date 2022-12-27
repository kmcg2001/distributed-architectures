using BankClasses;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

/// <summary>
/// file name: BankOperationsController.cs
/// author: Kade McGarraghy
/// purpose: api methods that user can interact with to access data tier, so they can perform bank operations
/// date last modified: 24/05/21
/// </summary>
/// 
namespace BusinessTier.Controllers
{
    public class BankOperationsController : ApiController
    {
        private string url; // setting url for ASP.NET web api connection
        private RestClient client;
        private RestRequest request;
        private IRestResponse response;

        private static int transactionCount = 0;

        public BankOperationsController()
        {
            url = "http://localhost:50860";
            client = new RestClient(url);  // making connection
        }

        /// <summary>
        /// creates user with first name and last name
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        [Route("api/BankOperations/CreateUser")]
        [Route("api/BankOperations/CreateUser/{firstName}/{lastName}")]
        [HttpGet]
        public UserDataStruct CreateUser(string firstName, string lastName) // Need to do exception handling
        {
            request = new RestRequest("api/CreateUser/" + firstName + "/" + lastName); // set up api method request
            response = client.Get(request); // call api method
            UserDataStruct newUser = JsonConvert.DeserializeObject<UserDataStruct>(response.Content);
            Save();

            return newUser;

        }

        /// <summary>
        /// Creates account for given user id
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [Route("api/BankOperations/CreateAccount/{userID}")]
        [Route("api/BankOperations/CreateAccount/")]
        [HttpGet]
        public AccountDataStruct CreateAccount(uint userID) // Need to do exception handling
        {
            request = new RestRequest("/api/CreateAccount/" + userID); // set up api method request
            response = client.Get(request); // call api method
            AccountDataStruct newAccount = JsonConvert.DeserializeObject<AccountDataStruct>(response.Content);    // if error, then console.writeline ERROR
            Save();
            return newAccount;
        }

        /// <summary>
        /// Gets account details for given account ID
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        [Route("api/BankOperations/Account/{accountID}")]
        [Route("api/BankOperations/Account")]
        [HttpGet]
        public AccountDataStruct GetAccountDetails(uint accountID) 
        {
            request = new RestRequest("api/Account/" + accountID); // set up api method request
            response = client.Get(request); // call api method
            AccountDataStruct account = JsonConvert.DeserializeObject<AccountDataStruct>(response.Content);
            return account;
        }

        /// <summary>
        /// Deposits money into user account
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        [Route("api/BankOperations/Deposit/")]
        [Route("api/BankOperations/Deposit/{accountID}/{amount}")]
        [HttpGet]
        public uint Deposit(uint accountID, uint amount) 
        {
            uint finalDepositAmount = 0;

            if (UInt32.TryParse(amount.ToString(), out uint result)) // makes sure number entered is integer
            {
                uint validAmount = UInt32.Parse(amount.ToString());

                if (validAmount > 0)
                {
                    AccountDataStruct account = GetAccountDetails(accountID);
                    if (account.id > 0)
                    {
                        request = new RestRequest("api/Deposit/" + accountID + "/" + amount); // set up api method request
                        response = client.Get(request); // call api method
                        finalDepositAmount = JsonConvert.DeserializeObject<uint>(response.Content);

                        Save();
                    }
                    else
                    {
                        finalDepositAmount = 0;
                        System.Diagnostics.Debug.WriteLine("Invalid deposit due to invalid account ID given  (account ID (" + account.id + ")");
                    }
                }
                else
                {
                    finalDepositAmount = 0;
                    System.Diagnostics.Debug.WriteLine("Invalid deposit amount, less than 0 (" + validAmount + ")");
                }

            }

            return finalDepositAmount;
        }

        /// <summary>
        /// withdraws money from user account
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        [Route("api/BankOperations/Withdraw/")]
        [Route("api/BankOperations/Withdraw/{accountID}/{amount}")]
        [HttpGet]
        public uint Withdraw(uint accountID, uint amount) // Need to do exception handling
        {
            uint finalWithdrawalAmount = 0;
            //((UInt32.TryParse(accountID.ToString(), out uint result1) && 
            if (UInt32.TryParse(amount.ToString(), out uint result)) // makes sure number entered is integer
            {
                uint validAmount = UInt32.Parse(amount.ToString());

                if (validAmount > 0)
                {
                    AccountDataStruct account = GetAccountDetails(accountID);
                    if (account.id > 0)
                    {
                        if (validAmount <= account.accountBalance)
                        {
                            request = new RestRequest("api/Withdraw/" + accountID + "/" + amount); // set up api method request
                            response = client.Get(request); // call api method
                            finalWithdrawalAmount = JsonConvert.DeserializeObject<uint>(response.Content);

                            Save();
                        }
                        else
                        {
                            finalWithdrawalAmount = 0;
                        }
                    }
                    else
                    {
                        finalWithdrawalAmount = 0;
                        // log "less than 0"
                    }
                }
            }

            return finalWithdrawalAmount;
        }

        /// <summary>
        /// gets user details from user ID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [Route("api/BankOperations/User/")]
        [Route("api/BankOperations/User/{userID}")]
        [HttpGet]
        public UserDataStruct GetUserDetails(uint userID) // Need to do exception handling
        {
            request = new RestRequest("api/User/" + userID); // set up api method request
            response = client.Get(request); // call api method
            UserDataStruct user = JsonConvert.DeserializeObject<UserDataStruct>(response.Content);
            return user;
        }

        /// <summary>
        /// gets transaction details from transaction ID
        /// </summary>
        /// <param name="transactionID"></param>
        /// <returns></returns>
        [Route("api/BankOperations/Transaction/")]
        [Route("api/BankOperations/Transaction/{transactionID}")]
        [HttpGet]
        public TransactionDataStruct GetTransactionDetails(uint transactionID) // Need to do exception handling
        {
            TransactionDataStruct transactionData = new TransactionDataStruct();
            if (UInt32.TryParse(transactionID.ToString(), out uint result)) // makes sure number entered is integer
            {
                uint validID = UInt32.Parse(transactionID.ToString());
                request = new RestRequest("api/Transaction/" + validID); // set up api method request
                response = client.Get(request); // call api method
                transactionData = JsonConvert.DeserializeObject<TransactionDataStruct>(response.Content);

                ProcessAllTransactions();

            }
            return transactionData;
        }

        /// <summary>
        /// creates transaction between sender, receiver accounts 
        /// </summary>
        /// <param name="senderID"></param>
        /// <param name="receiverID"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        [Route("api/BankOperations/CreateTransaction/")]
        [Route("api/BankOperations/CreateTransaction/{senderID}/{receiverID}/{amount}")]
        [HttpGet]
        public TransactionDataStruct CreateTransaction(uint senderID, uint receiverID, uint amount) // Need to do exception handling
        {
            TransactionDataStruct transactionData = new TransactionDataStruct();
            if ((UInt32.TryParse(senderID.ToString(), out uint result1) && (UInt32.TryParse(receiverID.ToString(), out uint result2) && (UInt32.TryParse(amount.ToString(), out uint result3))))) // makes sure number entered is integer
            {
                if (amount > 0)
                {
                    request = new RestRequest("api/CreateTransaction/" + senderID + "/" + receiverID + "/" + amount); // set up api method request
                    response = client.Get(request); // call api method
                    transactionData = JsonConvert.DeserializeObject<TransactionDataStruct>(response.Content);

                    if (transactionData.id > 0)
                    {
                        //success
                        Save();
                        //ProcessAllTransactions();

                        transactionCount++;
                        if (transactionCount == 3)
                        {
                            ProcessAllTransactions();
                            transactionCount = 0;
                        }
                    }
                }
            }
            return transactionData;
        }

        /// <summary>
        /// gets account transactions for given account ID
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        [Route("api/BankOperations/GetAccountTransactions/")]
        [Route("api/BankOperations/GetAccountTransactions/{accountID}/")]
        [HttpGet]
        public List<TransactionDataStruct> GetAccountTransactions(uint accountID)
        {
            List<TransactionDataStruct> transactionsData = new List<TransactionDataStruct>();
            if ((UInt32.TryParse(accountID.ToString(), out uint result1))) // makes sure number entered is integer
            {
                request = new RestRequest("api/GetAccountTransactions/" + accountID); // set up api method request
                response = client.Get(request); // call api method
                transactionsData = JsonConvert.DeserializeObject<List<TransactionDataStruct>>(response.Content);

                if (transactionsData.Count > 0)
                {
                    //success
                }
            }
            return transactionsData;
        }


        /// <summary>
        /// calls corresponding method in data tier to update accounts involved in recent transactions
        /// </summary>
        private void ProcessAllTransactions()
        {
            request = new RestRequest("api/Admin/ProcessTransactions/"); // set up api method request
            client.Get(request); // call api method
        }

        /// <summary>
        /// calls corresponding method in data tier to save all bank data
        /// </summary>
        private void Save()
        {
            request = new RestRequest("api/Admin/Save/"); // set up api method request
            client.Get(request); // call api method
        }





    }
}
