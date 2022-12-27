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
using System.Windows.Navigation;
using System.Windows.Shapes;


/*
    class: MainWindow.xaml.cs
    author: Kade McGarraghy
    purpose:  Interaction logic for MainWindow.xaml
    date last modified: 27/4/21
*/

namespace Client
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
        }

        private void registerButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registerWindow = new RegistrationWindow();
            registerWindow.ShowDialog();
        }
    }
}
