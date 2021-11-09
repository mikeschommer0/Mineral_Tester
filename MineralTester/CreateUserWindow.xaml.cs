using MineralTester.Classes;
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
    /// Interaction logic for CreateUserWindow.xaml
    /// </summary>
    public partial class CreateUserWindow : Window
    {
        IBusinessLogic bl = new BusinessLogic();
        public CreateUserWindow(User currentUser, int accountValue)
        {
            InitializeComponent();
            user = currentUser;
            newUserAccountType = accountValue;
        }
        private User user;
        private int newUserAccountType;
        private void SubmitUserInfo(object sender, RoutedEventArgs e)
        {
            List<String> fields = new List<String>();
            String firstName = FirstNameTextBox.Text;
            fields.Add(firstName);
            String lastName = LastNameTextBox.Text;
            fields.Add(lastName);
            String username = UsernameTextBox.Text;
            fields.Add(username);
            String password = PasswordTextBox.Text;
            fields.Add(password);
            List<bool> validFields = bl.ValidateUserData(fields);

            if (validFields.Contains(false)) // If any invaild fields, show message box for appropriate invalid field.
            {
                EntryErrors(validFields);
            }
            else
            {
                IUserManager userManager = new UserManager();
                User newUser = new User(0, firstName, lastName, username, password, newUserAccountType);
                
                if (newUserAccountType == 1)
                {
                    MessageBox.Show("A new student was added.");
                }
                else
                {
                    MessageBox.Show("A new user was added.");
                }
            }
        }

        private void EntryErrors(List<bool> validFields)
        {
            if (validFields[0] == false)
            {
                MessageBox.Show("Error while adding Mineral:\nInvalidFirstNameLength");
            }
            if (validFields[1] == false)
            {
                MessageBox.Show("Error while adding Mineral:\nInvalidLastNameLength");
            }
            if (validFields[2] == false)
            {
                MessageBox.Show("Error while adding Mineral:\nInvalidUsernameLength");
            }
            if (validFields[3] == false)
            {
                MessageBox.Show("Error while adding Mineral:\nInvalidPasswordLength");
            }

        }

        private void ExitUserInfo(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = new AdminWindow(user);
            Close();
        }
    }
}
