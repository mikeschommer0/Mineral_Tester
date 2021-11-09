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
    /// Interaction logic for StudentMineral.xaml
    /// </summary>
    public partial class StudentMineralWindow : Window
    {
        public StudentMineralWindow(User currentUser)
        {
            InitializeComponent();
            user = currentUser;
        }
        private User user;
        private void GoStudyButton(object sender, RoutedEventArgs e)
        {
            PlaygroundWindow playground = new PlaygroundWindow(user);
            playground.Show();
            Close();
        }

        private void ExitStudentMineral(object sender, RoutedEventArgs e)
        {
            if(user.AccountType == Enums.AccountType.Student)
            {
                Close();
            }
            if (user.AccountType == Enums.AccountType.Assistant || user.AccountType == Enums.AccountType.Teacher)
            {
                AdminWindow adminWindow = new AdminWindow(user);
                adminWindow.Show();
                Close();
            }
        }
    }
}
