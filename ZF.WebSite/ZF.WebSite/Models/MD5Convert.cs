using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace ZF.WebSite.Models
{
    public class MD5Convert
    {
        public static string GetMD5(string inputStr)
        {
            return MD5Convert.Get32Md5Str(inputStr);
        }
        public static string Get16Md5Str(string convertString)
        {
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            string text = BitConverter.ToString(mD5CryptoServiceProvider.ComputeHash(Encoding.Default.GetBytes(convertString)), 4, 8);
            text = text.Replace("-", "");
            return FormsAuthentication.HashPasswordForStoringInConfigFile(convertString, "MD5").ToLower().Substring(8, 16);
        }
        private static string Get32Md5Str(string convertString)
        {
            string str = "";
            MD5 mD = MD5.Create();
            byte[] array = mD.ComputeHash(Encoding.UTF8.GetBytes(convertString));
            for (int i = 0; i < array.Length; i++)
            {
                str += array[i].ToString("X");
            }
            return FormsAuthentication.HashPasswordForStoringInConfigFile(convertString, "MD5");
        }
    }
}
