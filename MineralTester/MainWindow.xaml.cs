using MineralTester.Classes;
using System.Windows;

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
            if (txtUsername.Text == string.Empty)
            {
                MessageBox.Show("Please enter a username.");
                return;
            }
            if (txtPassword.Password == string.Empty)
            {
                MessageBox.Show("Please enter a password.");
                return;
            }
            ILoginManager loginManager = new LoginManager();
            User user = loginManager.Login(txtUsername.Text, txtPassword.Password);
            if (user == null)
            {
                MessageBox.Show("Username or password was incorrect.");
                return;
            }
            if (user.AccountType == Enums.AccountType.Teacher)
            {
                AdminWindow adminWindow = new AdminWindow(user);
                adminWindow.Show();
                Close();
            }
            else if (user.AccountType == Enums.AccountType.Assistant)
            {
                QuestionsWindow questionsWindow = new QuestionsWindow();
                questionsWindow.Show();
                Close();
            }
            else if (user.AccountType == Enums.AccountType.Student)
            {
                PlaygroundWindow playgroundWindow = new PlaygroundWindow(user);
                playgroundWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("An unexpected error has occurred.");
            }

        }

        private void ForgotPasswordButton(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("A message will be sent to the teacher.");
        }
    }
}
