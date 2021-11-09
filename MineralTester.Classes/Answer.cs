namespace MineralTester.Classes
{
    public class Answer
    {
        public int AnswerID { get; set; }
        public string Description { get; set; }
        public bool IsCorrect { get; set; }

        public Answer()
        {

        }

        public Answer(int answerID, string description)
        {
            AnswerID = answerID;
            Description = description;
        }

        public Answer(int answerID, string description, bool isCorrect)
        {
            AnswerID = answerID;
            Description = description;
            IsCorrect = isCorrect;
        }
    }
}
