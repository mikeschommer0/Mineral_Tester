using MineralTester.Classes;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// Written by Seth Frevert
/// XAML styling by Rick Bowman
/// </summary>
namespace MineralTester.UI
{

    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UsersWindow : Window
    {
        IDatabase database = new Database();

        public UsersWindow()
        {
            InitializeComponent();
            DisplayUsers();
        }

        /// <summary>
        /// Displays the users
        /// </summary>
        private void DisplayUsers()
        {
            List<User> users = database.GetAllUsers();
            dgUsers.ItemsSource = null;
            dgUsers.ItemsSource = users;
        }

        /// <summary>
        /// Formats data grid columns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Handles a user selecting add user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddUserClick(object sender, RoutedEventArgs e)
        {
            ManageUserWindow createUserWindow = new ManageUserWindow();
            createUserWindow.ShowDialog();
            DisplayUsers();
        }

        /// <summary>
        /// Handles a user selecting to update a user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateUserClick(object sender, RoutedEventArgs e)
        {
            User userToUpdate = dgUsers.SelectedItem as User;
            if (userToUpdate == null)
            {
                MessageBox.Show("Select a user to update.");
                return;
            }
            ManageUserWindow createUserWindow = new ManageUserWindow(userToUpdate);
            createUserWindow.ShowDialog();
            DisplayUsers();
        }

        /// <summary>
        /// Handles a user selecting delete a user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Exits the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
