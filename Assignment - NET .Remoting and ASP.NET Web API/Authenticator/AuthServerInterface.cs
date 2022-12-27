using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

/*
    class: AuthServerInterface.cs
    author: Kade McGarraghy
    purpose: interface user services for logging in, registering, and validation
    date last modified: 27/4/21
*/


namespace Authenticator
{
 
    [ServiceContract]
    public interface AuthServerInterface
    {
        [OperationContract]
        string Register(string name, string password);

        [OperationContract]
            
        int Login(string name, string password);

        [OperationContract]
        string Validate(int token);

    }
}
