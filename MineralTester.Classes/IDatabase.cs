using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineralTester.Classes
{
    public interface IDatabase
    {
        public bool CheckUserExists(string userName);

        public User GetUser(string userName);

        public void AddUser(User newUser);

        public void DeleteUser(User userToDelete);

        public List<Question> GetQuestions();

        public void InsertQuestion(string description);

        public void UpdateQuestion(int idToUpdate, string description);

        public void DeleteQuestion(int idToDelete);

        public List<Answer> GetAnswers();

        public void InsertAnswer(string description);

        public void UpdateAnswer(int idToUpdate, string description);

        public void DeleteAnswer(int idToDelete);

        public (bool isSuccess, string message) InsertQuestionAnswers(Question question);

        public void DeleteQuestionAnswers(int questionID);

        public List<Answer> GetQuestionAnswers(int questionID);

        public void AddMineral(Mineral toAdd);
    }
}
