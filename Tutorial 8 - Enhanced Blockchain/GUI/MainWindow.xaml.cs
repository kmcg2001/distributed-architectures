using APIClasses;
using Lib;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
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
using Block = APIClasses.Block;

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

        public MainWindow()
        {
            InitializeComponent();

            PortList portList = new PortList();
            thisPort = portList.GetNewPort();
            Debug.WriteLine("port :" + thisPort);
            thisIp = "localhost";

            ThreadStart opsDelegate = new ThreadStart(RunMiningThread);
            Thread thread1 = new Thread(opsDelegate);
            thread1.Start();

            ThreadStart blockchainDelegate = new ThreadStart(RunBlockchainThread);
            Thread thread2 = new Thread(blockchainDelegate);
            thread2.Start();

        }

        /// <summary>
        /// thread for clients to mine 
        /// </summary>
        public void RunMiningThread()
        {
            string URL = "net.tcp://" + thisIp + ":" + thisPort + "/BlockchainService"; // sets url to blockchain service endpoint
            var tcp = new NetTcpBinding();  // binds tcp interface
            ChannelFactory<BlockchainServiceInterface> chanFactory;
            BlockchainServiceInterface foob;

            Queue<Transaction> transactions;

            while (true)
            {
                transactions = TransactionsList.transactions;
                
                while (transactions.Count > 0)
                {
                    bool transactionDone = false;
                    Transaction transaction = transactions.Dequeue();

                    while (!transactionDone)
                    {
                        chanFactory = new ChannelFactory<BlockchainServiceInterface>(tcp, URL);  // makes connection to auth server
                        foob = chanFactory.CreateChannel();


                        /*string url = "http://localhost:18012/";
                        RestClient client = new RestClient(url);
                        RestRequest request = new RestRequest("Blockchain/Balance/" + transaction.fromWalletID);  // set up api method request // add JSON object in body to be sent when api method is called, user token makes sure they are validated to use service
                        IRestResponse response = client.Get(request); // call api method
                        List<Block> blockchain = JsonConvert.DeserializeObject<List<Block>>(response.Content);*/

                        if ((foob.GetAccountBalance(transaction.fromWalletID) >= transaction.amount) || transaction.fromWalletID == 0)
                        {
                            Block newBlock = new Block();


                            List<Block> currBlockchain = foob.GetCurrentBlockchain();

                            newBlock.id = (uint)currBlockchain.Count;
                            newBlock.fromWalletID = transaction.fromWalletID;
                            newBlock.toWalletID = transaction.toWalletID;
                            newBlock.amount = transaction.amount;
                            newBlock.prevBlockHash = currBlockchain[currBlockchain.Count - 1].currBlockHash;

                            SHA256 sha256 = SHA256.Create();
                            uint offset = 1;
                            string hash;

                            do
                            {
                                offset++;

                                string transactionData = newBlock.id.ToString() + newBlock.fromWalletID.ToString() + newBlock.toWalletID.ToString() + newBlock.amount.ToString() + offset.ToString();

                                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(transactionData));
                                hash = BitConverter.ToUInt64(hashBytes, 0).ToString();
                            }
                            while ((!hash.StartsWith("12345")) || (offset % 5 != 0));

                            newBlock.currBlockHash = hash;
                            newBlock.blockOffset = offset;

                            if (Blockchain.AddBlock(newBlock))
                            {
                                transactionDone = true;
                            }
                        }
                        else
                        {
                            transactionDone = true;
                        }

                    }

                }
            }
        }

        /// <summary>
        /// client submits transaction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void submitTransactionButton_Click(object sender, RoutedEventArgs e)
        {
            string url = "http://localhost:18012/"; // setting url for ASP.NET web api connection
            RestClient restClient = new RestClient(url); // making connection
            RestRequest request = new RestRequest("RequestClientList");  // set up api method request
            IRestResponse response = restClient.Get(request); // call api method
            List<Client> clientList = JsonConvert.DeserializeObject<List<Client>>(response.Content);

            var tcp = new NetTcpBinding();  // binds tcp interface
            var URL = "";
            ChannelFactory<BlockchainServiceInterface> chanFactory;
            BlockchainServiceInterface foob;

            foreach (Client client in clientList)
            {
                URL = "net.tcp://" + client.ip + ":" + client.port + "/BlockchainService";  // sets url to auth server endpoint
                chanFactory = new ChannelFactory<BlockchainServiceInterface>(tcp, URL);  // makes connection to auth server
                foob = chanFactory.CreateChannel();

                Transaction newTransaction = new Transaction();

                string amountString = transactionAmountTextBox.Text;
                if (float.TryParse(amountString, out float floatRes)) // makes sure number entered is integer
                {
                    float amount = Int32.Parse(amountString);
                    if (amount > 0) // makes sure number entered is a positive number > 0 (it's a calculator, needs at least 1 operand)
                    {
                        newTransaction.amount = float.Parse(amountString);

                        String fromWalletIDString = senderTextBox.Text;
                        if (uint.TryParse(fromWalletIDString, out uint uIntRes)) // makes sure number entered is integer
                        {
                            uint fromWallet = UInt32.Parse(fromWalletIDString);
                            {
                                newTransaction.fromWalletID = uint.Parse(fromWalletIDString);
                            }
                        }
                        else
                        {
                            newTransaction.fromWalletID = 0;
                        }

                        String toWalletIDString = receiverTextBox.Text;
                        if (uint.TryParse(toWalletIDString, out uIntRes)) // makes sure number entered is integer
                        {
                            uint toWallet = UInt32.Parse(toWalletIDString);
                            {
                                newTransaction.toWalletID = uint.Parse(toWalletIDString);
                            }
       
                        }
                        else
                        {
                            newTransaction.toWalletID = 0;
                        }

                        if (newTransaction.toWalletID == 0 && newTransaction.fromWalletID == 0)
                        {
                            System.Diagnostics.Debug.WriteLine("Invalid transaction provided.");
                            
                        }
                        else
                        {
                            foob.ReceiveNewTransaction(newTransaction);
                        }

                    }
                }
            }
        }

        /// runs thread that starts blockchain service
        public void RunBlockchainThread()
        {

            Console.WriteLine("Starting Blockchain Service");
            var tcp = new NetTcpBinding(); // binds tcp interface
            var host = new ServiceHost(typeof(BlockchainService));

            host.AddServiceEndpoint(typeof(BlockchainServiceInterface), tcp, "net.tcp://" + thisIp + ":" + thisPort + "/BlockchainService"); // other layers will connect to this endpoint to use this service
            host.Open();

            string url = "http://localhost:18012/"; // setting url for ASP.NET web api connection
            RestClient restClient = new RestClient(url); // making connection

            RestRequest request = new RestRequest("Register/" + thisIp + "/" + thisPort);  // set up api method request
            IRestResponse response = restClient.Get(request); // call api method
            thisClientID = JsonConvert.DeserializeObject<uint>(response.Content);

            Console.WriteLine("");
            Console.ReadLine(); // keeps the server open
            host.Close();  // closes the host 

        }

        /// <summary>
        /// shows client state of blockchain
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ChainState state = Blockchain.GetState();

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
    }
  
}
