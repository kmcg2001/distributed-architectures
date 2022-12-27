using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.Text;
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
using APIClasses;
using Newtonsoft.Json;
using RestSharp;

namespace WPFApp
{
    /// <summary>
    /// file name: MainWindow.xaml.cs
    /// author: Kade McGarraghy (reference: Tutorial 2 Worksheet)
    /// purpose: Interaction logic for MainWindow.xaml
    /// date last modified: 23/05/21
    /// </summary>

    public partial class MainWindow : Window
    {
        private delegate DataIntermed SearchOperation(string searchValue);
        private RestClient client;

        public MainWindow()
        {
            InitializeComponent();

            string URL = "http://localhost:12266/";
            client = new RestClient(URL);
            RestRequest request = new RestRequest("api/values");

            IRestResponse numOfThings = client.Get(request);
            totalLabel.Content = "Total items: " + numOfThings.Content;

            progressBar.Visibility = Visibility.Hidden;

        }


        /// <summary>
        /// when clicked, user will get values for data object at index provided
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
                     
            int index = 0;
            string fName = "", lName = "";
            int bal = 0;
            uint acct = 0, pin = 0;
            if (Int32.TryParse(indexTextBox.Text, out int res)) // makes sure number entered is integer
            {
                index = Int32.Parse(indexTextBox.Text);
                if (index >= 0) // make sure the index entered is a pos number
                {

                    RestRequest request = new RestRequest("api/getvalues/" + index.ToString());
                    IRestResponse resp = client.Get(request);

                    DataIntermed dataIntermed = JsonConvert.DeserializeObject<DataIntermed>(resp.Content);

                    firstNameText.Text = dataIntermed.fname; // setting GUI values
                    lastNameText.Text = dataIntermed.lname;
                    balanceText.Text = dataIntermed.bal.ToString("C");
                    acctNoText.Text = dataIntermed.acct.ToString();
                    pinText.Text = dataIntermed.pin.ToString("D4");

                    System.Diagnostics.Debug.WriteLine("Success setting GUI values to data for index " + index);

                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Error: Index '" + indexTextBox.Text + "' was negative");
                    indexTextBox.Text = "Index cannot be negative";


                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Error: Index '" + indexTextBox.Text + "' was not an integer");
                indexTextBox.Text = "Index should be a number";

            }

        }

        /// <summary>
        /// search by last name will begin on button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            progressBar.Visibility = Visibility.Visible;
            
            SearchOperation searchOp;
            AsyncCallback callback;
            string searchValue = searchText.Text;

            searchText.IsReadOnly = true;
            indexTextBox.IsReadOnly = true;
            searchButton.IsEnabled = false;
            goButton.IsEnabled = false;
            progressBar.IsIndeterminate = true;

            searchOp = new SearchOperation(this.Search);
            callback = this.OnSearchCompletion;

            IAsyncResult result = searchOp.BeginInvoke(searchValue, callback, null);

        }

        /// <summary>
        /// calls the RPC which performs search by lastname
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        private DataIntermed Search(string searchValue)
        {
            SearchData mySearch = new SearchData();
            mySearch.searchStr = searchValue;
            
            RestRequest request = new RestRequest("api/search/");
            request.AddJsonBody(mySearch);
            IRestResponse resp = client.Post(request);
            DataIntermed dataIntermed = JsonConvert.DeserializeObject<DataIntermed>(resp.Content); // setting GUI values


            return dataIntermed;
        }

        /// <summary>
        /// once search result is returned by delegate, clean up
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        private void OnSearchCompletion(IAsyncResult asyncResult)
        {
            SearchOperation searchOp;
            AsyncResult asyncObj = (AsyncResult)asyncResult;

            if (asyncObj.EndInvokeCalled == false)
            {
                searchOp = (SearchOperation)asyncObj.AsyncDelegate;
                DataIntermed dataIntermed = searchOp.EndInvoke(asyncObj);
                this.Dispatcher.Invoke(() =>
                {
                    if ((dataIntermed.acct.ToString() != "0") || dataIntermed == null)
                    {
                        firstNameText.Text = dataIntermed.fname; // setting GUI values
                        lastNameText.Text = dataIntermed.lname;
                        balanceText.Text = dataIntermed.bal.ToString("C");
                        acctNoText.Text = dataIntermed.acct.ToString();
                        pinText.Text = dataIntermed.pin.ToString("D4");

                        searchText.IsReadOnly = false;
                        indexTextBox.IsReadOnly = false;
                        searchButton.IsEnabled = true;
                        goButton.IsEnabled = true;
                        progressBar.IsIndeterminate = false;
                        progressBar.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Error finding account details");

                        searchText.IsReadOnly = false;
                        indexTextBox.IsReadOnly = false;
                        searchButton.IsEnabled = true;
                        goButton.IsEnabled = true;
                        progressBar.IsIndeterminate = false;
                        progressBar.Visibility = Visibility.Hidden;
                    }

                });
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    searchText.IsReadOnly = false;
                    indexTextBox.IsReadOnly = false;
                    searchButton.IsEnabled = true;
                    goButton.IsEnabled = true;
                    progressBar.IsIndeterminate = false;
                    progressBar.Visibility = Visibility.Hidden;
                });

            }

            asyncObj.AsyncWaitHandle.Close();
        }

    }
}
