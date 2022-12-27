using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;
using System.ServiceModel;
using Authenticator;
using APIClasses;

/*
    class: AddController.cs
    author: Kade McGarraghy
    purpose:  Controller for calc services involving ADDITION
    date last modified: 27/4/21
*/

namespace ServiceProvider.Controllers
{
    public class AddController : ApiController
    {
        private AuthServerInterface foob;

        public AddController()
        {
            var tcp = new NetTcpBinding();  // binds tcp interface
            var URL = "net.tcp://localhost/AuthenticationService"; // sets url to auth server endpoint
            var chanFactory = new ChannelFactory<AuthServerInterface>(tcp, URL); // makes connection to auth server

            foob = chanFactory.CreateChannel();
        }

        [Route("ADDTwoNumbers/{num1}/{num2}")]
        [Route("ADDTwoNumbers/")]
        [HttpPost]
        // POST: ADDTwoNumbers/{num1}/{num2}/
        public ServiceData Post([FromBody] int token, int num1, int num2) // method is of type post because it needs token from JSON body
        {
            ServiceData serviceData = new ServiceData();

            string validatedStatus = foob.Validate(token);

            if (validatedStatus.Equals("Successfully validated"))
            {
                int result = num1 + num2;
                serviceData.result.Add(result.ToString()); // result in string form because GUI doesn't know data type
                serviceData.status = ServiceData.AUTHENTICATED;
            }
            else
            {
                serviceData.status = ServiceData.DENIED;
                serviceData.reason = ServiceData.AUTH_ERROR;
            }

            return serviceData;
        }

        [Route("ADDThreeNumbers/{num1}/{num2}/{num3}")]
        [Route("ADDThreeNumbers/")]
        [HttpPost]
        // POST: ADDThreeNumbers/{num1}/{num2}/{num3}/
        public ServiceData Post([FromBody] int token, int num1, int num2, int num3) // method is of type post because it needs token from JSON body
        {
            ServiceData serviceData = new ServiceData();

            string validatedStatus = foob.Validate(token);

            if (validatedStatus.Equals("Successfully validated"))
            {
                int result = num1 + num2 + num3;
                serviceData.result.Add(result.ToString()); // result in string form because GUI doesn't care what data type
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