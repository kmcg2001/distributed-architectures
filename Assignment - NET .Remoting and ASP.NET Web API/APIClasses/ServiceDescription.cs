using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    class: ServiceDescription.cs
    author: Kade McGarraghy
    purpose: data construct to be used as template for .NET json serlializer -> represents a calc service
    date last modified: 27/4/21
*/


namespace APIClasses
{
    public class ServiceDescription
    {
        public string name;
        public string description;
        public string api_endpoint;
        public int num_operands;
        public string type_operands;
    }
}
