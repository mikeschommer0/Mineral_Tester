using System.Collections.Generic;

/// <summary>
/// User and Mineral methods coded by Quinn Nimmer.
/// Question/Answer methods coded by Rick Bowman.
/// </summary>
namespace MineralTester.Classes
{
    /// <summary>
    /// Interface for the database
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// Method to check if a user does exist before attempting to pull user from DB.
        /// </summary>
        /// <param name="userName"> User name to check if exists.</param>
        /// <returns> True = exits | false = doesn't exist.</returns>
        public bool CheckUserExists(string userName);

        /// <summary>
        /// ASSUMES: That CheckUserExists has been ran and returned true prior to call.
        /// Method to pull user info from DB and create user to return.
        /// </summary>
        /// <param name="userName"> userName to get. </param>
        /// <returns> Returns the specified user. </returns>
        public User GetUser(string userName);

        /// <summary>
        /// Method to pull user info from DB and create users to return.
        /// </summary>
        /// <returns> List of users. </returns>
        public List<User> GetAllUsers();

        /// <summary>
        /// ASSUMES: That CheckUserExists has been ran and return false prior to call.
        /// UPDATES: _RowsEffected for testing.
        /// Takes in a user to insert into DB.
        /// </summary>
        /// <param name="newUser"> New User to add to DB. </param>
        /// <returns> An int that is equal to the rows affected. </returns>
        public int AddUser(User newUser);

        /// <summary>
        /// ASSUMES: That CheckUserExists has been ran and return true prior to call.
        /// UPDATES: _RowsEffected for testing.
        /// Takes in a user to update in DB.
        /// </summary>
        /// <param name="userToUpdate"> User to update in DB. </param>
        /// <returns> An int that is equal to the rows affected. </returns>
        public int UpdateUser(User userToUpdate);

        /// <summary>
        /// ASSUMES: That CheckUserExists has been ran and return true prior to call.
        /// UPDATES: _RowsEffected for testing.
        /// Takes in a user to delete from DB.
        /// </summary>
        /// <param name="userToDelete"> User to delete from DB. </param>
        /// <returns> An int that is equal to the rows affected. </returns>
        public int DeleteUser(User userToDelete);

        /// <summary>
        /// Gets a list of all questions from the database.
        /// </summary>
        /// <returns> Returns a list of all questions in the database if there are any, otherwise returns an empty list. </returns>
        public List<Question> GetQuestions();

        /// <summary>
        /// Inserts a new question into the database.
        /// </summary>
        /// <param name="description"> Description is the string representation of the question. </param>
        public void InsertQuestion(Question questionToInsert);

        /// <summary>
        /// Updates an existing question in the database.
        /// </summary>
        /// <param name="questionToUpdate"> The question to update. </param>
        public void UpdateQuestion(Question questionToUpdate);

        /// <summary>
        /// Deletes an existing question in the database.
        /// </summary>
        /// <param name="idToDelete"> Is the question id to delete. </param>
        public void DeleteQuestion(int idToDelete);

        /// <summary>
        /// Check mineral does not exist under same name in DB.
        /// </summary>
        /// <param name="mineralName"> The mineral to check. </param>
        /// <returns> <see langword="true"/>if the mineral exists, false otherwise. </returns>
        public bool CheckMineralExists(string mineralName);

        /// <summary>
        /// Adds a mineral to the database
        /// </summary>
        /// <param name="mineralToAdd"> The mineral to add. </param>
        public void AddMineral(Mineral mineralToAdd);

        /// <summary>
        /// Gets Mineral from db.
        /// </summary>
        /// <param name="mineralName"> The mineral to get. </param>
        /// <returns> The specified mineral. </returns>
        public Mineral GetMineral(string mineralName);

        /// <summary>
        /// Deletes a mineral in DB.
        /// </summary>
        /// <param name="mineralToDelete"> The mineral to delete. </param>
        /// <returns> The amount of rows affected by the query. </returns>
        public int UpdateMineral(Mineral mineralToUpdate);

        /// <summary>
        /// Updates a mineral entry in the database.
        /// </summary>
        /// <param name="mineralToUpdate"> The mineral to update</param>
        /// <returns> The amount of rows affected by the query. </returns>
        public int DeleteMineral(Mineral mineralToDelete);

        /// <summary>
        /// Gets all minerals from database.
        /// </summary>
        public List<Mineral> GetMinerals();
    }
}
