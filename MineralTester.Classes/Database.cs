using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace MineralTester.Classes
{
    public class Database
    {
        private string connectionStringToDB = ConfigurationManager.ConnectionStrings["MySQLDB"].ConnectionString;

        private int RowsEffected = 0;

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
        /// UPDATES: RowsEffected for testing.
        /// 
        /// Takes in a user to insert into DB.
        /// </summary>
        /// <param name="newUser">New User to add to DB</param>
        public void AddUser(User newUser)
        {
            //planned to be used for testing
            this.RowsEffected = 0;

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
            //ExecuteNonQuery is used as it will be useful in
            //testing at later date to see if insertion has occurred.
            this.RowsEffected = addNewUser.ExecuteNonQuery();
            conn.Close();
        }

        /// <summary>
        /// ASSUMES: That CheckUserExists has been ran and return
        /// true prior to call.
        /// 
        /// UPDATES: RowsEffected for testing.
        /// 
        /// Takes in a user to delete from DB.
        /// </summary>
        /// <param name="userToDelete">User to delete from DB</param>
        public void DeleteUser(User userToDelete)
        {
            //planned to be used for testing
            this.RowsEffected = 0;

            //open connection
            MySqlConnection conn = new MySqlConnection(connectionStringToDB);
            conn.Open();

            //create command to delete entry and populate
            MySqlCommand deleteUser = new MySqlCommand("DELETE FROM users WHERE" +
                " user_id = @user_id", conn);
            deleteUser.Parameters.Add(new MySqlParameter("user_id", userToDelete.ID));

            //run delete and close connection
            //ExecuteNonQuery is used as it will be useful in
            //testing at later date to see if deletion has occurred.
            this.RowsEffected = deleteUser.ExecuteNonQuery();
            conn.Close();
        }

        /// <summary>
        /// Simple method to allow for validation of command execution.
        /// </summary>
        /// <param name="expectedEffected">How many rows were expected to be
        /// effected</param>
        /// <returns>true if matched else false.</returns>
        public bool ValidateExecution(int expectedEffected)
        {
            if(this.RowsEffected == expectedEffected)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Question> GetQuestions()
        {
            List<Question> questions = new List<Question>();
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT * FROM questions", connection);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        questions.Add(new Question((int)reader["question_id"], (string)reader["description"]));
                    }
                }
                connection.Close();
                return questions;
            }
        }

        public void InsertQuestion(string description)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("INSERT INTO questions (description) VALUES (@description)", connection);
                command.Parameters.Add(new MySqlParameter("description", description));
                MySqlDataReader reader = command.ExecuteReader();
                connection.Close();
            }
        }

        public void UpdateQuestion(int idToUpdate, string description)
        {
            List<Question> questions = GetQuestions();
            if (!questions.Any(q => q.QuestionID == idToUpdate))
            {
                throw new ArgumentException("Error updating question, ID not found");
            }
            else
            {
                using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("UPDATE questions SET description = @description WHERE question_id = @id_to_update", connection);
                    command.Parameters.Add(new MySqlParameter("description", description));
                    command.Parameters.Add(new MySqlParameter("id_to_update", idToUpdate));
                    MySqlDataReader reader = command.ExecuteReader();
                    connection.Close();
                }
            }
        }

        public void DeleteQuestion(int idToDelete)
        {
            List<Question> questions = GetQuestions();
            if (!questions.Any(q => q.QuestionID == idToDelete))
            {
                throw new ArgumentException("Error deleting question, ID not found");
            }
            else
            {
                using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("DELETE FROM questions WHERE question_id == @id_to_delete");
                    command.Parameters.Add(new MySqlParameter("id_to_delete", idToDelete));
                    MySqlDataReader reader = command.ExecuteReader();
                    connection.Close();
                }
            }
        }

        public List<Answer> GetAnswers()
        {
            List<Answer> answers = new List<Answer>();
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT * FROM answers", connection);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        answers.Add(new Answer((int)reader["answer_id"], (string)reader["description"]));
                    }
                }
                connection.Close();
                return answers;
            }
        }

        public void InsertAnswer(string description)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("INSERT INTO answers (description) VALUES (@description)", connection);
                command.Parameters.Add(new MySqlParameter("description", description));
                MySqlDataReader reader = command.ExecuteReader();
                connection.Close();
            }
        }

        public void UpdateAnswer(int idToUpdate, string description)
        {
            List<Answer> answers = GetAnswers();
            if (!answers.Any(q => q.AnswerID == idToUpdate))
            {
                throw new ArgumentException("Error updating answer, ID not found");
            }
            else
            {
                using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("UPDATE answers SET description = @description WHERE answer_id = @id_to_update", connection);
                    command.Parameters.Add(new MySqlParameter("description", description));
                    command.Parameters.Add(new MySqlParameter("id_to_update", idToUpdate));
                    MySqlDataReader reader = command.ExecuteReader();
                    connection.Close();
                }
            }
        }

        public void DeleteAnswer(int idToDelete)
        {
            List<Answer> answers = GetAnswers();
            if (!answers.Any(q => q.AnswerID == idToDelete))
            {
                throw new ArgumentException("Error deleting answer, ID not found");
            }
            else
            {
                using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("DELETE FROM answers WHERE answer_id == @id_to_delete");
                    command.Parameters.Add(new MySqlParameter("id_to_delete", idToDelete));
                    MySqlDataReader reader = command.ExecuteReader();
                    connection.Close();
                }
            }
        }

        public void InsertQuestionAnswers(Question question)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();
                foreach (Answer answer in question.Answers)
                {
                    MySqlCommand command = new MySqlCommand("INSERT INTO question_answers (question_id, answer_id, is_correct) VALUES (@question_id, @answer_id, @is_correct)");
                    command.Parameters.Add(new MySqlParameter("question_id", question.QuestionID));
                    command.Parameters.Add(new MySqlParameter("answer_id", answer.AnswerID));
                    command.Parameters.Add(new MySqlParameter("is_correct", Convert.ToSByte(answer.IsCorrect)));
                    MySqlDataReader reader = command.ExecuteReader();
                    connection.Close();
                }
            }
        }

        public void DeleteQuestionAnswers(int questionID)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                MySqlCommand command = new MySqlCommand("DELETE FROM question_answers WHERE question_id = @question_id");
                command.Parameters.Add(new MySqlParameter("question_id", questionID));
                MySqlDataReader reader = command.ExecuteReader();
                connection.Close();
            }
        }

        public List<Answer> GetQuestionAnswers(int question_id)
        {
            List<Answer> answers = new List<Answer>();
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("CALL get_answers(@question_id)", connection);
                command.Parameters.Add(new MySqlParameter("question_id", question_id));
                MySqlDataReader reader = command.ExecuteReader();
                if(reader.HasRows)
                {
                    while (reader.Read())
                    {
                        answers.Add(new Answer((int)reader["question_id"], (string)reader["description"], Convert.ToBoolean((sbyte)reader["is_correct"])));
                    }
                }
                connection.Close();
                return answers;
            }
        }
    }
}
