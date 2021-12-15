/// <summary>
/// Written by: Rick Bowman
/// </summary>

namespace MineralTester.Classes
{
    /// <summary>
    /// Class representing an answer to a question.
    /// </summary>
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
