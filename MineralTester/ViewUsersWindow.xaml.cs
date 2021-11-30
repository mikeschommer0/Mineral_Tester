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
    /// Interaction logic for ViewUsersWindow.xaml
    /// </summary>
    public partial class ViewUsersWindow : Window
    {
        IDatabase database = new Database();

        public ViewUsersWindow()
        {
            InitializeComponent();
            DisplayUsers();
        }

        private void DisplayUsers()
        {
            List<User> users = database.GetAllUsers();
            lvUsers.ItemsSource = null;
            lvUsers.ItemsSource = users;
            lvUsers.DisplayMemberPath = "FullName";
        }

        private void UpdateUser(object sender, RoutedEventArgs e)
        {
            User userToUpdate = lvUsers.SelectedItem as User;
            if(userToUpdate == null)
            {
                MessageBox.Show("Select a user to update.");
                return;
            }
            CreateUserWindow createUserWindow = new CreateUserWindow(Enums.AccountType.Teacher, false, userToUpdate);
            createUserWindow.ShowDialog();
            DisplayUsers();
        }

        private void DeleteUser(object sender, RoutedEventArgs e)
        {
            User userToDelete = lvUsers.SelectedItem as User;
            if (userToDelete == null)
            {
                MessageBox.Show("Select a user to delete.");
                return;
            }
            if(userToDelete.AccountType == Enums.AccountType.Teacher)
            {
                MessageBox.Show("A teacher can not be deleted.");
                return;
            }
            IUserManager userManager = new UserManager();
            userManager.DeleteUser(userToDelete);
            DisplayUsers();
        }
    }
}
