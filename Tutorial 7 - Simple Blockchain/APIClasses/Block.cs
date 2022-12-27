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
    /// purpose: represents block in blockchain
    /// date: 24/5/21

    public class Block
    {
        public uint id; // uniquely identifies the block 
        public uint toWalletID; // identifying the account the transaction is from
        public uint fromWalletID; // identifying the account the transaction is to
        public float amount; // representing the amount of coins being sent. It cannot be negative
        public uint blockOffset; // used to produce a valid hash
        // A block offset that is a multiple of 5
        public string prevBlockHash; // hash of the block immediately prior to this one
        // A hash that starts with 12345

        public string currBlockHash; // hash of the current block

    }
}
