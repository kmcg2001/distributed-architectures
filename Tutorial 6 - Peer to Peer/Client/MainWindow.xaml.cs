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

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ThreadStart serverDelegate = new ThreadStart(ServerThread.Run);
            Thread serverThread = new Thread(serverDelegate);

            ThreadStart networkingDelegate = new ThreadStart(NetworkingThread.Run);
            Thread networkingThread = new Thread(networkingDelegate);
        }
    }

    class ServerThread
    {
        public static void Run()
        {

        }
    }

    class NetworkingThread
    {




        public static void Run()
        {
            string url = "http://localhost:12130/"; // setting url for ASP.NET web api connection
            RestClient client;
            RestRequest request;
            IRestResponse response;

            while (true)
            {
                client = new RestClient(url); // making connection
                request = new RestRequest("RequestClientList");  // set up api method request
                response = client.Get(request); // call api method

                List<Client> clientList = JsonConvert.DeserializeObject<Client>(response.Content);

            }
        }
    }
}
