using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

/// <summary>
/// User and Mineral methods coded by Quinn Nimmer.
/// Question/Answer methods coded by Rick Bowman.
/// </summary>
namespace MineralTester.Classes
{
    /// <summary>
    /// Database interaction class.
    /// </summary>
    public class Database : IDatabase
    {
        private string connectionStringToDB = ConfigurationManager.ConnectionStrings["MySQLDB"].ConnectionString;
        private int _rowsEffected = 0;

        /// <summary>
        /// Method to check if a user does exist before attempting to pull user from DB.
        /// </summary>
        /// <param name="userName"> User name to check if exists.</param>
        /// <returns> True = exits | false = doesn't exist.</returns>
        public bool CheckUserExists(string userName)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();
                MySqlCommand checkUser = new MySqlCommand("SELECT COUNT(*) FROM users WHERE user_name = @userName", connection);
                checkUser.Parameters.Add(new MySqlParameter("userName", userName));
                MySqlDataReader reader = checkUser.ExecuteReader();
                reader.Read();
                int result = reader.GetInt32(0);
                reader.Close();
                connection.Close();
                return result == 1;
            }
        }

        /// <summary>
        /// ASSUMES: That CheckUserExists has been ran and returned true prior to call.
        /// Method to pull user info from DB and create user to return.
        /// </summary>
        /// <param name="userName"> userName to get. </param>
        /// <returns> Returns the specified user. </returns>
        public User GetUser(string userName)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();
                MySqlCommand getUser = new MySqlCommand("SELECT * FROM users WHERE user_name = @userName", connection);
                getUser.Parameters.Add(new MySqlParameter("userName", userName));
                MySqlDataReader reader = getUser.ExecuteReader();
                reader.Read();
                User result = new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), (Enums.AccountType)reader.GetInt32(5), reader.GetString(6));
                reader.Close();
                connection.Close();
                return result;
            }
        }

        /// <summary>
        /// Method to pull user info from DB and create users to return.
        /// </summary>
        /// <returns> List of users. </returns>
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();
                MySqlCommand getUsers = new MySqlCommand("SELECT * FROM users", connection);
                MySqlDataReader reader = getUsers.ExecuteReader();
                while (reader.Read())
                {
                    User user = new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                    reader.GetString(3), reader.GetString(4), (Enums.AccountType)reader.GetInt32(5), reader.GetString(6));
                    users.Add(user);
                }
                connection.Close();
            }
            return users;
        }

        /// <summary>
        /// ASSUMES: That CheckUserExists has been ran and return false prior to call.
        /// UPDATES: _RowsEffected for testing.
        /// Takes in a user to insert into DB.
        /// </summary>
        /// <param name="newUser"> New User to add to DB. </param>
        /// <returns> An int that is equal to the rows affected. </returns>
        public int AddUser(User newUser)
        {
            _rowsEffected = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();
                MySqlCommand addNewUser = new MySqlCommand("INSERT INTO users (first_name, last_name, user_name, password, account_type, salt) VALUES(@first_name, @last_name, @user_name, @password, @account_type, @salt)", connection);
                addNewUser.Parameters.Add(new MySqlParameter("first_name", newUser.FirstName));
                addNewUser.Parameters.Add(new MySqlParameter("last_name", newUser.LastName));
                addNewUser.Parameters.Add(new MySqlParameter("user_name", newUser.Username));
                addNewUser.Parameters.Add(new MySqlParameter("password", newUser.Password));
                addNewUser.Parameters.Add(new MySqlParameter("account_type", newUser.AccountType));
                addNewUser.Parameters.Add(new MySqlParameter("salt", newUser.Salt));
                _rowsEffected = addNewUser.ExecuteNonQuery();
                connection.Close();
            }
            return _rowsEffected;
        }

        /// <summary>
        /// ASSUMES: That CheckUserExists has been ran and return true prior to call.
        /// UPDATES: _RowsEffected for testing.
        /// Takes in a user to update in DB.
        /// </summary>
        /// <param name="userToUpdate"> User to update in DB. </param>
        /// <returns> An int that is equal to the rows affected. </returns>
        public int UpdateUser(User userToUpdate)
        {
            _rowsEffected = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();
                MySqlCommand updateUser = new MySqlCommand("UPDATE users SET first_name = @first_name, last_name = @last_name, user_name = @user_name, password = @password, account_type = @account_type, salt = @salt WHERE user_id = @user_id", connection);
                updateUser.Parameters.Add(new MySqlParameter("first_name", userToUpdate.FirstName));
                updateUser.Parameters.Add(new MySqlParameter("last_name", userToUpdate.LastName));
                updateUser.Parameters.Add(new MySqlParameter("user_name", userToUpdate.Username));
                updateUser.Parameters.Add(new MySqlParameter("password", userToUpdate.Password));
                updateUser.Parameters.Add(new MySqlParameter("account_type", userToUpdate.AccountType));
                updateUser.Parameters.Add(new MySqlParameter("user_id", userToUpdate.ID));
                updateUser.Parameters.Add(new MySqlParameter("salt", userToUpdate.Salt));
                _rowsEffected = updateUser.ExecuteNonQuery();
                connection.Close();
            }
            return _rowsEffected;
        }

        /// <summary>
        /// ASSUMES: That CheckUserExists has been ran and return true prior to call.
        /// UPDATES: _RowsEffected for testing.
        /// Takes in a user to delete from DB.
        /// </summary>
        /// <param name="userToDelete"> User to delete from DB. </param>
        /// <returns> An int that is equal to the rows affected. </returns>
        public int DeleteUser(User userToDelete)
        {
            _rowsEffected = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();
                MySqlCommand deleteUser = new MySqlCommand("DELETE FROM users WHERE user_id = @user_id", connection);
                deleteUser.Parameters.Add(new MySqlParameter("user_id", userToDelete.ID));
                _rowsEffected = deleteUser.ExecuteNonQuery();
                connection.Close();
            }
            return _rowsEffected;
        }

        /// <summary>
        /// Simple method to allow for validation of command execution.
        /// </summary>
        /// <param name="expectedEffected"> How many rows were expected to be effected. </param>
        /// <returns> True if matched else false.</returns>
        public bool ValidateExecution(int expectedEffected)
        {
            return _rowsEffected == expectedEffected;
        }

        /// <summary>
        /// Gets the highest ID from the question table.
        /// </summary>
        /// <returns> Returns the highest integer ID from the question table. </returns>
        private int GetHighestQuestionID()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT MAX(question_id) FROM questions;", connection);
                MySqlDataReader reader = command.ExecuteReader();
                dynamic queryResult = null;
                while (reader.Read())
                {
                    queryResult = reader.GetValue(0).ToString();
                }
                if (queryResult == "")
                {
                    return -1;
                }
                else
                {
                    return int.Parse(queryResult);
                }
            }
        }

        /// <summary>
        /// Gets a list of all questions from the database.
        /// </summary>
        /// <returns> Returns a list of all questions in the database if there are any, otherwise returns an empty list. </returns>
        public List<Question> GetQuestions()
        {
            List<Question> questions = new List<Question>();
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT * FROM questions;", connection);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        questions.Add(new Question((int)reader["question_id"], (string)reader["description"]));
                    }
                }
                connection.Close();
            }
            if (questions.Count > 0)
            {
                foreach (Question question in questions)
                {
                    GetQuestionAnswers(question);
                }
            }
            return questions;
        }

        /// <summary>
        /// Gets the answers associated with the indicated question.
        /// </summary>
        /// <param name="question"> The indicated question to get answers for. </param>
        private void GetQuestionAnswers(Question question)
        {
            question.Answers = new List<Answer>();
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT * FROM question_answers WHERE question_id = @question_id", connection);
                command.Parameters.Add(new MySqlParameter("question_id", question.QuestionID));
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        question.Answers.Add(new Answer((string)reader["answer_description"], Convert.ToBoolean((sbyte)reader["is_correct"])));
                    }
                }
            }
        }

        /// <summary>
        /// Inserts a new question into the database.
        /// </summary>
        /// <param name="description"> Description is the string representation of the question. </param>
        public void InsertQuestion(Question questionToInsert)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("INSERT INTO questions (description) VALUES (@description);", connection);
                command.Parameters.Add(new MySqlParameter("description", questionToInsert.Description));
                MySqlDataReader reader = command.ExecuteReader();
                connection.Close();
            }
            InsertAnswers(questionToInsert);
        }

        /// <summary>
        /// Inserts the indicated question's associated answers into the database.
        /// </summary>
        /// <param name="question"> The indicated question. </param>
        private void InsertAnswers(Question question)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("", connection);
                int questionID;
                if (question.QuestionID == 0)
                {
                    questionID = GetHighestQuestionID();
                }
                else
                {
                    questionID = question.QuestionID;
                }
                for (int i = 0; i < question.Answers.Count; i++)
                {
                    command.CommandText += $"INSERT INTO question_answers (question_id, answer_description, is_correct) VALUES (@question_id{i}, @answer_description{i}, @is_correct{i});";
                    command.Parameters.Add(new MySqlParameter($"question_id{i}", questionID));
                    command.Parameters.Add(new MySqlParameter($"answer_description{i}", question.Answers[i].Description));
                    command.Parameters.Add(new MySqlParameter($"is_correct{i}", question.Answers[i].IsCorrect));
                }
                MySqlDataReader reader = command.ExecuteReader();
                connection.Close();
            }
        }

        /// <summary>
        /// Updates an existing question in the database.
        /// </summary>
        /// <param name="questionToUpdate"> The question to update. </param>
        public void UpdateQuestion(Question questionToUpdate)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("UPDATE questions SET description = @description WHERE question_id = @question_id;DELETE FROM question_answers WHERE question_id = @question_id;", connection);
                command.Parameters.Add(new MySqlParameter("description", questionToUpdate.Description));
                command.Parameters.Add(new MySqlParameter("question_id", questionToUpdate.QuestionID));
                MySqlDataReader reader = command.ExecuteReader();
            }
            InsertAnswers(questionToUpdate);
        }

        /// <summary>
        /// Deletes an existing question in the database.
        /// </summary>
        /// <param name="idToDelete"> Is the question id to delete. </param>
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
                    MySqlCommand command = new MySqlCommand("DELETE FROM questions WHERE question_id = @id_to_delete; DELETE FROM question_answers WHERE question_id = @id_to_delete;", connection);
                    command.Parameters.Add(new MySqlParameter("id_to_delete", idToDelete));
                    MySqlDataReader reader = command.ExecuteReader();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Check mineral does not exist under same name in DB.
        /// </summary>
        /// <param name="mineralName"> The mineral to check. </param>
        /// <returns> <see langword="true"/>if the mineral exists, false otherwise. </returns>
        public bool CheckMineralExists(string mineralName)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();
                MySqlCommand checkMineralExists = new MySqlCommand("SELECT COUNT(*) FROM minerals WHERE name = @mineralName", connection);
                checkMineralExists.Parameters.Add(new MySqlParameter("mineralName", mineralName));
                MySqlDataReader reader = checkMineralExists.ExecuteReader();
                reader.Read();
                int result = reader.GetInt32(0);
                reader.Close();
                connection.Close();
                return result == 1;
            }
        }

        /// <summary>
        /// Adds a mineral to the database
        /// </summary>
        /// <param name="mineralToAdd"> The mineral to add. </param>
        public void AddMineral(Mineral mineralToAdd)
        {
            _rowsEffected = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();
                MySqlCommand addNewMineral = new MySqlCommand("INSERT INTO minerals (name, hardness, is_magnetic, acid_reaction, image, streak_color) VALUES(@name, @hardness, @is_magnetic, @acid_reaction, @image, @streak_color)", connection);
                addNewMineral.Parameters.Add(new MySqlParameter("name", mineralToAdd.Name));
                addNewMineral.Parameters.Add(new MySqlParameter("hardness", mineralToAdd.Hardness));
                addNewMineral.Parameters.Add(new MySqlParameter("is_magnetic", mineralToAdd.IsMagnetic));
                addNewMineral.Parameters.Add(new MySqlParameter("acid_reaction", mineralToAdd.AcidReaction));
                addNewMineral.Parameters.Add(new MySqlParameter("image", mineralToAdd.Image));
                addNewMineral.Parameters.Add(new MySqlParameter("streak_color", mineralToAdd.StreakColor));
                _rowsEffected = addNewMineral.ExecuteNonQuery();
                connection.Close();
            }
        }

        /// <summary>
        /// Gets Mineral from db.
        /// </summary>
        /// <param name="mineralName"> The mineral to get. </param>
        /// <returns> The specified mineral. </returns>
        public Mineral GetMineral(string mineralName)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();
                MySqlCommand getMineral = new MySqlCommand("SELECT * FROM minerals WHERE" + " name = @mineralName", connection);
                getMineral.Parameters.Add(new MySqlParameter("mineralName", mineralName));
                MySqlDataReader reader = getMineral.ExecuteReader();
                reader.Read();
                Mineral result = new Mineral(reader.GetInt32(0), reader.GetString(1), reader.GetFloat(2), reader.GetBoolean(3), reader.GetBoolean(4), (byte[])reader["image"], reader.GetString(6));
                reader.Close();
                connection.Close();
                return result;
            }
        }

        /// <summary>
        /// Delete mineral in DB.
        /// </summary>
        /// <param name="mineralToDelete"> The mineral to delete. </param>
        /// <returns> The amount of rows affected by the query. </returns>
        public int DeleteMineral(Mineral mineralToDelete)
        {
            _rowsEffected = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();
                MySqlCommand deleteMineral = new MySqlCommand("DELETE FROM minerals WHERE mineral_id = @mineral_id", connection);
                deleteMineral.Parameters.Add(new MySqlParameter("mineral_id", mineralToDelete.ID));
                _rowsEffected = deleteMineral.ExecuteNonQuery();
                connection.Close();
            }
            return _rowsEffected;
        }

        /// <summary>
        /// Updates a mineral entry in the database.
        /// </summary>
        /// <param name="mineralToUpdate"> The mineral to update</param>
        /// <returns> The amount of rows affected by the query. </returns>
        public int UpdateMineral(Mineral mineralToUpdate)
        {
            _rowsEffected = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();
                MySqlCommand updateMineral = new MySqlCommand("UPDATE minerals SET name = @newName, hardness = @newHardness, is_magnetic = @newMagnetic, acid_reaction = @newAcidReact, image = @newImg, streak_color = @newStreakColor WHERE mineral_id = @mineral_id", connection);
                updateMineral.Parameters.Add(new MySqlParameter("newName", mineralToUpdate.Name));
                updateMineral.Parameters.Add(new MySqlParameter("newHardness", mineralToUpdate.Hardness));
                updateMineral.Parameters.Add(new MySqlParameter("newMagnetic", mineralToUpdate.IsMagnetic));
                updateMineral.Parameters.Add(new MySqlParameter("newAcidReact", mineralToUpdate.AcidReaction));
                updateMineral.Parameters.Add(new MySqlParameter("newImg", mineralToUpdate.Image));
                updateMineral.Parameters.Add(new MySqlParameter("newStreakColor", mineralToUpdate.StreakColor));
                updateMineral.Parameters.Add(new MySqlParameter("mineral_id", mineralToUpdate.ID));
                _rowsEffected = updateMineral.ExecuteNonQuery();
                connection.Close();
            }
            return _rowsEffected;
        }

        /// <summary>
        /// Gets all minerals from database.
        /// </summary>
        public List<Mineral> GetMinerals()
        {
            List<Mineral> minerals = new List<Mineral>();
            using (MySqlConnection connection = new MySqlConnection(connectionStringToDB))
            {
                connection.Open();
                MySqlCommand getMinerals = new MySqlCommand("SELECT * FROM minerals", connection);
                MySqlDataReader reader = getMinerals.ExecuteReader();
                while (reader.Read())
                {
                    Mineral mineral = new Mineral();
                    mineral.ID = (int)reader["mineral_id"];
                    mineral.Name = reader["name"].ToString();
                    mineral.Hardness = (float)Convert.ToDouble(reader["hardness"]);
                    mineral.IsMagnetic = Convert.ToBoolean(reader["is_magnetic"]);
                    mineral.AcidReaction = Convert.ToBoolean(reader["acid_reaction"]);
                    mineral.StreakColor = (string)reader["streak_color"];
                    if (reader["image"] == DBNull.Value)
                    {
                        mineral.Image = null;
                    }
                    else
                    {
                        mineral.Image = (byte[])reader["image"];
                    }
                    minerals.Add(mineral);
                }
                connection.Close();
            }
            return minerals;
        }
    }
}
