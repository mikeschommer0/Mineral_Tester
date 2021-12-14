using MineralTester.Classes;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MineralTester.UI
{

    /// <summary>
    /// Interaction logic for UsersWindow.xaml
    /// </summary>
    public partial class UsersWindow : Window
    {
        IDatabase database = new Database();

        public UsersWindow()
        {
            InitializeComponent();
            DisplayUsers();
        }

        private void DisplayUsers()
        {
            List<User> users = database.GetAllUsers();
            dgUsers.ItemsSource = null;
            dgUsers.ItemsSource = users;
        }

        private void dgUsers_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "ID" || e.Column.Header.ToString() == "FullName" || e.Column.Header.ToString() == "Password")
            {
                e.Column.Visibility = Visibility.Hidden;
            }
            if (e.Column.Header.ToString() == "FirstName")
            {
                e.Column.Header = "First Name";
            }
            if (e.Column.Header.ToString() == "LastName")
            {
                e.Column.Header = "Last Name";
            }
            if (e.Column.Header.ToString() == "AccountType")
            {
                e.Column.Header = "Account Type";
            }
            if (e.Column.Header.ToString() == "Salt")
            {
                e.Column.Visibility = Visibility.Hidden;
            }
            e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }

        private void AddUserClick(object sender, RoutedEventArgs e)
        {
            CreateUserWindow createUserWindow = new CreateUserWindow();
            createUserWindow.ShowDialog();
            DisplayUsers();
        }

        private void UpdateUserClick(object sender, RoutedEventArgs e)
        {
            User userToUpdate = dgUsers.SelectedItem as User;
            if (userToUpdate == null)
            {
                MessageBox.Show("Select a user to update.");
                return;
            }
            CreateUserWindow createUserWindow = new CreateUserWindow(userToUpdate);
            createUserWindow.ShowDialog();
            DisplayUsers();
        }

        private void DeleteUserClick(object sender, RoutedEventArgs e)
        {
            User userToDelete = dgUsers.SelectedItem as User;
            if (userToDelete == null)
            {
                MessageBox.Show("Select a user to delete.");
                return;
            }
            IUserManager userManager = new UserManager();
            userManager.DeleteUser(userToDelete);
            DisplayUsers();
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
