using Microsoft.VisualStudio.TestTools.UnitTesting;
using MineralTester.UI;

namespace MineralTester.Tests
{
    [TestClass]
    public class WindowTest
    {
        [TestMethod]
        public void TestMineralWindow()
        {
            Assert.IsTrue((bool)AddMineralWindow.IsEnabledProperty.DefaultMetadata.DefaultValue);
            Assert.IsFalse((bool)AddMineralWindow.IsActiveProperty.DefaultMetadata.DefaultValue);
        }

        [TestMethod]
        public void TestAdminWindow()
        {
            Assert.IsTrue((bool)AdminWindow.IsEnabledProperty.DefaultMetadata.DefaultValue);
            Assert.IsFalse((bool)AdminWindow.IsActiveProperty.DefaultMetadata.DefaultValue);
        }
    }
}
