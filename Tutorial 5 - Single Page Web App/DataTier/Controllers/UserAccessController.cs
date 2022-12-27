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
    // File name: UserAccessController.cs
    // Author: Kade McGarraghy
    // Purpose: API Controller for accessing users
    // Date last modified: 24/05/21

    public class UserAccessController : ApiController
    {
        private UserDataStruct userData = new UserDataStruct();
        private BankDB.UserAccessInterface users = BankModel.bankData.GetUserAccess();

        /// <summary>
        /// gets user details from id
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [Route("api/User/{userID}")]
        [HttpGet]
        public UserDataStruct GetUserDetails(uint userID) // Need to do exception handling
        {
            try
            {
                users.SelectUser(userID);

                userData.id = userID;
                string firstName, lastName;
                users.GetUserName(out firstName, out lastName);
                userData.firstName = firstName;
                userData.lastName = lastName;
            }
            catch (Exception e)
            {
                userData.id = 0;
                userData.firstName = "";
                userData.lastName = "";
            }


            return userData;
        }

        /// <summary>
        /// creates user with first name/last name
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        [Route("api/CreateUser/{firstName}/{lastName}")]
        [HttpGet]
        public UserDataStruct CreateUser(string firstName, string lastName) // Need to do exception handling
        {
            userData.id = users.CreateUser();

            string fName = firstName;
            string lName = lastName;
            users.SetUserName(fName, lName);
            users.SelectUser(userData.id);

            users.GetUserName(out fName, out lName);
            userData.firstName = firstName;
            userData.lastName = lastName;

            return userData;
        }
    }
}
