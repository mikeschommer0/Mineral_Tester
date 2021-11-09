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
using MineralTester.Classes;

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
            if (user.AccountType == 2)
            {
                btnCreateFaculty.IsEnabled = false;
                btnCreateStudent.IsEnabled = false;
            }
            else
            {
                btnCreateFaculty.IsEnabled = true;
                btnCreateStudent.IsEnabled = true;
            }
        }

        private User user;

        private void OpenCreateUserWindow(int createType)
        {
            if (user.AccountType == 3)
            {
                CreateUserWindow userWindow = new CreateUserWindow(user, createType);
                userWindow.Show();
            }
            else
            {
                MessageBox.Show("You do not have access to this feature.");
            }
        }

        private void CreateStudentButton(object sender, RoutedEventArgs e)
        {
            OpenCreateUserWindow(1);
        }

        private void CreateFacultyButton(object sender, RoutedEventArgs e)
        {

            OpenCreateUserWindow(2);
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

        private void PlaygroundButton(object sender, RoutedEventArgs e)
        {
            StudentMineralWindow studentMineral = new StudentMineralWindow(user);
            studentMineral.Show();
            Close();
        }
    }
}
