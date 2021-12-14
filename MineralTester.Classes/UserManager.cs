using System;

namespace MineralTester.Classes
{
    /// <summary>
    /// Class for handling user logic.
    /// 
    /// Coded by: Seth Frevert.
    /// </summary>
    public class UserManager : IUserManager
    {
        /// <summary>
        /// Method to add a new user to the database.
        /// </summary>
        /// <param name="newUser"> User to add to database.</param>
        /// <returns> Returns the number of rows affected.</returns>
        public int AddUser(User newUser)
        {
            Database db = new Database();
            int result = 0;
            try
            {
                result = db.AddUser(newUser);
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
        /// <param name="userToUpdate"> User to update in the database.</param>
        /// <returns> Returns the number of rows affected.</returns>
        public int UpdateUser(User userToUpdate)
        {
            userToUpdate.Salt = SecurityHelper.GenerateSalt();
            userToUpdate.Password = SecurityHelper.HashPassword(userToUpdate.Password, userToUpdate.Salt);
            Database db = new Database();
            int result = 0;
            try
            {
                result = db.UpdateUser(userToUpdate);
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
        /// <param name="userToDelete"> User to delete form the database.</param>
        /// <returns> Returns the number of rows affected.</returns>
        public int DeleteUser(User userToDelete)
        {
            Database db = new Database();
            int result = 0;
            try
            {
                result = db.DeleteUser(userToDelete);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return result;
        }

    }
}
