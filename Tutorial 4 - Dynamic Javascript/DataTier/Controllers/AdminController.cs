using DataTier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DataTier.Controllers
{
    // File name: AdminController.cs
    // Author: Kade McGarraghy
    // Purpose: API Controller for performing admin actions
    // Date last modified: 
    public class AdminController : ApiController
    {
        /// <summary>
        /// admin only action of saving all of bank's data
        /// </summary>
        [Route("api/Admin/Save")]
        [HttpGet]
        public void Save()
        {
            BankModel.bankData.SaveToDisk();
        }

        /// <summary>
        /// admin only action of processing all pending transactions (deposits/withdraws money from user account)
        /// </summary>
        [Route("api/Admin/ProcessTransactions")]
        [HttpGet]
        public void ProcessAllTransactions()
        {
            BankModel.bankData.ProcessAllTransactions();
        }

    }
}
