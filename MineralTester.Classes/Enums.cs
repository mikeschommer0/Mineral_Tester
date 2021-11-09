using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineralTester.Classes
{
    public static class Enums
    {
        public enum AccountType
        {
            Teacher = 1,
            Assistant = 2,
            Student = 3
        }

        public enum QAEditMode
        {
            EditQuestions,
            EditAnswers
        }
    }
}
