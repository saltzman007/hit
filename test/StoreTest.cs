using System.IO;
using store;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System;
using business;
using System.Runtime.CompilerServices;
using common;

namespace test
{
    [TestClass]
    public class StoreTest
    {
        const string Testfilename0 = "test0.txt";
        const string Testfilename1 = "test1.txt";
        const string Testfilename2 = "test2.txt";

        string Content0;
        string Content1;
        string Content2;

        [TestInitialize]
        public void Init()
        {
            if (Directory.Exists($"{Constant.HitFolderName}"))
                Directory.Delete($"{Constant.HitFolderName}", true);
            if (Directory.Exists($"{Constant.GitFolderName}"))
                Directory.Delete($"{Constant.GitFolderName}", true);

            Content0 = Guid.NewGuid().ToString();
            Content1 = Guid.NewGuid().ToString();
            Content2 = Guid.NewGuid().ToString();

            File.WriteAllBytes(Testfilename0, Encoding.UTF8.GetBytes(Content0));
            File.WriteAllBytes(Testfilename1, Encoding.UTF8.GetBytes(Content1));
            File.WriteAllBytes(Testfilename2, Encoding.UTF8.GetBytes(Content2));
        }

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

        [TestMethod]
        public void ReadWriteIntegrity()
        {
            Persist persist = new Persist();

            persist.Init();

            string sha1 = persist.WriteStoreFile(Testfilename0);

            byte[] contentRead = persist.ReadStoreFile(sha1);

            string strContentRead = Encoding.UTF8.GetString(contentRead);

            Assert.AreEqual(Content0, strContentRead);
        }


        [TestMethod]
        public void GitCompatibility()
        {
            Persist persist = new Persist();

            persist.Init();

            string sha1 = persist.WriteStoreFile(Testfilename0);

            Git.Run("init");
            Git.Run("add test0.txt");

            string gitFileName = $"{Path.Combine(Constant.GitFolderName, Constant.ObjectFolderName, sha1.Substring(0,2), sha1.Substring(2))}";
            string hitFileName = gitFileName.Replace(Constant.GitFolderName, Constant.HitFolderName);

            Assert.IsTrue(BitConverter.ToString(File.ReadAllBytes(gitFileName))
                    .Equals(BitConverter.ToString(File.ReadAllBytes(hitFileName))));
        }
    }
}
