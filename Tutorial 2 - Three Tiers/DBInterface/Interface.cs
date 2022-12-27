using System.ServiceModel;

namespace Server
{
    /// <summary>
    /// file name: DataServerInterface.cs
    /// author: Kade McGarraghy (reference: Tutorial 1 Worksheet)
    /// purpose: interface for data server -> user services to get database info
    /// date last modified: 23/05/21
    /// </summary>


    [ServiceContract]
    public interface DataServerInterface
    {
        
         [OperationContract] // means service function contract
         int GetNumEntries();

         [OperationContract]
         void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out
         string fName, out string lName);


    }

}
