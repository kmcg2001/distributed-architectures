using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClientClasses;
using System.ServiceModel;
using IronPython.Hosting;
using Lib;
using Microsoft.Scripting.Hosting;
using System.Diagnostics;
using System.Security.Cryptography;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private uint thisPort;
        private string thisIp;
        private uint thisClientID;
        public static Mutex mut = new Mutex();

        /// <summary>
        /// initalises GUI
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            jobsCompletedTextBox.Text = "" + 0;
            resultTextBox.Text = "Awaiting result";

            PortList portList = new PortList();
            thisPort = portList.GetNewPort();
            Debug.WriteLine("port :" + thisPort);
            thisIp = "localhost";

            ThreadStart serverDelegate = new ThreadStart(RunServerThread);
            Thread thread1 = new Thread(serverDelegate);
            thread1.Start();

            ThreadStart networkingDelegate = new ThreadStart(RunNetworkingThread);
            Thread thread2 = new Thread(networkingDelegate);
            thread2.Start();
        }

        /// <summary>
        /// runs the thread that each client registers to the server on 
        /// </summary>
        public void RunServerThread()
        {
            Console.WriteLine("Starting Services Host Server");
            var tcp = new NetTcpBinding(); // binds tcp interface
            var host = new ServiceHost(typeof(ServicesHost));

            host.AddServiceEndpoint(typeof(ServicesHostInterface), tcp, "net.tcp://" + thisIp + ":" + thisPort); // other layers will connect to this endpoint to use this service
            host.Open();
            Console.WriteLine("Starting Services Host Server");

            string url = "http://localhost:12130/"; // setting url for ASP.NET web api connection
            RestClient restClient = new RestClient(url); // making connection
            RestRequest request = new RestRequest("Register/" + thisIp + "/" + thisPort);  // set up api method request
            IRestResponse response = restClient.Get(request); // call api method
            thisClientID = JsonConvert.DeserializeObject<uint>(response.Content);

            Console.WriteLine("");
            Console.ReadLine(); // keeps the server open
            host.Close();  // closes the host
        }

        /// <summary>
        /// runs the thread that handles the access of clients to complete jobs
        /// </summary>
        public void RunNetworkingThread()
        {
            string url;
            RestClient restClient;
            RestRequest request;
            IRestResponse response;

            //Job mostRecentJob = null;
            string recentSolution = "";
            while (true)
            {
                url = "http://localhost:12130/"; // setting url for ASP.NET web api connection
                restClient = new RestClient(url); // making connection
                request = new RestRequest("RequestClientList");  // set up api method request
                response = restClient.Get(request); // call api method
                List<Client> clientList = JsonConvert.DeserializeObject<List<Client>>(response.Content);
                var tcp = new NetTcpBinding();  // binds tcp interface
                var URL = "";
                ChannelFactory<ServicesHostInterface> chanFactory;
                ServicesHostInterface foob;


                foreach (Client client in clientList)
                {
         
                    URL = "net.tcp://" + client.ip + ":" + client.port;  // sets url to auth server endpoint
                    chanFactory = new ChannelFactory<ServicesHostInterface>(tcp, URL);  // makes connection to auth server
                    foob = chanFactory.CreateChannel();
               
                    if ((client.ip != thisIp) || (client.port != thisPort))
                    {
                        Job currentJob = null;
                        Dispatcher.Invoke(() =>
                        {
                            currentJob = foob.LookForJob();
               
                        });

                        /*int newJobID = foob.LookForJob();
                        if (newJobID != -1)*/

                        if (currentJob != null)
                        {
                            string task = "invalid task";
                            Dispatcher.Invoke(() =>
                            {
                                try
                                { 
                                    if (!String.IsNullOrEmpty(currentJob.GetTask()))
                                    {
                                        byte[] encodedTaskBytes = Convert.FromBase64String(currentJob.GetTask());
                                        task = Encoding.UTF8.GetString(encodedTaskBytes);

                                        SHA256 sha256 = SHA256.Create();

                                        byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(currentJob.GetTask()));
                                        if (!hash.SequenceEqual(currentJob.GetHash()))
                                        {
                                            throw new Exception();
                                        }
                       

                                        //JobList.jobs.Remove(job);
                                        request = new RestRequest("UpdateClientJobCount/" + thisClientID);  // set up api method request, gets client index from list
                                        restClient.Get(request); // call api method

                                        request = new RestRequest("GetJobCount/" + thisClientID);  // set up api method request, gets client index from list
                                        response = restClient.Get(request); // call api method
                                        uint jobCount = JsonConvert.DeserializeObject<uint>(response.Content);

                                
                                        jobsCompletedTextBox.Text = jobCount.ToString();

                                        System.Diagnostics.Debug.WriteLine("testing 1");


                                        ScriptEngine engine = Python.CreateEngine();
                                        ScriptScope scope = engine.CreateScope();
                                        engine.Execute(task, scope);
                                        Debug.WriteLine("Task!!!!!: " + task);

                                        dynamic function = scope.GetVariable("task");
                                        var result = function();

                                        recentSolution = foob.UploadSolution(currentJob.GetID(), result.ToString());
             
                                    }
                                    
                                }
                                catch (Exception e)
                                {
                                    System.Diagnostics.Debug.WriteLine("error: " + e.Message + "could not complete task: " + task);
                                    foob.UploadSolution(currentJob.GetID(), "No solution");
                                    

                                }
                            });

                        }

                    }

                    Dispatcher.Invoke(() =>
                    {
                        foreach (Job job in JobList.jobs)
                        {
                            if (job.IsAllocated())
                            {
       
                                // Debug.WriteLine("allocatedclientid " + job.GetAllocatedClientID() + " thisclientid " + thisClientID + "job id: " + job.GetID() + "solution: " + job.GetSolution());
                                if (job.GetSolution() != null)
                                {
                                    resultTextBox.Text = job.GetSolution();

                                    if (job.GetSolution() == "No solution")
                                    {
                                        workingIndicator.Content = "Error completing job";
                                    }
                                    else
                                    {
                                        workingIndicator.Content = "Job completed";
                                    }
                                    
                                }


                            }

                        }
                    });
      
                    /*else
                    {
                       
                        //if (mostRecentJob == null)
                       // {
                            Dispatcher.Invoke(() =>
                            {
                                //Debug.WriteLine("id: " + mostRecentJob.GetID() + " sl: " + recentSolution);

                                foreach (Job job in JobList.jobs)
                                {
                                    if (job.GetAllocatedClientID() == (int) thisClientID)
                                    {
                                        resultTextBox.Text = job.GetSolution();
                                    }
                                }
                                

                                workingIndicator.Content = "Job completed";
                            });
                           
                        //}
                    }*/

                }

            }
        }
        /// <summary>
        /// client submits a service to the services hoster
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void submitJobButton_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (!String.IsNullOrEmpty(taskInputTextBox.Text))
                {
                    byte[] textBytes = Encoding.UTF8.GetBytes(taskInputTextBox.Text);
                    string task = Convert.ToBase64String(textBytes);
                    SHA256 sha256 = SHA256.Create();

                    byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(task));

                    Job newJob = new Job((JobList.jobs.Count), task);
                    newJob.SetAllocatedClientID((int)thisClientID);
                    newJob.SetHash(hash);

                    JobList.jobs.Add(newJob);
                }  

            });

        }

    }

}
