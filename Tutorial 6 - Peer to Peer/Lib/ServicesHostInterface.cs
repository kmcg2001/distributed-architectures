using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Lib
{
    /// <summary>
    ///  file name: ServicesHostInterface.cs
    ///  author: Kade McGarraghy
    ///  purpose: interfaces for services host for client to use
    ///  date: 24/05/21
    /// </summary>

    [ServiceContract]
    public interface ServicesHostInterface
    {
        [OperationContract]
        Job LookForJob();

        [OperationContract]
        string UploadSolution(int jobID, string solution);
    }
}
