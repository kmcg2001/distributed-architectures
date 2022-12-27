using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Http;
using System.Web.Mvc;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;
using Authenticator;
using System.ServiceModel;
using APIClasses;

/*
    class: IsPrimeController.cs
    author: Kade McGarraghy
    purpose:  Controller which checks if a number is prime
    date last modified: 27/4/21
*/

namespace ServiceProvider.Controllers
{
    public class IsPrimeController : ApiController
    {
        private AuthServerInterface foob;

        public IsPrimeController()
        {
            var tcp = new NetTcpBinding();  // binds tcp interface
            var URL = "net.tcp://localhost/AuthenticationService"; // sets url to auth server endpoint
            var chanFactory = new ChannelFactory<AuthServerInterface>(tcp, URL); // makes connection to auth server
            foob = chanFactory.CreateChannel();
        }


        [Route("IsPrimeNumber/{num}")]
        [Route("IsPrimeNumber/")]
        [HttpPost]
        // POST: IsPrimeNumber/{num}
        public ServiceData Post([FromBody] int token, int num) // method is of type post because it needs token from JSON body
        {
            ServiceData serviceData = new ServiceData();

            bool isPrime = true; // start of assuming all prime numbers true

            string validatedStatus = foob.Validate(token);
            
            if (validatedStatus.Equals("Successfully validated"))
            {

                for (int divisor = 2; divisor <= (num / 2); divisor++) // starts from 2 because negative numbers, 1, 0 not prime numbers 
                {
                    if ((num % divisor) == 0) // if the remainder of a number divided by divisor = 0, that means it has a factor(s) so is NOT a prime number
                    {
                        isPrime = false;
                        break; // if not prime number, don't need to do any more tests on it
                    }
                   
                    // goes until num / 2 because anything past that will have a decimal result OR be the number/divided itself (which can be done on any number)
                }
                
                if (isPrime && (num >= 2)) // makes sure number not less than 2 (i.e. negative) to be prime
                {
                    serviceData.result.Add("Prime");
                }
                else
                {
                    serviceData.result.Add("Not Prime"); 
                }
                serviceData.status = ServiceData.AUTHENTICATED;

            }
            else
            {
                serviceData.status = ServiceData.DENIED;
                serviceData.reason = ServiceData.AUTH_ERROR;
            }

            return serviceData;

        }
    }
}