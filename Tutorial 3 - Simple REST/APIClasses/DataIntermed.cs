using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace APIClasses
{

    /// <summary>
    /// file name: DataIntermed.cs
    /// author: Kade McGarraghy (reference: Tutorial 1 Worksheet)
    /// purpose: represents account object to be sent across network for api methods
    /// date last modified: 23/05/21
    /// </summary>

    public class DataIntermed
    {
        public int bal;
        public uint acct;
        public uint pin;
        public string fname;
        public string lname;
    }
}
