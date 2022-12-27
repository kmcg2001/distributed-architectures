using Authenticator;
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
using System.Windows.Shapes;

/*
    class: RegistrationWindow.xaml.cs
    author: Kade McGarraghy
    purpose:  Interaction logic for RegistrationWindow.xaml
    date last modified: 27/4/21
*/

namespace Client
{
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            string username;
            string password;

            var tcp = new NetTcpBinding();  // binds tcp interface
            var URL = "net.tcp://localhost/AuthenticationService"; // sets url to auth server endpoint

            var chanFactory = new ChannelFactory<AuthServerInterface>(tcp, URL);  // makes connection to auth server
            AuthServerInterface foob = chanFactory.CreateChannel();

            username = usernameTextBox.Text;
            password = passwordTextBox.Text;

            string registerResult = foob.Register(username, password);

            if (registerResult.Equals("Successfully registered"))
            {
                string outputStr = registerResult;
                string title = "Registration Success";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Information;

                MessageBox.Show(outputStr, title, button, icon);

                this.Close();
            }
            else
            {
                string outputStr = (registerResult + ". Make sure your username and password are not empty and try again.");
                string title = "Registration Error";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;

                MessageBox.Show(outputStr, title, button, icon);
            }
        }
    }
}
