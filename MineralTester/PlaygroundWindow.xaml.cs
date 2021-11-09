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
    /// Interaction logic for PlaygroundWindow.xaml
    /// </summary>
    public partial class PlaygroundWindow : Window
    {
        User _user;

        public PlaygroundWindow(User currentUser)
        {
            InitializeComponent();
            _user = currentUser;
        }

        private void DisplayPracticeQuestions(object sender, RoutedEventArgs e)
        {
            PracticeQuestionsWindow practiceQuestionsWindow = new PracticeQuestionsWindow();
            practiceQuestionsWindow.Show();
        }

        private void ExitPlayground(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
