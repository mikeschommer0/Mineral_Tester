using System;
using System.Collections.Generic;

namespace MineralTester.Classes
{
    public interface IDatabase
    {
        public bool CheckUserExists(string userName);

        public User GetUser(string userName);

        public List<User> GetAllUsers();

        public int AddUser(User newUser);

        public int UpdateUser(User userToUpdate);

        public int DeleteUser(User userToDelete);

        public List<Question> GetQuestions();

        public void InsertQuestion(Question question);

        public void UpdateQuestion(Question question);

        public void DeleteQuestion(int idToDelete);

        public bool CheckMineralExists(string mineralName);

        public void AddMineral(Mineral toAdd);

        public Mineral GetMineral(String mineralName);

        public int UpdateMineral(Mineral newMineralInfo);

        public int DeleteMineral(Mineral mineralToDelete);

        public List<Mineral> GetMinerals();
    }
}
