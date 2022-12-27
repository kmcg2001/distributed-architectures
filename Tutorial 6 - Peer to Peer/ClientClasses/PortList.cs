using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientClasses
{    
    
    /// <summary>
     ///  file name: PortList.cs
     ///  author: Kade McGarraghy
     ///  purpose: makes sure no two clients' ports are the same
     ///  date: 24/05/21
     /// </summary>
    public class PortList
    {
        private static Random rand = new Random();
        public static List<uint> ports = new List<uint>();
        
        
        public PortList()
        {
        }

        public uint GetNewPort()
        {
           uint newPort = (uint) rand.Next(1000, 9999);
  
           bool portFound = false;

           int index;


            List<uint> tempPorts = ports;
            tempPorts.Add(newPort);

            if (tempPorts.Count > 0)
            {
                while (!portFound)
                {
                    index = -1;
                    int listSize = tempPorts.Count;
                    bool resetSearch = false;

                    foreach (uint port in tempPorts)
                    {
                        index++;
                        
                        if (newPort == port)
                        {
                            newPort = (uint) rand.Next(1000, 9999);
                            resetSearch = true;
                            Debug.WriteLine("index : " + index + " port: " + port + " new port: " + newPort);
                        }

                        if ((index == (listSize - 1)) && !resetSearch)
                        {
                            portFound = true;
                        }
                    }

                }
               
            }
            ports.Add(newPort);

            return (uint)newPort;
        }
    }
}
