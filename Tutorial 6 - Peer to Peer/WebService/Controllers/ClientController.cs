using ClientClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebService.Models;

namespace WebService.Controllers
{
    /// <summary>
    ///  file name: ClientController.cs
    ///  author: Kade McGarraghy
    ///  purpose: api controller for methods to support multiple clients and concurrent job access
    ///  date: 24/05/21
    /// </summary>

    public class ClientController : ApiController
    {
        /// <summary>
        /// retrieves whole list of clients
        /// </summary>
        /// <returns></returns>
        [Route("RequestClientList")]
        [HttpGet]
        public List<Client> RequestClientList()
        {
            return ClientListModel.clients;
        }

        /// <summary>
        /// registers client so that they can submit/complete jobs
        /// </summary>
        /// <param name="inIp"></param>
        /// <param name="inPort"></param>
        /// <returns></returns>
        [Route("Register")]
        [Route("Register/{inIp}/{inPort}")]
        [HttpGet]
        public uint Register(string inIp, uint inPort)
        {
            Client client = new Client();
            client.id = (uint) ClientListModel.clients.Count;
            client.ip = inIp;
            client.port = inPort;
            ClientListModel.clients.Add(client);
            return client.id;
        }

        /// <summary>
        /// updates job count for client
        /// </summary>
        /// <param name="clientNum"></param>
        [Route("UpdateClientJobCount")]
        [Route("UpdateClientJobCount/{clientNum}")]
        [HttpGet]
        public void UpdateClientJobCount(int clientNum)
        {
            ClientListModel.clients[clientNum].jobsCompleted++;
        }

        /// <summary>
        /// gets job count for client
        /// </summary>
        /// <param name="clientNum"></param>
        /// <returns></returns>
        [Route("GetJobCount")]
        [Route("GetJobCount/{clientNum}")]
        [HttpGet]
        public uint GetJobCount(uint clientNum)
        {
            return ClientListModel.clients[(int)clientNum].jobsCompleted;
        }
    }
}
