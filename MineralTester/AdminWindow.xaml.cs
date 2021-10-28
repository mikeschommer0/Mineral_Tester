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
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }

        private void CreateStudentButton(object sender, RoutedEventArgs e)
        {
            CreateUserWindow userWindow = new CreateUserWindow();
            userWindow.Show();
        }

        private void CreateFacultyButton(object sender, RoutedEventArgs e)
        {
            CreateUserWindow userWindow = new CreateUserWindow();
            userWindow.Show();
        }

        private void CreateQuestionButton(object sender, RoutedEventArgs e)
        {
            CreateQuestionWindow createQuestion = new CreateQuestionWindow();
            createQuestion.Show();
        }

        private void AddMineralButton(object sender, RoutedEventArgs e)
        {
            AddMineralWindow addMineral = new AddMineralWindow();
            addMineral.Show();
        }

        private void ExitAdminButton(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
