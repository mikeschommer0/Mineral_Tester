using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace MineralTester.Classes
{
    public class Database : IDatabase
    {
        private string connectionStringToDB = ConfigurationManager.ConnectionStrings["MySQLDB"].ConnectionString;

        private int _rowsEffected = 0; // MODIFIED.

        /// <summary>
        /// Method to check if a user does exist before
        /// attempting to pull user from DB.
        /// </summary>
        /// <param name="userName"> User name to check if exists.</param>
        /// <returns> True = exits | false = doesn't exist.</returns>
        public bool CheckUserExists(string userName)
        {
            // Build connection and open.
            // (MODIFIED).
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();

                // Command to check user is real.
                MySqlCommand checkUser = new MySqlCommand("SELECT COUNT(*) FROM users WHERE user_name = @userName", connection);
                checkUser.Parameters.Add(new MySqlParameter("userName", userName));

                // Run command.
                MySqlDataReader reader = checkUser.ExecuteReader();
                reader.Read();

                // Pull result and close reader & connection.
                int result = reader.GetInt32(0);
                reader.Close();
                connection.Close();

                // (MODIFIED).
                return result == 1;
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
            // Build connection and open.
            // (MODIFIED).
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();

                // Command to get user info from DB.
                MySqlCommand getUser = new MySqlCommand("SELECT * FROM users WHERE" + " user_name = @userName", connection);
                getUser.Parameters.Add(new MySqlParameter("userName", userName));

                // Run command.
                MySqlDataReader reader = getUser.ExecuteReader();
                reader.Read();

                // Create user (int id, string firstName,
                // String lastName, string username,
                // String password, int accountType).
                User result = new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                    reader.GetString(3), reader.GetString(4), (Enums.AccountType)reader.GetInt32(5));

                // Close reader & conn and return user.
                reader.Close();
                connection.Close();

                return result;
            }
        }

        /// <summary>
        /// ASSUMES: That CheckUserExists has been ran and return
        /// false prior to call.
        /// 
        /// UPDATES: _RowsEffected for testing.
        /// 
        /// Takes in a user to insert into DB.
        /// </summary>
        /// <param name="newUser"> New User to add to DB.</param>
        /// <returns>An int that is equal to the rows affected.</returns>
        public int AddUser(User newUser)
        {
            // Planned to be used for testing.
            _rowsEffected = 0;

            // Open connection.
            // (MODIFIED).
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();

                // Create command to insert new user and populate.
                MySqlCommand addNewUser = new MySqlCommand("INSERT INTO users " +
                    "(first_name, last_name, user_name, password, account_type)" +
                    " VALUES(@first_name, @last_name, @user_name, @password, @account_type)", connection);
                addNewUser.Parameters.Add(new MySqlParameter("first_name", newUser.FirstName));
                addNewUser.Parameters.Add(new MySqlParameter("last_name", newUser.LastName));
                addNewUser.Parameters.Add(new MySqlParameter("user_name", newUser.Username));
                addNewUser.Parameters.Add(new MySqlParameter("password", newUser.Password));
                addNewUser.Parameters.Add(new MySqlParameter("account_type", newUser.AccountType));

                // Run insert and close connection
                // ExecuteNonQuery is used as it will be useful in
                // Testing at later date to see if insertion has occurred.
                _rowsEffected = addNewUser.ExecuteNonQuery();
                connection.Close();
            }
            return _rowsEffected;
        }

        /// <summary>
        /// ASSUMES: That CheckUserExists has been ran and return
        /// true prior to call.
        /// 
        /// UPDATES: _RowsEffected for testing.
        /// 
        /// Takes in a user to update in DB.
        /// </summary>
        /// <param name="userToUpdate"> User to update in DB.</param>
        /// <returns>An int that is equal to the rows affected.</returns>
        public int UpdateUser(User userToUpdate)
        {
            _rowsEffected = 0;

            using(MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();

                MySqlCommand updateUser = new MySqlCommand("UPDATE users SET first_name = @first_name, last_name = @last_name, user_name = @user_name, " + 
                   "password = @password, account_type = @account_type WHERE user_id = @user_id", connection);
                updateUser.Parameters.Add(new MySqlParameter("first_name", userToUpdate.FirstName));
                updateUser.Parameters.Add(new MySqlParameter("last_name", userToUpdate.LastName));
                updateUser.Parameters.Add(new MySqlParameter("user_name", userToUpdate.Username));
                updateUser.Parameters.Add(new MySqlParameter("password", userToUpdate.Password));
                updateUser.Parameters.Add(new MySqlParameter("account_type", userToUpdate.AccountType));
                updateUser.Parameters.Add(new MySqlParameter("user_id", userToUpdate.ID));

                _rowsEffected = updateUser.ExecuteNonQuery();
                connection.Close();
            }
            return _rowsEffected;
        }

        /// <summary>
        /// ASSUMES: That CheckUserExists has been ran and return
        /// true prior to call.
        /// 
        /// UPDATES: _RowsEffected for testing.
        /// 
        /// Takes in a user to delete from DB.
        /// </summary>
        /// <param name="userToDelete"> User to delete from DB.</param>
        /// <returns>An int that is equal to the rows affected.</returns>
        public int DeleteUser(User userToDelete)
        {
            // Planned to be used for testing.
            _rowsEffected = 0;


            // Open connection.
            // (MODIFIED).
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();

                // Create command to delete entry and populate.
                MySqlCommand deleteUser = new MySqlCommand("DELETE FROM users WHERE" +
                    " user_id = @user_id", connection);
                deleteUser.Parameters.Add(new MySqlParameter("user_id", userToDelete.ID));

                // Run delete and close connection
                // ExecuteNonQuery is used as it will be useful in
                // Testing at later date to see if deletion has occurred.
                _rowsEffected = deleteUser.ExecuteNonQuery();
                connection.Close();
            }
            return _rowsEffected;
        }

        /// <summary>
        /// Simple method to allow for validation of command execution.
        /// </summary>
        /// <param name="expectedEffected">How many rows were expected to be
        /// effected</param>
        /// <returns>true if matched else false.</returns>
        public bool ValidateExecution(int expectedEffected)
        {
            // (MODIFIED).
            return _rowsEffected == expectedEffected;
        }

        /// <summary>
        /// Gets a list of all questions from the database. (MODIFIED)
        /// </summary>
        /// <returns> Returns a list of all questions in the database if there are any, otherwise returns an empty list. (MODIFIED)</returns>
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

        /// <summary>
        /// Inserts a new question into the database. (MODIFIED)
        /// </summary>
        /// <param name="description"> Description is the string representation of the question. (MODIFIED)</param>
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

        /// <summary>
        /// Updates an existing question in the database. (MODIFIED)
        /// </summary>
        /// <param name="idToUpdate">/> Is the question id being updated. (MODIFIED)</param>
        /// <param name="description"> Is the string representation of the question. (MODIFIED)</param>
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

        /// <summary>
        /// Deletes an existing question in the database. (MODIFIED)
        /// </summary>
        /// <param name="idToDelete"> Is the question id to delete. (MODIFIED)</param>
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
                    MySqlCommand command = new MySqlCommand("DELETE FROM questions WHERE question_id = @id_to_delete", connection);
                    command.Parameters.Add(new MySqlParameter("id_to_delete", idToDelete));
                    MySqlDataReader reader = command.ExecuteReader();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Gets a list of all answers in the database. (MODIFIED)
        /// </summary>
        /// <returns> A list of all answers in the database if there are any, otherwise returns an empty list. (MODIFIED)</returns>
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

        /// <summary>
        /// Inserts a new answer into the database. (MODIFIED)
        /// </summary>
        /// <param name="description"> Is the string representation of the answer. (MODIFIED)</param>
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

        /// <summary>
        /// Updates an existing answer in the database. (MODIFIED)
        /// </summary>
        /// <param name="idToUpdate"> Is the answer id being updated. (MODIFIED)</param>
        /// <param name="description"> It the string representation of the answer. (MODIFIED)</param>
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

        /// <summary>
        /// Deletes an existing answer in the database. (MODIFIED)
        /// </summary>
        /// <param name="idToDelete"> Is the answer id to delete. (MODIFIED)</param>
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
                    MySqlCommand command = new MySqlCommand("DELETE FROM answers WHERE answer_id = @id_to_delete", connection);
                    command.Parameters.Add(new MySqlParameter("id_to_delete", idToDelete));
                    MySqlDataReader reader = command.ExecuteReader();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Inserts a question and its corresponding answers into the question_answer junction table. (MODIFIED)
        /// </summary>
        /// <param name="question"> Is the question whose data will be inserted. (MODIFIED)</param>
        public (bool isSuccess, string message) InsertQuestionAnswers(Question question)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
                {
                    foreach (Answer answer in question.Answers)
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand("INSERT INTO question_answers (question_id, answer_id, is_correct) VALUES (@question_id, @answer_id, @is_correct)", connection);
                        command.Parameters.Add(new MySqlParameter("question_id", question.QuestionID));
                        command.Parameters.Add(new MySqlParameter("answer_id", answer.AnswerID));
                        command.Parameters.Add(new MySqlParameter("is_correct", Convert.ToSByte(answer.IsCorrect)));
                        MySqlDataReader reader = command.ExecuteReader();
                        connection.Close();
                    }
                }
                return (true, "Successfully added practice question.");
            }
            catch (Exception exception)
            {
                return (false, string.Format($"Unable to add practice question: {exception.Message}"));
            }
        }

        /// <summary>
        /// Deletes all of the answers to the selected question form the question_answer junction table. (MODIFIED)
        /// </summary>
        /// <param name="questionID"> Is the question that will have all of its answers deleted from the junction table. (MODIFIED)</param>
        public void DeleteQuestionAnswers(int questionID)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("DELETE FROM question_answers WHERE question_id = @question_id", connection);
                command.Parameters.Add(new MySqlParameter("question_id", questionID));
                MySqlDataReader reader = command.ExecuteReader();
                connection.Close();
            }
        }

        /// <summary>
        /// Gets a list of answers for the input question id. (MODIFIED)
        /// </summary>
        /// <param name="questionID"> Is the question to get answers for from 
        /// the question_answer junction table. (MODIFIED)</param>
        /// <returns> A list of answers for the given question if
        /// there are any, or an empty list if there are none. (MODIFIED)</returns>
        public List<Answer> GetQuestionAnswers(int questionID)
        {
            List<Answer> answers = new List<Answer>();
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("CALL get_answers(@question_id)", connection);
                command.Parameters.Add(new MySqlParameter("question_id", questionID));
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
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

        /// <summary>
        /// Check mineral does not exist under same name in DB.
        /// </summary>
        /// <param name="mineralName"></param>
        /// <returns></returns>
        public bool CheckMineralExists(string mineralName)
        {
            // Build connection and open.
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();

                // Command to check mineral exists.
                MySqlCommand checkUser = new MySqlCommand("SELECT COUNT(*) FROM minerals WHERE name = @mineralName", connection);
                checkUser.Parameters.Add(new MySqlParameter("mineralName", mineralName));

                // Run command.
                MySqlDataReader reader = checkUser.ExecuteReader();
                reader.Read();

                // Pull result and close reader & connection.
                int result = reader.GetInt32(0);
                reader.Close();
                connection.Close();

                return result == 1;
            }
        }

        /// <summary>
        /// *** Should check exists prior to adding. ***
        /// Get a mineral to add to the database.
        /// </summary>
        /// <param name="toAdd"></param>
        public void AddMineral(Mineral toAdd)
        {
            // Planned to be used for testing.
            _rowsEffected = 0;

            // Open connection.
            // (MODIFIED).
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();

                // Create command to insert new mineral and populate.
                MySqlCommand addNewMineral = new MySqlCommand("INSERT INTO minerals " +
                    "(name, hardness, is_magnetic, acid_reaction, image)" +
                    " VALUES(@name, @hardness, @is_magnetic, @acid_reaction, @image)", connection);
                addNewMineral.Parameters.Add(new MySqlParameter("name", toAdd.Name));
                addNewMineral.Parameters.Add(new MySqlParameter("hardness", toAdd.Hardness));
                addNewMineral.Parameters.Add(new MySqlParameter("is_magnetic", toAdd.IsMagnetic));
                addNewMineral.Parameters.Add(new MySqlParameter("acid_reaction", toAdd.AcidReaction));
                addNewMineral.Parameters.Add(new MySqlParameter("image", toAdd.Image));

                // Run insert and close connection
                // ExecuteNonQuery is used as it will be useful in
                // Testing at later date to see if insertion has occurred.
                _rowsEffected = addNewMineral.ExecuteNonQuery();
                connection.Close();
            }
        }

        /// <summary>
        /// Gets Mineral from db.
        /// *** Should check exists prior to adding. ***
        /// </summary>
        /// <param name="mineralName"></param>
        /// <returns></returns>
        public Mineral GetMineral(string mineralName)
        {
            // Build connection and open.
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();

                // Command to get user info from DB.
                MySqlCommand getUser = new MySqlCommand("SELECT * FROM minerals WHERE" + " name = @mineralName", connection);
                getUser.Parameters.Add(new MySqlParameter("mineralName", mineralName));

                // Run command.
                MySqlDataReader reader = getUser.ExecuteReader();
                reader.Read();

                // Create mineral (int id, string name,
                // int hardness, bool IsMagnetic,
                // bool AcidReaction, byte[] Image).
                Mineral result = new Mineral(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2),
                    reader.GetBoolean(3), reader.GetBoolean(4), (byte[])reader["image"]);

                // Close reader & conn and return user.
                reader.Close();
                connection.Close();

                return result;
            }
        }

        /// <summary>
        /// Gets all minerals from database.
        /// </summary>
        public List<Mineral> GetMinerals()
        {
            List<Mineral> minerals = new List<Mineral>();

            // Open connection.
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();
                // Create command to insert new mineral and populate.
                MySqlCommand getMinerals = new MySqlCommand("SELECT * FROM minerals", connection);
                MySqlDataReader reader = getMinerals.ExecuteReader();

                while(reader.Read())
                {
                    Mineral mineral = new Mineral();
                    mineral.Name = reader["name"].ToString();
                    mineral.Hardness = (float)Convert.ToDouble(reader["hardness"]);
                    mineral.IsMagnetic = Convert.ToBoolean(reader["is_magnetic"]);
                    mineral.AcidReaction = Convert.ToBoolean(reader["acid_reaction"]);
                    mineral.Image = (byte[])reader["image"];
                    minerals.Add(mineral);
                }

                connection.Close();
            }

            return minerals;
        }
    }
}
