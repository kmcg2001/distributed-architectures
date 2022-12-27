using APIClasses;
using BlockchainServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BlockchainServer.Controllers
{
    /// <summary>
    /// file name: BlockchainController.cs
    /// author: Kade McGarraghy
    /// purpose: api methods to retrieve blockchain info
    /// date: 24/5/21
    /// </summary>
    public class BlockchainController : ApiController
    {
        private static Blockchain blockchain = new Blockchain();

        /// <summary>
        /// gets current blockchain
        /// </summary>
        /// <returns></returns>
        [Route("Blockchain/Get")]
        [HttpGet]
        public List<Block> GetBlockChain()
        {
            return Blockchain.blocks;
        }

        /// <summary>
        /// gets current state of blockchain
        /// </summary>
        /// <returns></returns>
        [Route("Blockchain/State")]
        [HttpGet]
        public ChainState BlockChainState()
        {
            return blockchain.GetState();
        }

        /// <summary>
        /// gets balance for given account
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("Blockchain/Balance/")]
        [Route("Blockchain/Balance/{id}")]
        [HttpGet]
        public float UserBalance(uint id)
        {
            return blockchain.GetAccountBalance(id);
        }

        /// <summary>
        /// creates new block in blockchain
        /// </summary>
        /// <param name="newBlock"></param>
        /// <returns></returns>
        [Route("Blockchain/NewBlock")]
        [HttpPost]
        public bool NewBlock([FromBody] Block newBlock)
        {

            return blockchain.AddBlock(newBlock);
           
        }
    }
}
