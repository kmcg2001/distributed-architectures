using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Authenticator;
using RestSharp;
using APIClasses;
using Newtonsoft.Json;

/*
    class: ServicePublisher.cs
    author: Kade McGarraghy
    purpose:  Runs console application for developers to publish/unpublish services once authenticated
    date last modified: 27/4/21
*/

namespace ServicePublisher
{
    class Program
    {
        static void Main(string[] args)
        {

            var tcp = new NetTcpBinding(); // binds tcp interface
            var URL = "net.tcp://localhost/AuthenticationService";  // sets url to auth server endpoint
            var chanFactory = new ChannelFactory<AuthServerInterface>(tcp, URL);  // makes connection to auth server

            AuthServerInterface foob = chanFactory.CreateChannel();

            string username;
            string password;
            int token = -1;
            bool loginSuccess = false;
            bool registerSuccess = false;

            char choice = 'D';

            while (choice != 'X' && loginSuccess != true)
            {
                Console.WriteLine("Welcome to the Service Publisher. Login (L) or Register (R)?");
                try
                {
                    choice = Convert.ToChar(Console.ReadLine().ToUpper());
                }
                catch (System.FormatException)
                {
                    choice = 'D'; // if they don't put in a character, will have to re-enter input (sent to default case)
                }
                
                switch (choice)
                {
                    case 'L':
                        while (loginSuccess == false)
                        {
                            Console.WriteLine("Username:");
                            username = Console.ReadLine();
                            Console.WriteLine("Password:");
                            password = Console.ReadLine();
                            token = foob.Login(username, password);

                            string validateResult = foob.Validate(token);
                            if (validateResult.Equals("Successfully validated"))
                            {
                                loginSuccess = true;
                                Console.WriteLine(validateResult);
                            }
                            else
                            {
                                Console.WriteLine(validateResult + ". Try again.");
                            }

                        }

                        break;

                    case 'R':
                        while (registerSuccess == false)
                        {
                            Console.WriteLine("Pick a username:");
                            username = Console.ReadLine();
                            Console.WriteLine("Pick a password:");
                            password = Console.ReadLine();
                            string registerResult = foob.Register(username, password);

                            if (registerResult.Equals("Successfully registered"))
                            {
                                registerSuccess = true;
                                Console.WriteLine(registerResult);
                            }
                            else
                            {
                                Console.WriteLine(registerResult + ". Try again.");
                            }

                        }

                        break;

                    default:
                        Console.WriteLine("Please use either 'L' or 'R' to make your choice!");
                        break; 
                        
                }
            }

            if (loginSuccess)
            {
                choice = 'D';
                string serviceName;
                string serviceDesc;
                string serviceEndpoint;
                int serviceOpNum;
                string serviceOpType;

                string url = "http://localhost:57174/"; // setting url for ASP.NET web api connection
                RestClient client = new RestClient(url);  // making connection
                RestRequest request;
                IRestResponse response;

                while (choice != 'X')
                {
                    Console.WriteLine("Publish ('P') or Unpublish ('U') a service? 'X' to exit");
                    try
                    {
                        choice = Convert.ToChar(Console.ReadLine().ToUpper());
                    }
                    catch (System.FormatException)
                    {
                        choice = 'D'; // if they don't put in a character, will have to re-enter input (sent to default case)
                    }
                    switch (choice)
                    {
                        case 'P':

                            ServiceDescription serviceDescObj = new ServiceDescription();

                            Console.WriteLine("Service name:");
                            serviceDescObj.name = Console.ReadLine();
                            Console.WriteLine("Description:");
                            serviceDescObj.description = Console.ReadLine();
                            Console.WriteLine("API Endpoint:");
                            serviceDescObj.api_endpoint = Console.ReadLine();

                            bool correctType = false;
                            string instruction = "Number of Operands:";
                            string outputStr = instruction;
                            while (!correctType)
                            {
                                Console.WriteLine(outputStr);
                                string numOperandsInput = Console.ReadLine();
                                if (Int32.TryParse(numOperandsInput, out int res)) // makes sure number entered is integer
                                {
                                    int newNum = Int32.Parse(numOperandsInput);
                                    if (newNum > 0) // makes sure number entered is a positive number > 0 (it's a calculator, needs at least 1 operand)
                                    {
                                        serviceDescObj.num_operands = newNum;
                                        correctType = true;
                                    }
                                   
                                }
                                outputStr = "You need to enter a positive integer number!" + "\n" + instruction;
                            }


                            Console.WriteLine("Operand Type:");
                            serviceDescObj.type_operands = Console.ReadLine();

                            request = new RestRequest("Publish/" + token); // set up api method request
                            request.AddJsonBody(serviceDescObj); // add JSON object (service description) in body to be sent when api method is called
                            response = client.Post(request); // call api method

                            RegistryData registryData = JsonConvert.DeserializeObject<APIClasses.RegistryData>(response.Content);

                            Console.WriteLine("status: " + registryData.status);
                            Console.WriteLine("reason: " + registryData.reason);

                            break;

                        case 'U':

                            string endpoint;

                            Console.WriteLine("Endpoint of the service to unpublish:");
                            endpoint = Console.ReadLine();

                            request = new RestRequest("Unpublish/" + endpoint); // set up api method request
                            request.AddJsonBody(token); // add JSON object in body to be sent when api method is called, user token makes sure they are validated to use service
                            response = client.Post(request); // call api method

                            registryData = JsonConvert.DeserializeObject<APIClasses.RegistryData>(response.Content);

                            Console.WriteLine("status: " + registryData.status);
                            Console.WriteLine("reason: " + registryData.reason);

                            break;

                        default:

                            Console.WriteLine("Please use either 'P' or 'U' to make your choice!");
                            break;

                    }
                }
                
            }
        }

    }
}

