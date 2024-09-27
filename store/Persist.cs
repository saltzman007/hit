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

        public void Store(object o)
        {

        }
        public object Load()
        {
            //if (!Fi)
            return null;
        }

        public void StoreFile(string filename)
        {
            byte[] content = File.ReadAllBytes(filename);
            byte[] blob = GetBlob(content);
            byte[] sha1 = GetSHA1(blob);

            string strSha1 = BitConverter.ToString(sha1).Replace("-", "").ToLower();
            string dirName = strSha1.Substring(0, 2);
            string blobFileName = strSha1.Substring(2);

            DirectoryInfo zipPath = Directory.CreateDirectory(Path.Combine(".hit", dirName));
            string zipfilename = Path.Combine(zipPath.FullName, blobFileName);

            WriteCompressed(content, zipfilename);
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
