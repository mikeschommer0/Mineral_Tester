using System.Windows;

namespace MineralTester.UI
{
    /// <summary>
    /// Interaction logic for FacultyWindow.xaml
    /// </summary>
    public partial class FacultyWindow : Window
    {
        public FacultyWindow()
        {
            InitializeComponent();
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ManageQuestionsClick(object sender, RoutedEventArgs e)
        {
            QuestionsWindow questionsWindow = new QuestionsWindow();
            questionsWindow.Show();
        }

        private void ManageUsersClick(object sender, RoutedEventArgs e)
        {
            UsersWindow usersWindow = new UsersWindow();
            usersWindow.Show();
        }

        private void ManageMineralsClick(object sender, RoutedEventArgs e)
        {
            MineralsWindow mineralsWindow = new MineralsWindow();
            mineralsWindow.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
