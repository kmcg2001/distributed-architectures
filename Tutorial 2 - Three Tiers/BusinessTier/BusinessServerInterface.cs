using DataServerInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BusinessTier
{
    /// <summary>
    /// file name: BusinessServerInterface.cs
    /// author: Kade McGarraghy 
    /// purpose: interface for business server -> user services to get database info from business server which uses data server
    /// date last modified: 23/05/21
    /// </summary>

    [ServiceContract]
    public interface BusinessServerInterface
    {
        [OperationContract]
        int GetNumEntries();

        [OperationContract]
        [FaultContract(typeof(IndexOutOfRangeFault))]
        void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName);

        [OperationContract]
        int SearchByLastname(string searchTerm);
    }
}
