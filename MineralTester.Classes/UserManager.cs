using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineralTester.Classes
{
    public class UserManager : IUserManager
    {
        public void AddUser(User newUser)
        {
            Database db = new Database();
            try
            {
                db.AddUser(newUser);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }
    }
}
