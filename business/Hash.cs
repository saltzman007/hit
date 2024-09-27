using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace business
{
    public class Hash
    {
        static SHA256 sha = SHA256.Create();
        static SHA256Managed sm = new();
        public static byte[] GetHash(object obj)
        {
            return sm.ComputeHash(Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(obj)));
        }
        public string GetStringFromHash(byte[] hash)
        {
            string hashString = "";
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;
        }

        public static bool Identic(byte[] hash1, byte[] hash2)
        {
            if(hash1 == null || hash2 == null) 
                return false;

            if (hash1.Length != hash2.Length)
                return false;

            for (int i = 0; i < hash1.Length; i++)
                if (hash1[i] != hash2[i])
                    return false;

            return true;
        }
    }
}
