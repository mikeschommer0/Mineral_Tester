﻿using System;

namespace MineralTester.Classes
{
    public class LoginManager : ILoginManager
    {
        public User Login(string username, string password)
        {
            Database db = new Database();
            if (db.CheckUserExists(username))
            {
                //salt password
                User user = db.GetUser(username);
                password = SecurityHelper.HashPassword(password, user.Salt);
                if (user.Password.Equals(password))
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
