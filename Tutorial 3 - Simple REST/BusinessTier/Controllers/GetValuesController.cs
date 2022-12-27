using APIClasses;
using DBWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace DBWebService.Controllers
{
    public class GetValuesController : ApiController
    {

        /// <summary>
        /// file name: GetValuesController.cs
        /// author: Kade McGarraghy (reference: Tutorial 1 Worksheet)
        /// purpose: API Controller for getting an account's values
        /// date last modified: 23/05/21
        /// </summary>

        // GET api/<controller>/5
        public DataIntermed Get(int id)
        {
            DataIntermed dataIntermed = new DataIntermed();
            DataModel dataModel = new DataModel();

            dataModel.GetValuesForEntry(id, out var accNo, out var pin, out var bal, out var fName, out var lName);
            dataIntermed.acct = accNo;
            dataIntermed.pin = pin;
            dataIntermed.bal = bal;
            dataIntermed.fname = fName;
            dataIntermed.lname = lName;

            return dataIntermed;

        }

    }
}