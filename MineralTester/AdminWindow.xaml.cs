using MineralTester.Classes;
using System.Windows;

namespace MineralTester.UI
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow(User currentUser)
        {
            InitializeComponent();
            user = currentUser;
        }

        private User user;

        private void OpenCreateUserWindow(Enums.AccountType createType)
        {
            if (user.AccountType == Enums.AccountType.Teacher)
            {
                CreateUserWindow userWindow = new CreateUserWindow(createType);
                userWindow.Show();
            }
            else
            {
                MessageBox.Show("You do not have access to this feature.");
            }
        }

        private void CreateStudentButton(object sender, RoutedEventArgs e)
        {
            OpenCreateUserWindow(Enums.AccountType.Student);
        }

        private void CreateFacultyButton(object sender, RoutedEventArgs e)
        {

            OpenCreateUserWindow(Enums.AccountType.Assistant);
        }

        private void CreateQuestionButton(object sender, RoutedEventArgs e)
        {
            QuestionsWindow questionsWindow = new QuestionsWindow();
            questionsWindow.Show();
        }

        private void AddMineralButton(object sender, RoutedEventArgs e)
        {
            AddMineralWindow addMineral = new AddMineralWindow();
            addMineral.Show();
        }

        private void EditOrDeleteMineral(object sender, RoutedEventArgs e)
        {
            EditOrDeleteMineral editOrDeleteMinera = new EditOrDeleteMineral();
            editOrDeleteMinera.Show();
        }

        private void ExitAdminButton(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PlaygroundButton(object sender, RoutedEventArgs e)
        {
            StudentMineralWindow studentMineral = new StudentMineralWindow(user);
            studentMineral.Show();
            Close();
        }

        private void ViewAllUsers(object sender, RoutedEventArgs e)
        {
            ViewUsersWindow viewUsersWindow = new ViewUsersWindow();
            viewUsersWindow.Show();
        }
    }
}
