namespace MineralTester.Classes
{
    public class Answer
    {
        public string Description { get; set; }
        public bool IsCorrect { get; set; }

        public Answer()
        {

        }

        public Answer(string description, bool isCorrect)
        {
            Description = description;
            IsCorrect = isCorrect;
        }
    }
}
