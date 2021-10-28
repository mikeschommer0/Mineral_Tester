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
    /// Interaction logic for StudentMineral.xaml
    /// </summary>
    public partial class StudentMineralWindow : Window
    {
        public StudentMineralWindow()
        {
            InitializeComponent();
        }

        private void GoStudyButton(object sender, RoutedEventArgs e)
        {
            PlaygroundWindow playground = new PlaygroundWindow();
            playground.Show();
            Close();
        }

        private void ExitStudentMineral(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = new AdminWindow();
            adminWindow.Show();
            Close();
        }
    }
}
