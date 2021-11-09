using System.Collections.Generic;

namespace MineralTester.Classes
{
    public class Question
    {
        public int QuestionID { get; set; }
        public string Description { get; set; }

        public List<Answer> Answers { get; set; }

        public Question()
        {

        }

        public Question(string description)
        {
            Description = description;
        }

        public Question(int questionID, string description)
        {
            QuestionID = questionID;
            Description = description;
        }
    }
}
