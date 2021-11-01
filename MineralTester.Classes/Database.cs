using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace MineralTester.Classes
{
    class Database
    {
        private string connectionStringToDB = ConfigurationManager.ConnectionStrings["MySQLDB"].ConnectionString;
    
        /// <summary>
        /// Method to check if a user does exist before
        /// attempting to pull user from DB.
        /// </summary>
        /// <param name="userName">User name to check if exists</param>
        /// <returns>true = exits | false = doesn't exist</returns>
        public bool CheckUserExists(string userName)
        {
            //build connection and open
            MySqlConnection conn = new MySqlConnection(connectionStringToDB);
            conn.Open();

            //command to check user is real
            MySqlCommand checkUser = new MySqlCommand("SELECT COUNT(*) FROM users WHERE" + " user_name = @userName", conn);
            checkUser.Parameters.Add(new MySqlParameter("userName", userName));
            
            //run command
            MySqlDataReader reader = checkUser.ExecuteReader();
            reader.Read();

            //pull result and close reader & connection
            int result = reader.GetInt32(0);
            reader.Close();
            conn.Close();

            if (result == 1)//if result is a match user exist
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// ASSUMES: That CheckUserExists has been ran and returned
        /// true prior to call.
        /// 
        /// Method to pull user info from DB and create
        /// user to return.
        /// </summary>
        /// <param name="userName">userName to get</param>
        /// <returns></returns>
        public User GetUser(string userName)
        {
            //build connection and open
            MySqlConnection conn = new MySqlConnection(connectionStringToDB);
            conn.Open();

            //command to get user info from DB
            MySqlCommand getUser = new MySqlCommand("SELECT * FROM users WHERE" + " user_name = @userName", conn);
            getUser.Parameters.Add(new MySqlParameter("userName", userName));

            //run command
            MySqlDataReader reader = getUser.ExecuteReader();
            reader.Read();

            //create user (int id, string firstName,
            //string lastName, string username,
            //string password, int accountType)
            User result = new User(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                reader.GetString(4),
                reader.GetInt32(5));

            //close reader & conn and return user 
            reader.Close();
            conn.Close();

            return result;
        }

        /// <summary>
        /// ASSUMES: That CheckUserExists has been ran and return
        /// false prior to call.
        /// 
        /// POSSIBLY: Changing return type for testing (return rows effect) TBT
        /// 
        /// Takes in a user to insert into DB.
        /// </summary>
        /// <param name="newUser">New User to add to DB</param>
        public void AddUser(User newUser)
        {
            //open connection
            MySqlConnection conn = new MySqlConnection(connectionStringToDB);
            conn.Open();

            //create command to insert new user and populate
            MySqlCommand addNewUser = new MySqlCommand("INSERT INTO users " +
                "(first_name, last_name, user_name, password, account_type)" +
                " VALUES(@first_name, @last_name, @user_name, @password, @account_type)", conn);
            addNewUser.Parameters.Add(new MySqlParameter("first_name", newUser.FirstName));
            addNewUser.Parameters.Add(new MySqlParameter("last_name", newUser.LastName));
            addNewUser.Parameters.Add(new MySqlParameter("user_name", newUser.Username));
            addNewUser.Parameters.Add(new MySqlParameter("password", newUser.Password));
            addNewUser.Parameters.Add(new MySqlParameter("account_type", newUser.AccountType));

            //run insert and close connection
            //ExecuteNonQuery is used as it will be usefull in
            //testing at later date to see if insertion has ran.
            int rowsEffected = addNewUser.ExecuteNonQuery();
            conn.Close();
        }
    }
}
