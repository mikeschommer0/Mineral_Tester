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
            cbAccountType.SelectedIndex = 2;
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
            FeedBack.Text = "";
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

            if (validFields.Contains(false)) // If any invalid fields, show message box for appropriate invalid field.
            {
                FeedBack.Text = EntryErrors(validFields);
            }
            else if(cbAccountType.SelectedItem == null)
            {
                MessageBox.Show("Please select a account type and try again.");
            }
            else
            {
                IUserManager userManager = new UserManager();
                if (_userToUpdate == null)
                {
                    Enums.AccountType accountType = (Enums.AccountType)cbAccountType.SelectedItem;
                    User newUser = new User(0, firstName, lastName, username, password, accountType);
                    userManager.AddUser(newUser);
                    FeedBack.Text = "A new user was added.";
                    ExitUserInfo(sender, e);
                }
                else
                {
                    string passwordUpdatedText = "\n (password was not changed)";
                    _userToUpdate.FirstName = firstName;
                    _userToUpdate.LastName = lastName;
                    _userToUpdate.Username = username;
                    if (!string.IsNullOrWhiteSpace(password))
                    {
                        _userToUpdate.Password = password;
                        passwordUpdatedText = "\n (password was updated)";
                    }
                    _userToUpdate.AccountType = (Enums.AccountType)cbAccountType.SelectedItem;
                    int result = userManager.UpdateUser(_userToUpdate);
                    if(result == 1)
                    {
                        FeedBack.Text = "The update was successful" + passwordUpdatedText;
                        ExitUserInfo(sender, e);
                    }
                    else
                    {
                        FeedBack.Text = "Something went wrong.";
                    }
                    
                }
            }
        }

        private string EntryErrors(List<bool> validFields)
        {
            string errors = "Error(s) while updating User:";

            if (validFields[0] == false)
            {
                errors += "\nInvalidFirstNameLength";
            }
            if (validFields[1] == false)
            {
                errors += "\nInvalidLastNameLength";
            }
            if (validFields[2] == false)
            {
                errors += "\nInvalidUsernameLength";
            }
            if (validFields[3] == false)
            {
                errors += "\nInvalidPasswordLength";
            }
            if (validFields[4] == false)
            {
                errors += "\nUsernameAlreadyTaken";
            }
            return errors;
        }

        private void ExitUserInfo(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
