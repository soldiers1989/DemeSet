using System.Security.Cryptography;

namespace ZF.Infrastructure.Md5
{
    public class Md5
    {
        public static string GetMd5(string str)
        {
            byte[] b = System.Text.Encoding.Default.GetBytes(str);

            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
            {
                ret += b[i].ToString("x").PadLeft(2, '0');
            }
            return ret;
        }
    }
}