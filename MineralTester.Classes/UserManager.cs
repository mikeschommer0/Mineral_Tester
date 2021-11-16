using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineralTester.Classes
{
    public class UserManager : IUserManager
    {
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
        public int UpdateUser(User userToUpdate)
        {
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
