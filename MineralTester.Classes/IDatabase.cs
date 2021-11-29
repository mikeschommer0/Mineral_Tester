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

        public int AddUser(User newUser);

        public int UpdateUser(User userToUpdate);
        
        public int DeleteUser(User userToDelete);

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

        public bool CheckMineralExists(string mineralName);

        public void AddMineral(Mineral toAdd);

        public Mineral GetMineral(String mineralName);

        public int UpdateMineral(Mineral newMineralInfo);

        public int DeleteMineral(Mineral mineralToDelete);

        public List<Mineral> GetMinerals();
    }
}
