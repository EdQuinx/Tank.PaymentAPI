using Rijndael256;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Tank.PaymentAPI.Helpers
{
    public static class BaseInterface
    {
        public static string GetMD5(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder sbHash = new StringBuilder();

            foreach (byte b in bHash)
            {
                sbHash.Append(String.Format("{0:x2}", b));
            }
            return sbHash.ToString();
        }

        public static string AESEncrypt(string content)
        {
            string EncryptText = RijndaelEtM.Encrypt(content, "GunBeyond", KeySize.Aes256);
            return EncryptText;
        }

        public static string AESDecrypt(string content)
        {
            string DecryptText = RijndaelEtM.Decrypt(content, "GunBeyond", KeySize.Aes256);
            return DecryptText;
        }

        public static string RequestContent(string url)//request web
        {
            HttpClient httpClient = HttpClientFactory.Create();
            return httpClient.GetStringAsync(url).Result;
        }
    }
}
