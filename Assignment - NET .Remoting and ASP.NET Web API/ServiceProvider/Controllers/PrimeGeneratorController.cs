using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;
using System.ServiceModel;
using Authenticator;
using APIClasses;

/*
    class: PrimeGeneratorController.cs
    author: Kade McGarraghy
    purpose:  Controller which performs calc services involving PRIME NUMBER GENERATION
    date last modified: 27/4/21
*/

namespace ServiceProvider.Controllers
{
    public class PrimeGeneratorController : ApiController
    {
        private AuthServerInterface foob;

        public PrimeGeneratorController()
        {
            var tcp = new NetTcpBinding();  // binds tcp interface
            var URL = "net.tcp://localhost/AuthenticationService"; // sets url to auth server endpoint
            var chanFactory = new ChannelFactory<AuthServerInterface>(tcp, URL); // makes connection to auth server
            foob = chanFactory.CreateChannel();
        }

       
        [Route("GeneratePrimeNumberstoValue/{num}")]
        [Route("GeneratePrimeNumberstoValue/")]
        [HttpPost]
        // // POST: GeneratePrimeNumberstoValue/{num}/
        public ServiceData Post([FromBody] int token, int num) // method is of type post because it needs token from JSON body
        {
            ServiceData serviceData = new ServiceData();
            ServiceData isPrimeData;
            IsPrimeController isPrimeController = new IsPrimeController();

            List<string> primes = new List<string>();

            string validatedStatus = foob.Validate(token);

            if (validatedStatus.Equals("Successfully validated"))
            {
               //  Generates all prime numbers up to a value
                for (int i = 2; i <= num; i++) // start at 2 because prime can't be less than 2
                {
                    isPrimeData = isPrimeController.Post(token, i); // call isPrime controller because logic already done there
                    if (isPrimeData.result[0].Equals("Prime")) 
                    {
                        primes.Add(i.ToString()); // if result returned says that number is prime, add it to list
                    }
                }
                if (primes.Count == 0)
                {
                    serviceData.result.Add("No primes found");
                }
                else
                {
                    serviceData.result = primes;
                }
                serviceData.status = ServiceData.AUTHENTICATED;
            }
            else
            {
                serviceData.status = ServiceData.DENIED;
                serviceData.reason = "Authentication Error";
            }

            return serviceData;
        }

        [Route("GeneratePrimeNumbersinRange/{num1}/{num2}")]
        [Route("GeneratePrimeNumbersinRange/")]
        [HttpPost]
        // POST: GeneratePrimeNumbersinRange/{num1}/{num2}/
        public ServiceData Post([FromBody] int token, int num1, int num2) // method is of type post because it needs token from JSON body
        {
            // Generates all prime numbers in a range

            //makes sure the range goes from the lowest to the highest number
            int lowest, highest;
            if (num1 < num2)
            {
                lowest = num1;
                highest = num2;
            }
            else
            {
                lowest = num2;
                highest = num1;
            }
            
            ServiceData serviceData = new ServiceData();
            ServiceData isPrimeData;
            IsPrimeController isPrimeController = new IsPrimeController();

            List<string> primes = new List<string>();

            string validatedStatus = foob.Validate(token);

            if (validatedStatus.Equals("Successfully validated"))
            {
                for (int i = lowest; i <= highest; i++)
                {

                    isPrimeData = isPrimeController.Post(token, i); // call isPrime controller because logic already done there
                    if (isPrimeData.result[0].Equals("Prime"))
                    {
                        primes.Add(i.ToString()); // if result returned says that number is prime, add it to list
                    }
                }
                if (primes.Count == 0)
                {
                    serviceData.result.Add("No primes found");
                }
                else
                {
                    serviceData.result = primes;
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