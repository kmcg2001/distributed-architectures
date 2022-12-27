using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIClasses
{
    /// <summary>
    /// file name: ChainState.cs 
    /// author: Kade McGarraghy
    /// purpose: represents state of blockchain
    /// date last modified: 25/5/21
    /// </summary>
    public class ChainState
    {
        public uint numBlocks;
        public float totalAmount;
        public List<uint> accountIDs;
        public List<float> accountBalances;
    }
}
