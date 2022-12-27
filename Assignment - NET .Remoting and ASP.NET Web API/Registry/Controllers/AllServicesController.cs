using APIClasses;
using Authenticator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;

/*
    class: AllServices.cs
    author: Kade McGarraghy
    purpose:  Controller for displaying all calc services
    date last modified: 27/4/21
*/

namespace Registry.Controllers
{
    public class AllServicesController : ApiController
    {

        private AuthServerInterface foob;

        public AllServicesController()
        {
            var tcp = new NetTcpBinding();  // binds tcp interface
            var URL = "net.tcp://localhost/AuthenticationService"; // sets url to auth server endpoint
            var chanFactory = new ChannelFactory<AuthServerInterface>(tcp, URL); // makes connection to auth server

            foob = chanFactory.CreateChannel();
        }

        // POST AllServices/
        [Route("AllServices/")]
        [HttpPost]
        public RegistryData Post([FromBody] int token)
        {
            RegistryData registryData = new RegistryData();
            List<ServiceDescription> services = new List<ServiceDescription>();
            bool success = true;

            string validatedStatus = foob.Validate(token);

            if (!validatedStatus.Equals("Successfully validated"))
            {
                success = false;
                registryData.reason = RegistryData.AUTH_ERROR;
            }
            else
            {

                ServiceDescription newServiceDesc = new ServiceDescription();

                String line;
                try
                {
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    StreamReader reader = new StreamReader(Path.Combine(path, "serviceList.txt"));

                    int lineNum = 0;
                    line = reader.ReadLine();
                    lineNum++;

                    while (line != null)
                    {

                        string[] tokens = line.Split(new[] { ": " }, StringSplitOptions.None);
                        switch (lineNum)
                        {
                            case 1:
                                if (newServiceDesc == (null))
                                {
                                    newServiceDesc = new ServiceDescription();
                                }

                                newServiceDesc.name = tokens[1]; // tokens[0] is label, next element is actual data item
                                break;

                            case 2:
                                newServiceDesc.description = tokens[1];
                                break;


                            case 3:
                                newServiceDesc.api_endpoint = tokens[1];
                                break;

                            case 4:
                                newServiceDesc.num_operands = Int32.Parse(tokens[1]);
                                break;

                            case 5:
                                newServiceDesc.type_operands = tokens[1];
                                services.Add(newServiceDesc);
                                newServiceDesc = null;
                                lineNum = 0;
                                break;

                        }

                        line = reader.ReadLine();
                        lineNum++;

                    }

                    reader.Close();
                }
                catch (IOException e1)
                {
                    Console.WriteLine("Exception: " + e1.Message);
                    success = false;
                    registryData.reason = "Input Output Error";
                }
                catch (ObjectDisposedException e2)
                {
                    Console.WriteLine("Exception: " + e2.Message);
                    success = false;
                    registryData.reason = "Input Output Error";

                }
            }

           

            if (success)
            {
                registryData.status = RegistryData.AUTHENTICATED;
                registryData.reason = "Showing all services";
            }
            else
            {
                registryData.status = RegistryData.DENIED;
            }

            registryData.result = services;
            return registryData;
        }

    }
}
