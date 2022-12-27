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
using BusinessTier;
using Server;

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
        private delegate int SearchOperation(string searchValue);
        private BusinessServerInterface foob;

        public MainWindow()
        {
            InitializeComponent();

            ChannelFactory<BusinessServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            string URL = "net.tcp://localhost:8101/BusinessService";
            foobFactory = new ChannelFactory<BusinessServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();

            totalLabel.Content = "Total items: " + foob.GetNumEntries().ToString();

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
                    foob.GetValuesForEntry(index, out acct, out pin, out bal, out fName, out lName); // remote procedure call

                    firstNameText.Text = fName; // setting GUI values
                    lastNameText.Text = lName;
                    balanceText.Text = bal.ToString("C");
                    acctNoText.Text = acct.ToString();
                    pinText.Text = pin.ToString("D4");

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
            SearchOperation searchOp;
            AsyncCallback callback;
            string searchValue = searchText.Text;

            searchText.IsReadOnly = true; // makes additional searching disabled while search being performed
            indexTextBox.IsReadOnly = true; 
            searchButton.IsEnabled = false;
            goButton.IsEnabled = false;
            progressBar.IsIndeterminate = true; // makes progress bar look like its loading data


            searchOp = new SearchOperation(this.Search);
            callback = this.OnSearchCompletion;

            IAsyncResult result = searchOp.BeginInvoke(searchValue, callback, null);


        }

        /// <summary>
        /// calls the RPC which performs search by lastname
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        private int Search(string searchValue)
        {
            int index = foob.SearchByLastname(searchValue);
            return index;
        }

        /// <summary>
        /// once search result is returned by delegate, clean up
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        private void OnSearchCompletion(IAsyncResult asyncResult)
        {
            int index;
            SearchOperation searchOp;
            AsyncResult asyncObj = (AsyncResult)asyncResult;

            if (asyncObj.EndInvokeCalled == false)
            {
                searchOp = (SearchOperation)asyncObj.AsyncDelegate;
                index = searchOp.EndInvoke(asyncObj);

                this.Dispatcher.Invoke(() =>
                {
                    if (index != -1)
                    {
                        foob.GetValuesForEntry(index, out var acct, out var pin, out var bal, out var fName, out var lName);
                        firstNameText.Text = fName; // setting GUI values
                        lastNameText.Text = lName;
                        balanceText.Text = bal.ToString("C");
                        acctNoText.Text = acct.ToString();
                        pinText.Text = pin.ToString("D4");

                        searchText.IsReadOnly = false;
                        indexTextBox.IsReadOnly = false;
                        searchButton.IsEnabled = true;
                        goButton.IsEnabled = true;
                        progressBar.IsIndeterminate = false;
                    }
       
                });


            }
            else
            {
                searchText.IsReadOnly = false;
                indexTextBox.IsReadOnly = false;
                searchButton.IsEnabled = true;
                goButton.IsEnabled = true;
                progressBar.IsIndeterminate = false;
            }
         
            asyncObj.AsyncWaitHandle.Close();
        }

        /// <summary>
        /// sets gui elements to returned data
        /// </summary>
        /// <param name="index"></param>
        private void LoadData(int index)
        {
            foob.GetValuesForEntry(index, out var acct, out var pin, out var bal, out var fName, out var lName);
            firstNameText.Text = fName; // setting GUI values
            lastNameText.Text = lName;
            balanceText.Text = bal.ToString("C");
            acctNoText.Text = acct.ToString();
            pinText.Text = pin.ToString("D4");
            indexTextBox.Text = index.ToString();

        }


    }
}
