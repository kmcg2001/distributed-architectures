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
    /// <summary>
    /// file name: SearchController.cs
    /// author: Kade McGarraghy (reference: Tutorial 1 Worksheet)
    /// purpose: API Controller for searching accounts list by lastname
    /// date last modified: 23/05/21
    /// </summary>

    public class SearchController : ApiController
    {
        private DataModel dataModel;
        private DataIntermed dataIntermed;

        // POST api/<controller>
        public DataIntermed Post([FromBody] SearchData value)
        {
            DataIntermed dataIntermed = new DataIntermed();
            dataModel = new DataModel();

            int index = dataModel.SearchByLastname(value.searchStr);

            if (index != -1)
            {
                dataModel.GetValuesForEntry(index, out var accNo, out var pin, out var bal, out var fName, out var lName);
                dataIntermed.acct = accNo;
                dataIntermed.pin = pin;
                dataIntermed.bal = bal;
                dataIntermed.fname = fName;
                dataIntermed.lname = lName;
            }
            else
            {
                dataIntermed.acct = 0;
                dataIntermed.pin = 0;
                dataIntermed.bal = 0;
                dataIntermed.fname = "";
                dataIntermed.lname = "";
            }
 

            return dataIntermed;
        }
    }
}