using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{   
    
    /// <summary>
     ///  file name: Job.cs
     ///  author: Kade McGarraghy
     ///  purpose: represents job that client can sign up to
     ///  date: 24/05/21
     /// </summary>
    [DataContract]
    public class Job
    {

        [DataMember] private static int id;
        [DataMember] private string task;
        [DataMember] private string solution;
        [DataMember] private bool allocated;
        [DataMember] private int allocatedClientID;
        [DataMember] private byte[] hash;

        public Job(int inID, string inTask)
        {
            id = inID;
            task = inTask;
            allocated = false;
        }

        public void SetSolution(string inSolution)
        {
            solution = inSolution;
        }

        public void SetAllocatedClientID(int inAllocatedClientID)
        {
            allocatedClientID = inAllocatedClientID;
        }

        public void SetHash(byte[] inHash)
        {
            hash = inHash;
        }


        public void Allocate()
        {
            allocated = true;
        }

        public bool IsAllocated()
        {
            return allocated;
        }

        public int GetID()
        {
            return id;
        }

        public string GetTask()
        {
            return task;
        }

        public string GetSolution()
        {
            return solution;
        }

        
        public int GetAllocatedClientID()
        {
            return allocatedClientID;
        }

        public byte[] GetHash()
        {
            return hash;
        }

    }
}
