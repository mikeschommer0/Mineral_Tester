using System.Windows;

/// <summary>
/// Written by Rick Bowman
/// XAML styling by Rick Bowman
/// </summary>
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

        /// <summary>
        /// Exits the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Opens the manage questions window when the user selects manage questions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ManageQuestionsClick(object sender, RoutedEventArgs e)
        {
            QuestionsWindow questionsWindow = new QuestionsWindow();
            questionsWindow.ShowDialog();
        }

        /// <summary>
        /// Opens the manage user window when the user selects manage users
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ManageUsersClick(object sender, RoutedEventArgs e)
        {
            UsersWindow usersWindow = new UsersWindow();
            usersWindow.ShowDialog();
        }

        /// <summary>
        /// Opens the manage minerals window when the user selects manage minerals
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ManageMineralsClick(object sender, RoutedEventArgs e)
        {
            MineralsWindow mineralsWindow = new MineralsWindow();
            mineralsWindow.ShowDialog();
        }

        /// <summary>
        /// Ensures everything is closed correctly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
