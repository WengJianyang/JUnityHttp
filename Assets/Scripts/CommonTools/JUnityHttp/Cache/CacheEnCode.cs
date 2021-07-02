using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CommonTools.JUnityHttp.Cache
{
    /// <summary>
    /// Encrypt and Decrypt
    /// </summary>
    public class CacheEnCode
    {
        public static string EncodeKey = "Cach";


        public static string Encrypt(string str)
        {
            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider(); 
            byte[] key = Encoding.Unicode.GetBytes(EncodeKey); 
            byte[] data = Encoding.Unicode.GetBytes(str); 
            MemoryStream MStream = new MemoryStream();
            CryptoStream
                CStream = new CryptoStream(MStream, descsp.CreateEncryptor(key, key),
                    CryptoStreamMode.Write); 
            CStream.Write(data, 0, data.Length); 
            CStream.FlushFinalBlock(); 
            return Convert.ToBase64String(MStream.ToArray());
        }

        public static string Decrypt(string str)
        {
            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider(); 
            byte[] key = Encoding.Unicode.GetBytes(EncodeKey);
            byte[] data = Convert.FromBase64String(str); 
            MemoryStream MStream = new MemoryStream(); 
           
            CryptoStream CStream = new CryptoStream(MStream, descsp.CreateDecryptor(key, key), CryptoStreamMode.Write);
            CStream.Write(data, 0, data.Length); 
            CStream.FlushFinalBlock(); 
            return Encoding.Unicode.GetString(MStream.ToArray());
        }
    }
}