using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStuff
{

    /// <summary>
    /// file name: EntryGenerator.cs
    /// author: Kade McGarraghy (reference: Tutorial 1 Worksheet)
    /// purpose: randomly generates a user's details
    /// date last modified: 23/05/21
    /// </summary>

    class EntryGenerator
    {
        private Random rand = new Random();

        private string GetFirstname()
        {
            int pos;
            List<String> firstnames = new List<String>();

            firstnames.Add("David");
            firstnames.Add("Kade");
            firstnames.Add("Barry");
            firstnames.Add("Joe");
            firstnames.Add("Donald");
            firstnames.Add("Kamala");
            firstnames.Add("Hillary");
            firstnames.Add("Meghan");
            firstnames.Add("Elizabeth");
            firstnames.Add("Angela");

            pos = rand.Next(0, firstnames.Count); // one of the listed last names will be returned, this is line makes this easily scalable :)

            return firstnames[pos];
        }

        private string GetLastname()
        {
            int pos;

            List<String> lastnames = new List<String>();

            lastnames.Add("Wyden");
            lastnames.Add("Rubio");
            lastnames.Add("Sanders");
            lastnames.Add("Johnston");

            pos = rand.Next(0, lastnames.Count); 

            return lastnames[pos];
        }

        private uint GetPIN()
        {
            uint pin;

            int dig1, dig2, dig3, dig4;
            dig1 = rand.Next(0, 9);
            dig2 = rand.Next(0, 9);
            dig3 = rand.Next(0, 9);
            dig4 = rand.Next(0, 9);

            string pinStr = "" + dig1 + dig2 + dig3 + dig4;

            pin = UInt32.Parse(pinStr);

            return pin;

        }

        private uint GetAcctNo()
        {
            uint acctNo;

            acctNo = (uint)rand.Next(0, 999999999);

            return acctNo;
        }

        private int GetBalance()
        {
            int balance;

            balance = rand.Next(0, 10000000);

            return balance;
        }

        public void GetNextAccount(out uint pin, out uint acctNo, out string firstName, out string
        lastName, out int balance)
        {
            pin = GetPIN();
            acctNo = GetAcctNo();
            firstName = GetFirstname();
            lastName = GetLastname();
            balance = GetBalance();
        }
    }
}
