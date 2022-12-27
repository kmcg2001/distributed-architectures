// Credit: Alex Starling - Distributed Computing - 2021
using System.Runtime.Serialization;

namespace ServerInterface
{
    /// <summary>
    /// file name: IndexOutOfRangeFault.cs
    /// author: Alex Starling (reference: Tutorial 1 Solution provided)
    /// purpose: interface for data server -> user services to get database info
    /// date last modified: 23/05/21
    /// </summary>

    [DataContract] // serialised so it can cross network boundary
    public class IndexOutOfRangeFault
    {
        [DataMember]
        public string Issue { get; set; }
    }
}
