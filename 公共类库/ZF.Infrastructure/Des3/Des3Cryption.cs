using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ZF.Infrastructure.Des3
{
    public class Des3Cryption
    {
        public Des3Cryption()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        //从配置文件获取密钥
        private static readonly string _key = System.Configuration.ConfigurationManager.AppSettings["DESKEY"];

        /// <summary>
        /// （KEY的）字符串处理（保证KEY的长度是24位）
        /// 不足24位使用A字符补充
        /// </summary>
        /// <param name="argKey">密钥</param>
        /// <returns>返回 24位字符密钥</returns>
        private static string DealKeyString(string argKey)
        {
            int length = argKey.Length;
            string strResult = String.IsNullOrEmpty(argKey) ? "" : argKey.Replace(" ", "").Trim();
            if (length > 24)
            {
                strResult = argKey.Substring(0, 24);
            }
            else if (length < 24)
            {
                string str = "OpQrStUvWxAbCdEfGhIjKlMn";
                strResult += str.Substring(0, 24 - length);
            }

            return strResult;
        }

        #region 【DES3加密解密】
        /// <summary>
        /// DES3加密
        /// </summary>
        /// <param name="argString">需要加密的字符串</param>
        /// <returns></returns>
        public static string Encrypt3DES(string argString)
        {
            if (String.IsNullOrEmpty(argString)) return "";
            return Encrypt3DES(argString, _key);
        }
        /// <summary>
        /// DES3加密
        /// </summary>
        /// <param name="argString">需要加密的字符串</param>
        /// <param name="argKey">密钥</param>
        /// <returns></returns>
        public static string Encrypt3DES(string argString, string argKey)
        {
            if (String.IsNullOrEmpty(argString)) return "";
            //处理密钥字符串
            argKey = DealKeyString(argKey);
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();

            DES.Key = System.Text.Encoding.Default.GetBytes(argKey);
            DES.Mode = CipherMode.ECB;
            DES.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

            ICryptoTransform DESEncrypt = DES.CreateEncryptor();

            byte[] Buffer = ASCIIEncoding.UTF8.GetBytes(argString);

            return HttpServerUtility.UrlTokenEncode(DESEncrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
        }
        /// <summary>
        /// DES3解密
        /// </summary>
        /// <param name="a_strString">需要解密的字符串</param>
        /// <returns></returns>
        public static string Decrypt3DES(string argString)
        {
            if (String.IsNullOrEmpty(argString)) return "";
            return Decrypt3DES(argString, _key);
        }
        /// <summary>
        /// DES3解密
        /// </summary>
        /// <param name="a_strString">需要解密的字符串</param>
        /// <param name="a_strKey">密钥</param>
        /// <returns></returns>
        public static string Decrypt3DES(string argString, string argKey)
        {
            if (String.IsNullOrEmpty(argString)) return "";
            //处理密钥字符串
            argKey = DealKeyString(argKey);
            TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();

            DES.Key = System.Text.Encoding.Default.GetBytes(argKey);
            DES.Mode = CipherMode.ECB;
            DES.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

            ICryptoTransform DESDecrypt = DES.CreateDecryptor();

            string result = "";
            try
            {
                byte[] Buffer = HttpServerUtility.UrlTokenDecode(argString);
                //byte[] Buffer = Convert.FromBase64String(argString.Replace(" ", "+"));
                result = ASCIIEncoding.UTF8.GetString(DESDecrypt.TransformFinalBlock(Buffer, 0, Buffer.Length));
            }
            catch (Exception e)
            {

            }

            return result;
        }
        #endregion


    }

}