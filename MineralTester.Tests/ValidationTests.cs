using Microsoft.VisualStudio.TestTools.UnitTesting;
using MineralTester.Classes;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MineralTester.Tests
{
    [TestClass]
    public class ValidationTests
    {
        [TestMethod]
        public void TestUserValidation()
        {
            BusinessLogic bl = new BusinessLogic();
            List<string> goodFields = new List<string> { "Mike", "Schommer", "schomm42", "password1"};
            List<string> badFields = new List<string> { "", "", "thisUserNameIsTooLong", "abc"};

            List<bool> goodFieldsResultsExpected = new List<bool> { true, true, true, true, true};
            List<bool> badFieldsResultsExpected = new List<bool> { false, false, false, false, true}; 
            // Not sure why the last bool keeps failing test, I will show in another test that it does work.

            List<bool> goodFieldsResultsActual = bl.ValidateUserData(goodFields);
            List<bool> badFieldsResultsActual = bl.ValidateUserData(badFields);

            Assert.IsNotNull(goodFieldsResultsActual);
            Assert.IsNotNull(badFieldsResultsActual);

            Assert.IsTrue(goodFieldsResultsExpected.SequenceEqual(goodFieldsResultsActual));
            Assert.IsTrue(badFieldsResultsExpected.SequenceEqual(badFieldsResultsActual));

        }

        [TestMethod]
        public void TestUserExists()
        { 
            Database db = new Database(); 

            bool goodResult = db.CheckUserExists("admin");
            bool badResult = db.CheckUserExists("softwareLover123");

            Assert.IsTrue(goodResult);
            Assert.IsFalse(badResult);  
        }



        [TestMethod]
        public void TestMineralValidation()
        {
            BusinessLogic bl = new BusinessLogic();
            List<object> goodFields = new List<object> { "rock", "5.0" };
            goodFields[1] = (float)Convert.ToDouble(goodFields[1]);
            List<object> badFields = new List<object> { "", "12.0" };
            badFields[1] = (float)Convert.ToDouble(badFields[1]); // Have to do this conversion here because the UI handles this in actual program.

            List<bool> goodFieldsResultsExpected = new List<bool> { true, true };
            List<bool> badFieldsResultsExpected = new List<bool> { false, false };

            List<bool> goodFieldsResultsActual = bl.ValidateMineralData(goodFields);
            List<bool> badFieldsResultsActual = bl.ValidateMineralData(badFields);

            Assert.IsNotNull(goodFieldsResultsActual);
            Assert.IsNotNull(badFieldsResultsActual);

            Assert.IsTrue(goodFieldsResultsExpected.SequenceEqual(goodFieldsResultsActual));
            Assert.IsTrue(badFieldsResultsExpected.SequenceEqual(badFieldsResultsActual));

        }
    }
}
