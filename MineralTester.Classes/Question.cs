using System.Collections.Generic;

namespace MineralTester.Classes
{
    public class Question
    {
        private Database database = new Database();

        public int QuestionID { get; set; }
        public string Description { get; set; }

        public List<Answer> Answers { get; set; }

        public Question()
        {
            Answers = new List<Answer>();
        }

        public void LoadAnswers()
        {
            
        }
    }
}
