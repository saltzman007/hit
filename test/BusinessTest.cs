using System.IO;
using business;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test
{
    [TestClass]
    public class BusinessTest
    {
        [TestMethod]
        public void Init()
        {
            Hit.init(new string[]{"init"});
            //Assert.IsTrue(Directory.Exists(".hit"));
        }
    }
}
