using Microsoft.VisualStudio.TestTools.UnitTesting;
using MineralTester.Classes;
using System.Collections.Generic;

namespace MineralTester.Tests
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void CheckUserExistsTest()
        {
            string user = "student";
            IDatabase db = new Database();
            Assert.AreEqual(true, db.CheckUserExists(user));
        }

        [TestMethod]
        public void CheckUserDoesntExistsTest()
        {
            string user = "RandomUserNotReal";
            IDatabase db = new Database();
            Assert.AreEqual(false, db.CheckUserExists(user));
        }

        [TestMethod]
        public void GetRealUser()
        {
            IDatabase db = new Database();
            User user = db.GetUser("admin");
            Assert.AreEqual(1, user.ID);
        }

        [TestMethod]
        public void AddExistingUser()
        {
            IDatabase db = new Database();
            string user = "admin";
            bool exists = db.CheckUserExists(user);
            Assert.AreEqual(true, exists);
            // Only true when use already exists.
            // In code this is checked prior to adding.
        }

        [TestMethod]
        public void AddNewUser()
        {
            IDatabase db = new Database();
            User user = new User(0, "TESTING", "TESTING", "TESTING", "TESTING", Enums.AccountType.Student);

            if (db.CheckUserExists(user.Username))
            {
                db.DeleteUser(db.GetUser(user.Username));
            }
            
            db.AddUser(user);
            Assert.AreEqual(true, db.CheckUserExists("TESTING"));


            db.DeleteUser(db.GetUser(user.Username));
        }

        [TestMethod]
        public void UpdateUser()
        {
            IDatabase db = new Database();
            User user = new User(0, "TESTING", "TESTING", "TESTING", "TESTING", Enums.AccountType.Student);

            if (db.CheckUserExists(user.Username))
            {
                db.DeleteUser(db.GetUser(user.Username));
            }

            db.AddUser(user);
            user = db.GetUser(user.Username);
            user.FirstName = "UPDATED";
            user.LastName = "UPDATED";
            user.Username = "UPDATED";
            user.Password = "UPDATED";
            user.AccountType = Enums.AccountType.Teacher;

            // 1 should be amount of rows affected.
            Assert.AreEqual(1, db.UpdateUser(user));
            Assert.AreEqual("UPDATED", db.GetUser(user.Username).FirstName);
            Assert.AreEqual("UPDATED", db.GetUser(user.Username).LastName);
            Assert.AreEqual("UPDATED", db.GetUser(user.Username).Username);
            Assert.AreEqual("UPDATED", db.GetUser(user.Username).Password);
            Assert.AreEqual(Enums.AccountType.Teacher, db.GetUser(user.Username).AccountType);

            db.DeleteUser(db.GetUser("UPDATED"));
        }
    
        [TestMethod]
        public void DeleteRealUser()
        {
            IDatabase db = new Database();
            User user = new User(0, "TESTING", "TESTING", "newUser", "TESTING", Enums.AccountType.Student);

            if (db.CheckUserExists(user.Username))
            {
                db.DeleteUser(db.GetUser(user.Username));
            }

            db.AddUser(user);
            Assert.AreEqual(true, db.CheckUserExists("newUser"));


            db.DeleteUser(db.GetUser(user.Username));
            Assert.AreEqual(false, db.CheckUserExists("newUser"));
        }

        [TestMethod]
        public void DeleteFakeUser()
        {
            IDatabase db = new Database();
            string user = "fakeaccount";
            bool exists = db.CheckUserExists(user);
            Assert.AreEqual(false, exists);
            // Only true when use already exists.
            // In code this is checked prior to adding.
        }
    }
}
