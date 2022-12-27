using BankClasses;
using DataTier.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace DataTier.Controllers
{
    // File name: AccountAccessController.cs
    // Author: Kade McGarraghy
    // Purpose: API Controller for accessing bank accounts
    // Date last modified: 25/05/21

    public class AccountAccessController : ApiController
    {
        private AccountDataStruct accountData = new AccountDataStruct();
        private BankDB.AccountAccessInterface accounts = BankModel.bankData.GetAccountInterface();

        /// <summary>
        /// retrieves account by its id
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        [Route("api/Account/{accountID}")]
        [Route("api/Account/")]
        [HttpGet]
        public AccountDataStruct GetAccountDetails(uint accountID) // Need to do exception handling
        {
            accounts.SelectAccount(accountID);

            accountData.id = accountID;

            try
            {
                accountData.userID = accounts.GetOwner();
                accountData.accountBalance = accounts.GetBalance();
                System.Diagnostics.Debug.WriteLine("account balance: " + accountData.accountBalance);
            }
            catch (Exception e) // sorry to catch generic exception, won't let me catch the specifc BankDB.NoAccount exception
            {
                accountData.id = 0;
                accountData.accountBalance = 0;
                accountData.userID = 0;
            }
            
            return accountData;
        }

        /// <summary>
        /// creates an account, requires a valid user id
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [Route("api/CreateAccount/{userID}")]
        [Route("api/CreateAccount/")]
        [HttpGet]
        public AccountDataStruct CreateAccount(uint userID) // Need to do exception handling
        {
            uint accountID = accounts.CreateAccount(userID);

            accountData.id = accountID;
            accountData.userID = accounts.GetOwner();
            accountData.accountBalance = accounts.GetBalance();

            return accountData;
        }

        /// <summary>
        /// deposits money into user's account
        /// </summary>
        /// <param name="userID"></param>
        [Route("api/Deposit/")]
        [Route("api/Deposit/{accountID}/{amount}")]
        [HttpGet]
        public uint Deposit(uint accountID, uint amount) // Need to do exception handling
        {
            accounts.SelectAccount(accountID);
            
            try
            {
                accounts.Deposit(amount);

                accountData.id = accountID;
                accountData.userID = accounts.GetOwner();
                accountData.accountBalance = accounts.GetBalance();
            }
            catch (Exception e)
            {
                accountData.id = 0;
                accountData.userID = 0; ;
                accountData.accountBalance = 0;
            }

            return amount;

        }

        /// <summary>
        /// withdraws money from user's account
        /// </summary>
        /// <param name="userID"></param>
        [Route("api/Withdraw/")]
        [Route("api/Withdraw/{accountID}/{amount}")]
        [HttpGet]
        public uint Withdraw(uint accountID, uint amount) // Need to do exception handling
        {
            accounts.SelectAccount(accountID);

            try
            {
                accounts.Withdraw(amount);

                accountData.id = accountID;
                accountData.userID = accounts.GetOwner();
                accountData.accountBalance = accounts.GetBalance();
            }
            catch (Exception e)
            {
                accountData.id = 0;
                accountData.userID = 0; ;
                accountData.accountBalance = 0;
            }  
            return amount;

        }

        /// <summary>
        /// gets all of a user's accounts
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [Route("api/Accounts/")]
        [Route("api/Accounts/{userID}")]
        [HttpGet]
        public List<uint> GetAccounts(uint userID) // Need to do exception handling
        {
            List<uint> retrievedAccounts = new List<uint>();
            try
            {
                retrievedAccounts = accounts.GetAccountIDsByUser(userID);
            }
            catch (Exception e)
            {

            }
            return retrievedAccounts;
        }
    }
}
