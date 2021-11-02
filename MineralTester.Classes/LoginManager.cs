using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineralTester.Classes
{
    public class LoginManager : ILoginManager
    {
        public User Login(string username, String password)
        {
            Database db = new Database();
            if (db.CheckUserExists(username))
            {
                User user = db.GetUser(username);
                if (user.Password == password)
                {
                    return user;
                }
            }
            return null;
        }

        public User Logout(ref User user)
        {
            user = null;
            return user;
        }
    }
}
