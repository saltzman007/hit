using System.IO;
using store;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System;

namespace test
{
    [TestClass]
    public class StoreTest
    {
        [TestMethod]
        public void GetBlob()
        {
            Persist persist = new Persist();

            byte[] content = Encoding.UTF8.GetBytes("sweet");

            byte[] blob = persist.GetBlob(content);

            byte[] sha1 = persist.GetSHA1(blob);


            string ist = BitConverter.ToString(sha1).Replace("-", "").ToLower();
            string soll = "aa823728ea7d592acc69b36875a482cdf3fd5c8d";

            Assert.AreEqual(soll, ist);
        }
    }
}
