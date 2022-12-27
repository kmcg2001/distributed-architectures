using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    class: RegistryDescription.cs
    author: Kade McGarraghy
    purpose: data construct to be used as template for .NET json serlializer -> for use with service provider controller
    date last modified: 27/4/21
*/


namespace APIClasses
{
    public class ServiceData
    {
        public string status;
        public static readonly string DENIED = "Denied"; // giving some read only constants to use for common error messages for consistency
        public static readonly string AUTHENTICATED = "Authenticated";

        public string reason;
        public static readonly string AUTH_ERROR = "Authentication error";
        public static readonly string OPERAND_ERROR = "Wrong operand type";

        public List<string> result = new List<string>(); // result in string list format because it accomodates to every possible result return type (and GUI shouldn't deal with converting data types)
    }
}
