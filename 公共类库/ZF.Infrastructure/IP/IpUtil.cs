using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ZF.Infrastructure.IP
{
   public static class IpUtil
    {
        /// 获取客户端IP地址（无视代理）
        /// </summary>
        /// <returns>若失败则返回回送地址</returns>
        public static string GetHostAddress( )
        {
            string userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if ( string.IsNullOrEmpty( userHostAddress ) )
            {
                if ( System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null )
                    userHostAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString( ).Split( ',' )[0].Trim( );
            }
            if ( string.IsNullOrEmpty( userHostAddress ) )
            {
                userHostAddress = HttpContext.Current.Request.UserHostAddress;
            }

            //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
            if ( !string.IsNullOrEmpty( userHostAddress ) && IsIP( userHostAddress ) )
            {
                return userHostAddress;
            }
            return "127.0.0.1";
        }

        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP( string ip )
        {
            return System.Text.RegularExpressions.Regex.IsMatch( ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$" );
        }

        public static string GetIP()
        {
            string tempip = "";
            try
            {
                WebRequest wr = WebRequest.Create("http://www.3322.org/dyndns/getip");
                Stream s = wr.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.Default);
                string all = sr.ReadToEnd(); //读取网站的数据
                tempip = all.Replace("\n", "").Trim();
                sr.Close();
                s.Close();
            }
            catch
            {
            }
            return tempip;
        }
    }
}
