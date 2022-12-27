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
    class: LoginWindow.xaml.cs
    author: Kade McGarraghy
    purpose: Interaction logic for LoginWindow.xaml
    date last modified: 27/4/21
*/

namespace Client
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            string username;
            string password;

            var tcp = new NetTcpBinding(); // binds tcp interface
            var URL = "net.tcp://localhost/AuthenticationService"; // sets url to auth server endpoint

            var chanFactory = new ChannelFactory<AuthServerInterface>(tcp, URL);  // makes connection to auth server
            AuthServerInterface foob = chanFactory.CreateChannel(); 

            username = usernameTextBox.Text;
            password = passwordTextBox.Text;

            int token = foob.Login(username, password);

            string validateResult = foob.Validate(token);
            if (validateResult.Equals("Successfully validated"))
            {
                ServiceViewerWindow serviceViewerWindow = new ServiceViewerWindow();
                serviceViewerWindow.SetToken(token); // sets the token in the service viewer GUI class so that user can use GUI to access services
                this.Close();
                serviceViewerWindow.ShowDialog();
                
            }
            else
            {
                string messageBoxText = (validateResult + ". Check your username and password and try again.");
                string caption = "Login Error";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;

                MessageBox.Show(messageBoxText, caption, button, icon);
            }
           




        }
    }
}
