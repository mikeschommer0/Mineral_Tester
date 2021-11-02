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
        public PlaygroundWindow(User currentUser)
        {
            InitializeComponent();
            user = currentUser;
        }
        private User user;
        private void ExitPlayground(object sender, RoutedEventArgs e)
        {
            StudentMineralWindow studentMineralWindow = new StudentMineralWindow(user);
            studentMineralWindow.Show();
            Close();
        }
    }
}
