using DBWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DBWebService.Controllers
{
    /// <summary>
    /// file name: ValuesController.cs
    /// author: Kade McGarraghy (reference: Tutorial 1 Worksheet)
    /// purpose: API Controller for getting number of entries for accounts
    /// date last modified: 23/05/21
    /// </summary>
    public class ValuesController : ApiController
    {
        // GET api/<controller>/5
        public int Get()
        {
            DataModel dataModel = new DataModel();
            return dataModel.GetNumEntries();
        }

    }

}