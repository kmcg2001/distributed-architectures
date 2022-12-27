using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Lib
{

     /// <summary>
     ///  file name: ServicesHost.cs
     ///  author: Kade McGarraghy
     ///  purpose: host the services that clients can use e.g. looking for job and uploading solution
     ///  date: 24/05/21
     /// </summary>

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false, InstanceContextMode = InstanceContextMode.Single)]

    public class ServicesHost : ServicesHostInterface
    {
        public static List<Job> jobList;

        /// <summary>
        /// retrieves next job for a client to complete
        /// </summary>
        /// <returns></returns>
        public Job LookForJob()
        {
            Job nextJob = null;
            int foundIndex = -1;

            jobList = JobList.jobs;
            int index = -1;
            foreach (Job job in jobList)
            {
                index++;
                if (!job.IsAllocated()) // if job has not been allocated yet
                {
                    System.Diagnostics.Debug.WriteLine("Job found: " + job.GetTask());
                    nextJob = job;
                    job.Allocate();
                    
                    break;
                }
              
            }
            return nextJob;
        }

        /// <summary>
        /// client can upload solution to assigned job
        /// </summary>
        /// <param name="jobID"></param>
        /// <param name="solution"></param>
        /// <returns></returns>
        public string UploadSolution(int jobID, string solution)
        {
            bool uploaded = false;

            string returnedSolution;

            if (jobID < JobList.jobs.Count)
            {
                if (JobList.jobs[jobID] != null)
                {
                    if (solution != null)
                    {
                        JobList.jobs[jobID].SetSolution(solution);

                        uploaded = true;
                    }

                }
            }
          
            if (uploaded)
            {
                returnedSolution = solution;
            }
            else
            {
                returnedSolution = null;
            }
            return returnedSolution;
        }
    }
}
