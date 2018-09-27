using System.IO;
using System.Net;

namespace ZF.WebSite.Models
{
    public  class RequestUtility
    {
        public static string GetResponseString(string url, CookieContainer cookieContainer)
        {
            string returnValue = "";
            HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create(url);
            if (cookieContainer != null)
            {
                myWebRequest.CookieContainer = cookieContainer;
            }
            HttpWebResponse myWebResponse = (HttpWebResponse)myWebRequest.GetResponse();
            if (cookieContainer != null)
            {
                myWebResponse.Cookies = cookieContainer.GetCookies(myWebResponse.ResponseUri);
            }
            using (StreamReader myStreamReader = new StreamReader(myWebResponse.GetResponseStream(), System.Text.Encoding.UTF8))
            {
                returnValue = myStreamReader.ReadToEnd();
            }
            return returnValue;
        }
    }
}
