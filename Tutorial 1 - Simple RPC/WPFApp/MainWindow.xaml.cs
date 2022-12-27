using System;
using System.Collections.Generic;
using System.Linq;
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
using Server;

namespace WPFApp
{
    /// <summary>
    /// file name: MainWindow.xaml.cs
    /// author: Kade McGarraghy (reference: Tutorial 1 Worksheet)
    /// purpose: Interaction logic for MainWindow.xaml
    /// date last modified: 23/05/21
    /// </summary>

    public partial class MainWindow : Window
    {

        private DataServerInterface foob;

        public MainWindow()
        {
            InitializeComponent();

            ChannelFactory<DataServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            string URL = "net.tcp://localhost:8100/DataService";
            foobFactory = new ChannelFactory<DataServerInterface>(tcp, URL);
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
    }
}
