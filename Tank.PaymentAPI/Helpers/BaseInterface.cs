using Rijndael256;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Tank.PaymentAPI.Helpers
{
    public class BaseInterface
    {
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        public static DateTime ConvertDateTime(dynamic needConvert)
        {
            string[] splitDateTimes = Convert.ToString(needConvert).Split(' ');//0 - date, 1 - time
            string[] splitDates = splitDateTimes[0].Split('/');
            string[] splitTimes = splitDateTimes[1].Split(':');
            string completeString = string.Format("{0}-{1}-{2} {3}", splitDates[2], splitDates[1], splitDates[0], splitDateTimes[1]);
            DateTime dateTime = new DateTime();
            DateTime.TryParse(completeString, out dateTime);
            return dateTime;//DateTime.ParseExact(completeString, "G", CultureInfo.CurrentUICulture.DateTimeFormat);
        }
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
