using APIClasses;
using Newtonsoft.Json;
using RestSharp;
using System.Windows;

namespace TransactionGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RestClient client;
        private RestRequest request;
        private IRestResponse response;
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// submits transaction with GUI entered details on click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void submitTransactionButton_Click(object sender, RoutedEventArgs e)
        {
            string url = "http://localhost:30327/"; // setting url for ASP.NET web api connection
            client = new RestClient(url); // making connection
            request = new RestRequest("PostTransaction/" + senderTextBox.Text + "/" + receiverTextBox.Text + "/" + transactionAmountTextBox.Text);  // set up api method request // add JSON object in body to be sent when api method is called, user token makes sure they are validated to use service
            // request.AddJsonBody(newBlock); // // add JSON object (serialized block object) in body to be sent when api method is called
            response = client.Get(request); // call api method
            try
            {
                bool transactionSuccess = JsonConvert.DeserializeObject<bool>(response.Content);
            }
            catch (JsonReaderException)
            {

            }

        }

        /// <summary>
        /// shows state of block chain on clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string url = "http://localhost:31551/"; // setting url for ASP.NET web api connection
            client = new RestClient(url); // making connection
            request = new RestRequest("Blockchain/State");  // set up api method request // add JSON object in body to be sent when api method is called, user token makes sure they are validated to use service
            response = client.Get(request); // call api method

            ChainState state;
            try
            {
                state = JsonConvert.DeserializeObject<ChainState>(response.Content);
                numBlocksTextBox.Text = state.numBlocks.ToString();
                totalMoneyTextBox.Text = state.totalAmount.ToString();

                int numAccounts = state.accountIDs.Count;

                accountsListBox.Items.Clear(); // gets rid of old found services in list before putting new ones in
                for (int i = 0; i < numAccounts; i++)
                {
                    if (i == 0)
                    {
                        accountsListBox.Items.Add("Account: " + state.accountIDs[i] + "  |  Balance: " + 0);
                    }
                    else
                    {
                        accountsListBox.Items.Add("Account: " + state.accountIDs[i] + "  |  Balance: " + state.accountBalances[i]);
                    }

                }
            }
            catch (System.NullReferenceException)
            {
                System.Diagnostics.Debug.WriteLine("ERROR: Failed to load state due to time out.");
                numBlocksTextBox.Text = 0.ToString();
                totalMoneyTextBox.Text = 0.ToString();
            }
            catch (JsonReaderException)
            {
                System.Diagnostics.Debug.WriteLine("ERROR: Failed to load state due to web loading issue.");
                numBlocksTextBox.Text = 0.ToString();
                totalMoneyTextBox.Text = 0.ToString();
            }





        }
    }
}
