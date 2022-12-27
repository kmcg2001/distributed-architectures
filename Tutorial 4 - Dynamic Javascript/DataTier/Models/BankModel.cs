using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataTier.Models
{

    /// <summary>
    /// file name: BankModel.cs
    /// author: Kade McGarraghy
    /// purpose: initalises and represents bank data base
    /// date last modified: 24/05/21
    /// </summary>
    /// 
    class BankModel
    {
        public static BankDB.BankDB bankData = new BankDB.BankDB();
    }
}