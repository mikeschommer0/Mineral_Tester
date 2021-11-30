using MineralTester.Classes;
using System;
using System.Collections.Generic;
using System.Windows;

namespace MineralTester.UI
{
    /// <summary>
    /// Interaction logic for CreateUserWindow.xaml
    /// </summary>
    public partial class CreateUserWindow : Window
    {
        IBusinessLogic bl = new BusinessLogic();
        public CreateUserWindow(Enums.AccountType accountValue, Boolean isCreatingAUser = true, User userToUpdate = null)
        {
            InitializeComponent();
            newUserAccountType = accountValue;
            aUserIsBeingCreated = isCreatingAUser;

            if (isCreatingAUser)
            {
                if (newUserAccountType == Enums.AccountType.Assistant)
                {
                    btnSubmit.Content = "Submit New Assistant";
                } 
                else if(newUserAccountType == Enums.AccountType.Student)
                {
                    btnSubmit.Content = "Submit New Student";
                }
            }
            else
            {
                aUserIsBeingCreated = isCreatingAUser;
                btnSubmit.Content = "Submit Update";
                userBeingUpdated = userToUpdate;
                FirstNameTextBox.Text = userBeingUpdated.FirstName;
                LastNameTextBox.Text = userBeingUpdated.LastName;
                UsernameTextBox.Text = userBeingUpdated.Username;
                PasswordTextBox.Text = userBeingUpdated.Password;
            }
        }

        private Boolean aUserIsBeingCreated = true;
        private Enums.AccountType newUserAccountType;
        private User userBeingUpdated = null;
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

            if (validFields.Contains(false) && userBeingUpdated == null) // If any invaild fields, show message box for appropriate invalid field.
            {
                EntryErrors(validFields);
            }
            else
            {
                IUserManager userManager = new UserManager();
                if (aUserIsBeingCreated)
                {
                    User newUser = new User(0, firstName, lastName, username, password, newUserAccountType);
                    userManager.AddUser(newUser);

                    if (newUserAccountType == Enums.AccountType.Student)
                    {
                        MessageBox.Show("A new student was added.");
                    }
                    else
                    {
                        MessageBox.Show("A new assistant was added.");
                    }
                }
                else
                {
                    userBeingUpdated.FirstName = firstName;
                    userBeingUpdated.LastName = lastName;
                    userBeingUpdated.Username = username;
                    userBeingUpdated.Password = password;
                    int result = userManager.UpdateUser(userBeingUpdated);
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
            //AdminWindow adminWindow = new AdminWindow(user);
            Close();
        }
    }
}
