using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Web.Http;
using APIClasses;
using Authenticator;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;

/*
    class: PublishController.cs
    author: Kade McGarraghy
    purpose:  Controller for publishing and unpublishing services
    date last modified: 27/4/21
*/


namespace Registry.Controllers
{
    public class PublishController : ApiController
    {
        private AuthServerInterface foob;
        public PublishController()
        {
            var tcp = new NetTcpBinding();  // binds tcp interface
            var URL = "net.tcp://localhost/AuthenticationService"; // sets url to auth server endpoint
            var chanFactory = new ChannelFactory<AuthServerInterface>(tcp, URL); // makes connection to auth server

            foob = chanFactory.CreateChannel();
        }

       [Route("Publish/{token}")]
       [Route("Publish/")]
       [HttpPost]
        // POST Publish/{token}
        public RegistryData Post([FromBody] ServiceDescription desc, int token) // method is type post because it needs service description from JSON body
        {
            RegistryData registryData = new RegistryData();
            bool success = true;

            string validatedStatus = foob.Validate(token);

            if (!validatedStatus.Equals("Successfully validated"))
            {
                success = false;
                registryData.reason = RegistryData.AUTH_ERROR;
            }
            else
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                bool needsNewLine = false;

                try
                {
                    StreamWriter writer = new StreamWriter(Path.Combine(path, "serviceList.txt"), true);
                    using (writer)
                    {
                        if (needsNewLine)
                        {
                            writer.WriteLine("");
                        }
                        writer.WriteLine("Name: " + desc.name);
                        writer.WriteLine("Description: " + desc.description);
                        writer.WriteLine("API endpoint: " + desc.api_endpoint);
                        writer.WriteLine("Number of operands: " + desc.num_operands);
                        writer.WriteLine("Operand type: " + desc.type_operands);
                    }
                }
                catch (IOException e1)
                {
                    success = false;
                    Console.WriteLine("Exception: " + e1.Message);
                }
                catch (ObjectDisposedException e2)
                {
                    success = false;
                    Console.WriteLine("Exception: " + e2.Message);
                }
            }

            if (success)
            {
                registryData.status = RegistryData.AUTHENTICATED;
                registryData.reason = "Successful Publishing of " + desc.name + " service description";
            }
            else
            {
                registryData.status = RegistryData.DENIED;
            }

            return registryData;
        }

        [Route("Unpublish/")]
        [Route("Unpublish/{value}")]
        [HttpPost]
        // POST Unpublish/{value}
        public RegistryData Post([FromBody] int token, string value) // method is type post because it needs token from JSON body
        {
            RegistryData registryData = new RegistryData();
            bool success = false;

            string validatedStatus = foob.Validate(token);

            if (!validatedStatus.Equals("Successfully validated"))
            {
                success = false;
                registryData.status = RegistryData.DENIED;
                registryData.reason = RegistryData.AUTH_ERROR;
            }
            else
            {
                registryData.status = RegistryData.AUTHENTICATED;
                List<int> oldServices = new List<int>();
               
                int servicePos = 1;

                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


                string line;
                try
                {
                    StreamReader reader = new StreamReader(Path.Combine(path, "serviceList.txt"));

                    int lineNum = 0;
                    line = reader.ReadLine();
                    lineNum++;

                    while (line != null)
                    {
                
                        if (line.ToUpper().Equals("API ENDPOINT: " + (value.ToUpper()))) // makes sure capitalisation of endpoint doesn't affect unpublishing
                        {
                            oldServices.Add(servicePos);
                            success = true;
                        }


                        if (lineNum % 5 == 0)
                        {
                            servicePos++;
                        }
                        lineNum++;

                        line = reader.ReadLine();

                    }

                    reader.Close();

                    // Read through file a second time to delete affected services IF there are services to be deleted

                    if (oldServices.Count != 0)
                    {

                        reader = new StreamReader(Path.Combine(path, "serviceList.txt"));

                        var temp = Path.GetTempFileName();


                        servicePos = 1;
                        int oldServicesIndex = 0;
                        lineNum = 0;
                        line = reader.ReadLine();
                        lineNum++;

                        StreamWriter writer = new StreamWriter(temp, true);
                        using (writer)
                        {
                            while (line != null)
                            {
                                System.Diagnostics.Debug.WriteLine("ServicePos: " + servicePos + " / oldServices[index]: " + oldServices[oldServicesIndex]);
                                if (servicePos == oldServices[oldServicesIndex]) // if the service item matches service item to be deleted
                                {
                                    if ((oldServicesIndex + 1) < oldServices.Count)
                                    {
                                        if (lineNum % 5 == 0) // if all 5 service attributes have been 'removed'
                                        {
                                            oldServicesIndex++; // tick off this service, move to the next to be deleted
                                        }
                                    }
                                }
                                else
                                {
                                    writer.WriteLine(line); // since service is not deleted, need to write it to temp file
                                    System.Diagnostics.Debug.WriteLine(line);

                                }

                                if (lineNum % 5 == 0)
                                {
                                    servicePos++; // need to keep track of which service we are up to
                                }
                                lineNum++;
                                System.Diagnostics.Debug.WriteLine(lineNum);

                                line = reader.ReadLine();
                            }
                        }

                        reader.Close();

                        File.Delete(Path.Combine(path, "serviceList.txt"));
                        File.Move(temp, Path.Combine(path, "serviceList.txt"));
                    }
                }
                catch (IOException e1)
                {
                    Console.WriteLine("Exception: " + e1.Message);
                    registryData.reason = "Input Output Error";
                }
                catch (ObjectDisposedException e2)
                {
                    Console.WriteLine("Exception: " + e2.Message);
                    registryData.reason = "Input Output Error";
                }
            }

            if (success)
            {
                registryData.status = RegistryData.AUTHENTICATED;
                registryData.reason = "Successful unpublishing of services that include " + value + ".";
            }
            else
            {
                registryData.reason = "Unsuccessful unpublishing of services that include " + value + ". No services found";
            }

            return registryData;
        }

    }
}
