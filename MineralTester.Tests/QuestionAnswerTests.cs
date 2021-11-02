using Microsoft.VisualStudio.TestTools.UnitTesting;
using MineralTester.Classes;
using System.Collections.Generic;

namespace MineralTester.Tests
{
    [TestClass]
    public class QuestionAnswerTests
    {
        [TestMethod]
        public void LoadAnswerTests()
        {
            Database database = new Database();
            List<Answer> answers = database.GetAnswers(1);
            Assert.AreEqual(4, answers.Count);
        }
    }
}
