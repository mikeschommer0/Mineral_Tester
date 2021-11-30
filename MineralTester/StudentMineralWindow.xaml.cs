using MineralTester.Classes;
using System.Windows;

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
            if (user.AccountType == Enums.AccountType.Student)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
