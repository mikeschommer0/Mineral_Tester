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
using System.Windows.Shapes;

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
