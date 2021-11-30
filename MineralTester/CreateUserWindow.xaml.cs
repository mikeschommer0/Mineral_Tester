using MineralTester.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MineralTester.UI
{
    /// <summary>
    /// Interaction logic for CreateUserWindow.xaml
    /// </summary>
    public partial class CreateUserWindow : Window
    {
        IBusinessLogic bl = new BusinessLogic();
        User _userToUpdate = null;
        public CreateUserWindow(User userToUpdate = null)
        {
            InitializeComponent();
            cbAccountType.ItemsSource = Enum.GetValues(typeof(Enums.AccountType)).Cast<Enums.AccountType>();
            if (userToUpdate != null)
            {
                _userToUpdate = userToUpdate;
                FirstNameTextBox.Text = _userToUpdate.FirstName;
                LastNameTextBox.Text = _userToUpdate.LastName;
                UsernameTextBox.Text = _userToUpdate.Username;
                cbAccountType.SelectedItem = _userToUpdate.AccountType;
            }
        }

        private void SubmitUserInfo(object sender, RoutedEventArgs e)
        {
            List<string> fields = new List<string>();
            string firstName = FirstNameTextBox.Text;
            fields.Add(firstName);
            string lastName = LastNameTextBox.Text;
            fields.Add(lastName);
            string username = UsernameTextBox.Text;
            fields.Add(username);
            string password = PasswordTextBox.Password;
            fields.Add(password);
            List<bool> validFields = bl.ValidateUserData(fields);

            if (validFields.Contains(false) && _userToUpdate == null) // If any invalid fields, show message box for appropriate invalid field.
            {
                EntryErrors(validFields);
            }
            else
            {
                IUserManager userManager = new UserManager();
                if (_userToUpdate == null)
                {
                    Enums.AccountType accountType = (Enums.AccountType)cbAccountType.SelectedItem;
                    User newUser = new User(0, firstName, lastName, username, password, accountType);
                    userManager.AddUser(newUser);
                    MessageBox.Show("A new user was added.");
                }
                else
                {
                    _userToUpdate.FirstName = firstName;
                    _userToUpdate.LastName = lastName;
                    _userToUpdate.Username = username;
                    _userToUpdate.Password = password;
                    _userToUpdate.AccountType = (Enums.AccountType)cbAccountType.SelectedItem;
                    int result = userManager.UpdateUser(_userToUpdate);
                    if(result == 1)
                    {
                        MessageBox.Show("The update was successful");
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Something went wrong.");
                    }
                    
                }
            }
        }

        private void EntryErrors(List<bool> validFields)
        {
            if (validFields[0] == false)
            {
                MessageBox.Show("Error while adding user:\nInvalidFirstNameLength");
            }
            if (validFields[1] == false)
            {
                MessageBox.Show("Error while adding user:\nInvalidLastNameLength");
            }
            if (validFields[2] == false)
            {
                MessageBox.Show("Error while adding user:\nInvalidUsernameLength");
            }
            if (validFields[3] == false)
            {
                MessageBox.Show("Error while adding user:\nInvalidPasswordLength");
            }
            if (validFields[4] == false)
            {
                MessageBox.Show("Error while adding user:\nUsernameAlreadyTaken");
            }

        }

        private void ExitUserInfo(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
