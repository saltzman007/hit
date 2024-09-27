using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.IO.Compression;
using common;

//www-cs-students.stanford.edu/~blynn/gitmagic/ch08
namespace store
{
    public class Persist : IStore
    {
        public void Init()
        {
            DirectoryInfo directoryInfo = Directory.CreateDirectory($"{Constant.HitFolderName}");

            File.WriteAllText($"{Constant.HitFolderName}/HEAD", "ref: refs/heads/master");

            Directory.CreateDirectory($"{Constant.HitFolderName}/refs");
            Directory.CreateDirectory($"{Constant.HitFolderName}/refs/heads");
            Directory.CreateDirectory($"{Constant.HitFolderName}/refs/tags");

            Directory.CreateDirectory($"{Constant.HitFolderName}/objects");
    
            Console.WriteLine($"Initialized empty Hit repository in {directoryInfo.FullName}");
        }

        public string WriteStoreFile(string filename)
        {
            byte[] content = File.ReadAllBytes(filename);
            byte[] blob = GetBlob(content);
            byte[] sha1 = GetSHA1(blob);

            string strSha1 = BitConverter.ToString(sha1).Replace("-", "").ToLower();
            string dirName = strSha1.Substring(0, 2);
            string blobFileName = strSha1.Substring(2);

            DirectoryInfo zipPath = Directory.CreateDirectory(Path.Combine(Constant.HitFolderName, Constant.ObjectFolderName, dirName));
            string zipfilename = Path.Combine(zipPath.FullName, blobFileName);

            WriteCompressed(content, zipfilename);

            return strSha1;
        }

        public byte[] ReadStoreFile(string sha1)
        {
            if (sha1.Length != 40)
                throw new ArgumentOutOfRangeException($"Länge sha1 Error {sha1}");

            string compressedFileName = Path.Combine(Constant.HitFolderName, Constant.ObjectFolderName, sha1.Substring(0, 2), sha1.Substring(2));

            using (FileStream compressedFileStream = File.Open(compressedFileName, FileMode.Open))
            {
                using (MemoryStream outstream = new MemoryStream())
                {
                    using (GZipStream decompressor = new GZipStream(compressedFileStream, CompressionMode.Decompress))
                    {
                        decompressor.CopyTo(outstream);
                    }

                    return outstream.ToArray();
                }
            }
        }

        private void WriteCompressed(byte[] content, string targegtfilename)
        {
            //Das ist nur Inhalt ohne Filename!
            // zwei idenzische files werden nur einmal gespeichert
            if (File.Exists(targegtfilename))
                return;

            using (MemoryStream contentStream = new MemoryStream(content))
            {
                using (FileStream compressedFileStream = File.Create(targegtfilename))
                {
                    using (GZipStream compressor = new GZipStream(compressedFileStream, CompressionMode.Compress))
                    {
                        contentStream.CopyTo(compressor);
                    }
                }
            }
        }

        public byte[] GetBlob(byte[] content)
        {
            //"blob " + <size_of_file> + "\0" + <contents_of_file>
            byte[] blob = Encoding.UTF8.GetBytes($"blob {content.Length + 1}")
                .Concat(new byte[] { 0x00 })
                .Concat(content)
                .Concat(new byte[] { 0x0A })
                .ToArray();

            return blob;
        }
        public byte[] GetSHA1(byte[] content)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                return sha1.ComputeHash(content);
            }
        }
    }
}
