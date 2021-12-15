using MineralTester.Classes;
using System.Windows;

/// <summary>
/// Written by Quinn Nimmer
/// </summary>
namespace MineralTester.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// XAML styling by Rick bowman
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Ensures user inputs correct account information and handles loggin them in
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                FacultyWindow facultyWindow = new FacultyWindow();
                Close();
                facultyWindow.ShowDialog();
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
    }
}
