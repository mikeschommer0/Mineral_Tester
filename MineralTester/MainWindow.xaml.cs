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
using MineralTester.Classes;

namespace MineralTester.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SignInButton(object sender, RoutedEventArgs e)
        {
            if(txtUsername.Text == string.Empty)
            {
                MessageBox.Show("Please enter a username.");
                return;
            }
            if (txtPassword.Text == string.Empty)
            {
                MessageBox.Show("Please enter a password.");
                return;
            }
            ILoginManager loginManager = new LoginManager();
            User user = loginManager.Login(txtUsername.Text, txtPassword.Text);
            if(user == null)
            {
                MessageBox.Show("Username or password was incorrect.");
                return;
            }
            if(user.AccountType == 3 || user.AccountType == 2)
            {
                AdminWindow adminWindow = new AdminWindow(user);
                adminWindow.Show();
                Close();
            }
            if(user.AccountType == 1)
            {
                StudentMineralWindow studentMineralWindow = new StudentMineralWindow(user);
                studentMineralWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("An unexpectated error has occured.");
            }
            
        }

        private void ForgotPasswordButton(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("A message will be sent to the teacher.");
        }
    }
}
