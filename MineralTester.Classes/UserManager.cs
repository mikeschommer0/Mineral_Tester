using System;

/// <summary>
/// Coded by: Seth Frevert
/// </summary>
namespace MineralTester.Classes
{
    /// <summary>
    /// Class for handling user logic.
    /// </summary>
    public class UserManager : IUserManager
    {

        IDatabase database = new Database();

        /// <summary>
        /// Method to add a new user to the database.
        /// </summary>
        /// <param name="newUser"> User to add to database. </param>
        /// <returns> Returns the number of rows affected. </returns>
        public int AddUser(User newUser)
        {
            int result = 0;
            try
            {
                result = database.AddUser(newUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Method to update a user in the database.
        /// </summary>
        /// <param name="userToUpdate"> User to update in the database. </param>
        /// <returns> Returns the number of rows affected. </returns>
        public int UpdateUser(User userToUpdate)
        {
            userToUpdate.Salt = SecurityHelper.GenerateSalt();
            userToUpdate.Password = SecurityHelper.HashPassword(userToUpdate.Password, userToUpdate.Salt);
            int result = 0;
            try
            {
                result = database.UpdateUser(userToUpdate);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// Method to delete a user from the database.
        /// </summary>
        /// <param name="userToDelete"> User to delete form the database. </param>
        /// <returns> Returns the number of rows affected. </returns>
        public int DeleteUser(User userToDelete)
        {
            int result = 0;
            try
            {
                result = database.DeleteUser(userToDelete);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }

    }
}
