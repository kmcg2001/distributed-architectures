using APIClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    /// <summary>
    /// file name: BlockchainServiceInterface.cs 
    /// author: Kade McGarraghy
    /// purpose: interface for clients to access blockchain info
    /// date last modified: 25/5/21
    [ServiceContract]
    public interface BlockchainServiceInterface
    {

        [OperationContract]
        List<Block> GetCurrentBlockchain();

        [OperationContract]
        float GetAccountBalance(uint accountID);

        [OperationContract]
        Block GetCurrentBlock();

        [OperationContract]
        void ReceiveNewTransaction(Transaction newTransaction);
    }
}
