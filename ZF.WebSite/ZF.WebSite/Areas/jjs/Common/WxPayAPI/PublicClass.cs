using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ZF.WebSite.Areas.jjs.Common.WxPayAPI
{
    public static class PublicClass
    {

        public static string GenSign(IDictionary<string, object> data, string sign_type, string key)
        {
            var sign = string.Join("&", data.Where(m => !m.Key.Contains("sign") && m.Value != null && (m.Value as string) != "").OrderBy(m => m.Key).Select(m => m.Key + "=" + m.Value)) + "&key=" + key;
            return sign_type == "HMAC-SHA256" ? HashSHA256(sign) : HashMD5(sign);
        }

        public static string HashSHA256(this string s)
        {
            using (var sha = SHA256.Create())
            {
                var data = sha.ComputeHash(Encoding.UTF8.GetBytes(s));
                var sb = new StringBuilder();
                foreach (var i in data) sb.Append(i.ToString("X2"));
                return sb.ToString();
            }
        }

        public static string HashMD5(this string s)
        {
            using (var md5 = MD5.Create())
            {
                var data = md5.ComputeHash(Encoding.UTF8.GetBytes(s));
                var sb = new StringBuilder();
                foreach (var i in data) sb.Append(i.ToString("X2"));
                return sb.ToString();
            }
        }
    }
}