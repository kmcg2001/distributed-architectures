using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace DBWebService.Models
{
    class DataModel
    {
        private DataServerInterface.DataServerInterface foob;

        public DataModel()
        {
            // This is a factory that generates remote connections to our remote class. This 
            // is what hides the RPC stuff!
            var tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            var URL = "net.tcp://localhost:8100/DataService";
            var chanFactory = new ChannelFactory<DataServerInterface.DataServerInterface>(tcp, URL);

            foob = chanFactory.CreateChannel();
        }

        public int GetNumEntries()
        {
            return foob.GetNumEntries();
            
        }

        public void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName)
        {
            foob.GetValuesForEntry(index, out acctNo, out pin, out bal, out fName, out lName);
        }

        public int SearchByLastname(string searchTerm)
        {
            int index = -1;

            for (int i = 0; i < GetNumEntries(); i++)
            {
                foob.GetValuesForEntry(i, out var accNo, out var pin, out var bal, out var fName, out var lName);
                if (searchTerm.ToUpper().Equals(lName.ToUpper()))
                {
                    index = i;
                    i = GetNumEntries();

                }
            }

            return index;
        }


    }
}