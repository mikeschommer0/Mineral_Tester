/// <summary>
/// Written by: Rick Bowman & Mike Schommer
/// </summary>

namespace MineralTester.Classes
{
    /// <summary>
    /// Class containing enumerations used throughout the program.
    /// </summary>
    public static class Enums
    {
        /// <summary>
        /// Represents the account type of the current user, different accounts have different permissions.
        /// </summary>
        public enum AccountType
        {
            Teacher = 1,
            Assistant = 2,
            Student = 3
        }

        /// <summary>
        /// Represents which type of test a student is doing within the mineral playground.
        /// </summary>
        public enum TestType
        {
            Scratch = 1,
            Magnestism = 2,
            AcidReaction = 3,
            Streak = 4
        }
    }
}
