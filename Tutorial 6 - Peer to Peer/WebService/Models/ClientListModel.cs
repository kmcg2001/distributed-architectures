using ClientClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// file name: ClientListModel
// author: Kade McGarraghy
// purpose: to store list of clients
// date last modified:

namespace WebService.Models
{
    public class ClientListModel
    {
        public static List<Client> clients = new List<Client>();
    }
}