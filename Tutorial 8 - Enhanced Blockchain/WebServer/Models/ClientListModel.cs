using APIClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServer.Models
{
    // file name: ClientListModel
    // author: Kade McGarraghy
    // purpose: to store list of clients
    // date last modified: 24/05/21

    public class ClientListModel
    {
        public static List<Client> clients = new List<Client>();
    }
}