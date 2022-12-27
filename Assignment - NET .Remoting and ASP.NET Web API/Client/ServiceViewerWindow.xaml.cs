using APIClasses;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

/*
   class: ServiceViewerWindow.xaml.cs
   author: Kade McGarraghy
   purpose:  Interaction logic for  ServiceViewerWindow.xaml
   date last modified: 27/4/21
*/

namespace Client
{
    public partial class ServiceViewerWindow : Window
    {
        // CLASS CONSTANTS
        public static readonly string ALLSERVICES = "AllServices";
        public static readonly string SEARCH = "Search";
        
        private RestClient client;
        private RestRequest request;
        private IRestResponse response;
        private int token;

        private List<ServiceDescription> services;
        private int currServiceIndex;
        private int numServices;
        private bool servicesFound;

        private string mode;


        public ServiceViewerWindow()
        {
            InitializeComponent();

            string url = "http://localhost:57174/"; // setting url for ASP.NET web api connection
        
            client = new RestClient(url); // making connection
            token = 0;

            currServiceIndex = 0;
            servicesFound = false;
            mode = ""; 
        }

        public void SetToken(int inToken)
        {
            token = inToken;
        }

        private void showAllServicesButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (!mode.Equals(ALLSERVICES)) // mode system means you can only do one type of request at a time.
            {
                mode = ALLSERVICES;
                string url = "http://localhost:57174/"; // setting url for ASP.NET web api connection
                client = new RestClient(url); // making connection
                request = new RestRequest("AllServices/");  // set up api method request
                request.AddJsonBody(token); // add JSON object in body to be sent when api method is called, user token makes sure they are validated to use service
                response = client.Post(request); // call api method

                RegistryData registryData;
                try
                {
                    registryData = JsonConvert.DeserializeObject<RegistryData>(response.Content); // deseralises object returned from JSON objct to RegistryData object

                }
                catch (Newtonsoft.Json.JsonReaderException ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                    registryData = new RegistryData();
                    registryData.status = RegistryData.DENIED;
                }


                LoadNewServiceList(registryData);

                mode = "";

            }
            
        }

        private void searchForServiceButton_Click(object sender, RoutedEventArgs e)
        {
            if (!mode.Equals(SEARCH))
            {
                mode = SEARCH;
                string searchString = searchTextBox.Text;
                System.Diagnostics.Debug.WriteLine("search string: " + searchString);
                string url = "http://localhost:57174/"; // setting url for ASP.NET web api connection
                client = new RestClient(url);  // making connection
                request = new RestRequest("Search/" + searchString); // set up api method request
                request.AddJsonBody(token);  // add JSON object in body to be sent when api method is called, user token makes sure they are validated to use service
                response = client.Post(request); // call api method
                RegistryData registryData;
                try
                {
                    registryData = JsonConvert.DeserializeObject<RegistryData>(response.Content); // deseralises object returned from JSON object to RegistryData object

                }
                catch (Newtonsoft.Json.JsonReaderException ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                    registryData = new RegistryData();
                    registryData.status = RegistryData.DENIED;

                }

                LoadNewServiceList(registryData);

                mode = "";

            }

        }

        private void prevServiceButton_Click(object sender, RoutedEventArgs e)
        {
            if (servicesFound)
            {
                if (currServiceIndex > 0) // don't want index to be less than 0
                {
                    currServiceIndex--;
                    ShowNewService();
                    listBox.SelectedItem = listBox.Items[currServiceIndex];

                }
            }
        }

        private void nextServiceButton_Click(object sender, RoutedEventArgs e)
        {
            if (servicesFound)
            {
                if (currServiceIndex < (numServices - 1)) // dont want index to be greater than number of services in list
                {
                    currServiceIndex++;
                    ShowNewService();
                    listBox.SelectedItem = listBox.Items[currServiceIndex];

                }
            }
        }

        private void LoadNewServiceList(RegistryData registryData)
        {
            servicesFound = false;

            if ((registryData.status).Equals(RegistryData.AUTHENTICATED)) // check that user was authenticated to use service
            {
                if (registryData.result != null) // make sure there was a result returned 
                {
                    numServices = registryData.result.Count;
                    if (numServices > 0)
                    {
                        servicesFound = true; // services are found if the result contains at least one item
                        services = registryData.result;
                    }
                }
            }


            listBox.Items.Clear(); // gets rid of old found services in list before putting new ones in
            for (int i = 0; i < numServices; i++)
            {
                listBox.Items.Add(services[i].name);
            }

            currServiceIndex = 0;

            if (servicesFound)
            {
                listBox.SelectedItem = listBox.Items[0];
                ShowNewService();
            }
            else
            {
                ShowServiceDefaults(); // if no services found, we want to show the default values for every service attribute
            }
        }

        private void ShowNewService()
        {
            // setting GUI values for selected service
            nameTextBox.Text = services[currServiceIndex].name; 
            descriptionTextBox.Text = services[currServiceIndex].description;
            api_endpointTextBox.Text = services[currServiceIndex].api_endpoint;
            num_operandsTextBox.Text = services[currServiceIndex].num_operands.ToString();
            type_operandsTextBox.Text = services[currServiceIndex].type_operands;

            numServiceResultsLabel.Content = ((currServiceIndex + 1) + " / " + numServices + " services found");

            operandsListBox.Items.Clear();

            // dynamically create textboxes depending on number of operands required
            for (int i = 0; i < services[currServiceIndex].num_operands; i++)
            {
                TextBox textBox = new TextBox();
                textBox.Text = "<operand " + i + " >"; // these textboxes will be where user inputs operands
                operandsListBox.Items.Add(textBox); // add operands to list box so it can infinitely scroll regardless of num of operands
            }
            resultTextBox.Clear();

        }

        private void ShowServiceDefaults()
        {
            // shows the default values for every service attribute
            nameTextBox.Text = "Service Name";
            descriptionTextBox.Text = "Service Description";
            api_endpointTextBox.Text = "Service Endpoint";
            num_operandsTextBox.Text = "Number of Operands";
            type_operandsTextBox.Text = "Operand Type";

            numServiceResultsLabel.Content = "0 / 0 services found";

            operandsListBox.Items.Clear();
            resultTextBox.Clear();


        }


        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox.SelectedIndex != -1)
            {
                currServiceIndex = listBox.SelectedIndex; // if user changes selection on services list box, that selected service should be shown
                ShowNewService();
            }
        }

        private void testServiceButton_Click(object sender, RoutedEventArgs e)
        {
            // string url = "http://localhost:57021/"; // working URL with port
            string url = services[currServiceIndex].api_endpoint;  // sets url for ASP.NET web api connection to given endpoint

            bool validURL = true;
            string resultStr = "";
            try
            {
                client = new RestClient(url); // making connection}
            }
            catch (UriFormatException fe)
            {
                validURL = false;
                resultStr = "No result. Invalid endpoint provided";
            }

            if (validURL)
            {
                string requestStr = "";
                for (int i = 0; i < services[currServiceIndex].num_operands; i++)
                {

                    TextBox tempTextBox = (TextBox)operandsListBox.Items[i]; // gets all user entered operand values from text boxes in operand list box
                    requestStr = requestStr + "/" + tempTextBox.Text;
                }

                request = new RestRequest(requestStr);  // set up api method request
                request.AddJsonBody(token); // add JSON object in body to be sent when api method is called, user token makes sure they are validated to use service
                response = client.Post(request); // call api method

                ServiceData serviceData;
                try
                {
                    serviceData = JsonConvert.DeserializeObject<ServiceData>(response.Content);

                }
                catch (Newtonsoft.Json.JsonReaderException ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                    serviceData = new ServiceData();
                    serviceData.status = ServiceData.DENIED;
                    serviceData.reason = ServiceData.OPERAND_ERROR; // this error will be caused by incorrect operand types

                }

                if (serviceData != null)
                {
                    if (serviceData.status == (null))
                    {
                        serviceData.status = ServiceData.DENIED;
                        serviceData.reason = ServiceData.OPERAND_ERROR;
                    }


                    if (serviceData.status.Equals(ServiceData.AUTHENTICATED))
                    {

                        for (int i = 0; i < serviceData.result.Count; i++)
                        {
                            resultStr = resultStr + serviceData.result[i];
                            if (i < (serviceData.result.Count - 1))
                            {
                                resultStr = resultStr + ", ";
                            }
                        }

                    }
                    else if (serviceData.status.Equals(ServiceData.DENIED))
                    {
                        if (serviceData.reason.Equals(ServiceData.AUTH_ERROR))
                        {
                            resultStr = "Authentication Error. No result"; // this error will be caused by a user's authentication being denied

                        }

                        if (serviceData.reason.Equals(ServiceData.OPERAND_ERROR))
                        {
                            resultStr = "No result. Your operands should be of type " + services[currServiceIndex].type_operands + ".";

                        }

                    }
                    else
                    {
                        resultStr = "No result.";
                    }

                }
                else
                {
                    resultStr = "Failed to connect to service. Your endpoint may be incorrect."; // this error will be caused by failure to connect to endpoint / incorrect endpoint
                }
            }

            resultTextBox.Text = resultStr;
            mode = "";

        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mode = "";
        }
    }
}
