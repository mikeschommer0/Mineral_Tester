using System;

namespace MineralTester.Classes
{
    /// <summary>
    /// Class for handling logins.
    /// Coded by: Seth Frevert.
    /// Salting by: Quinn Nimmer.
    /// </summary>
    public class LoginManager : ILoginManager
    {
        /// <summary>
        /// Method to attempt to allow a user to access the app.
        /// </summary>
        /// <param name="username"> User name to check if exists.</param>
        /// <param name="password"> Password to attempt to match.</param>
        /// <returns> The information associated with the user.</returns>
        public User Login(string username, string password)
        {
            Database db = new Database();
            if (db.CheckUserExists(username))
            {
                User user = db.GetUser(username);

                // Salt password.
                password = SecurityHelper.HashPassword(password, user.Salt);

                if (user.Password.Equals(password))
                {
                    return user;
                }
            }
            return null;
        }

        /// <summary>
        /// Method to logout the user, clears the user object of its information.
        /// </summary>
        /// <param name="user"> User to remove information from.</param>
        /// <returns> Returns the emtpy user.</returns>
        public User Logout(ref User user)
        {
            user = null;
            return user;
        }
    }
}
