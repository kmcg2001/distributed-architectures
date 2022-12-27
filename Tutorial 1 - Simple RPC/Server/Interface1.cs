﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DataStuff;

namespace Server
{
    //Make this a service contract as it is a service interface
    [ServiceContract]
    public interface DataServerInterface
    {
         //Each of these are service function contracts. They need to be tagged as OperationContracts.
        
         [OperationContract]
         int GetNumEntries();

         [OperationContract]
         void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out
         string fName, out string lName);
    }

}
