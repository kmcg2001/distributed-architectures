using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIClasses
{
    /// <summary>
    /// file name: BlockchainController.cs
    /// author: Kade McGarraghy
    /// purpose:  represents state of blockchain
    /// date: 24/5/21

    public class ChainState
    {
        public uint numBlocks;
        public float totalAmount;
        public List<uint> accountIDs;
        public List<float> accountBalances;
    }
}
