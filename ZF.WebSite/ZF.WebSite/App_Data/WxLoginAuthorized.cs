using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ZF.Infrastructure.WxLogin
{

    /// <summary>
    /// 微信授权拉取用户信息
    /// </summary>
    public class WxLoginAuthorized
    {
        private readonly string appId = ConfigurationManager.AppSettings["WxLoginAppId"];
        private readonly string secret = ConfigurationManager.AppSettings["WxLoginSecret"];

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public WxUserInfo GetWikiUserInfo(string code)
        {
            WxAccessTokenInfo accessTokenInfo = GetAccessToken(code);
            WxUserInfo userInfo = new WxUserInfo();
            //Code无效错误
            if (accessTokenInfo.errcode == 40029)
            {
                return userInfo;
            }
            else
            {
                //拉取用户数据
                //检验授权凭证（access_token）是否有效
                if (InspectionAccessToken(accessTokenInfo))
                {
                    //拉取用户数据
                    userInfo = PullWxUserInfo(accessTokenInfo);
                }
                return userInfo;
            }
        }

        /// <summary>
        /// 用户授权之后获取accessToken
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public WxAccessTokenInfo GetAccessToken(string code)
        {
            //获取access_token
            try
            {
                string accessToken = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={1}&secret={2}&code={0}&grant_type=authorization_code", code, appId, secret);
                WebClient wc = new WebClient();
                Encoding enc = Encoding.GetEncoding("UTF-8");
                Byte[] pageData = wc.DownloadData(accessToken);
                string re = enc.GetString(pageData);
                //WebClient client = new WebClient();
                //client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                //string strResult = Encoding.GetEncoding("UTF-8").GetString(client.DownloadData(accessToken));
                JavaScriptSerializer js = new JavaScriptSerializer();
                WxAccessTokenInfo info = js.Deserialize<WxAccessTokenInfo>(re);
                return info;
            }
            catch (Exception ex)
            {

                return null;
            }
        }


        /// <summary>
        /// 检验授权凭证（access_token）是否有效
        /// </summary>
        /// <returns></returns>
        public bool InspectionAccessToken(WxAccessTokenInfo info)
        {
            string url = string.Format(" https://api.weixin.qq.com/sns/auth?access_token={0}&openid={1}", info.access_token, info.openid);
            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            string strResult = Encoding.GetEncoding("UTF-8").GetString(client.DownloadData(url));
            JavaScriptSerializer js = new JavaScriptSerializer();
            MessageInput msg = js.Deserialize<MessageInput>(strResult);
            if (msg.errmsg == "ok")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 拉取微信用户信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public WxUserInfo PullWxUserInfo(WxAccessTokenInfo info)
        {
            try
            {
                string url = string.Format(" https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN ", info.access_token, info.openid);
                WebClient client = new WebClient();
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                string strResult = Encoding.GetEncoding("UTF-8").GetString(client.DownloadData(url));
                JavaScriptSerializer js = new JavaScriptSerializer();
                WxUserInfo userInfo = js.Deserialize<WxUserInfo>(strResult);
                return userInfo;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    [Serializable]
    public class WxAccessTokenInfo
    {
        /// <summary>
        /// 网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// access_token接口调用凭证超时时间，单位（秒）
        /// </summary>
        public int expires_in { get; set; }
        /// <summary>
        /// 用户刷新access_token
        /// </summary>
        public string refresh_token { get; set; }
        /// <summary>
        /// 用户唯一标识，请注意，在未关注公众号时，用户访问公众号的网页，也会产生一个用户和公众号唯一的OpenID
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 用户授权的作用域，使用逗号（,）分隔
        /// </summary>
        public string scope { get; set; }
        /// <summary>
        /// 调用失败错误码
        /// </summary>
        public int errcode { get; set; }
        /// <summary>
        /// 调用失败错误信息
        /// </summary>
        public string errmsg { get; set; }
    }

    [Serializable]
    public class WxUserInfo
    {
        public string openid { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string nickname { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string sex { get; set; }
        /// <summary>
        /// 用户个人资料填写的省份
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 普通用户个人资料填写的城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 国家，如中国为CN
        /// </summary>
        public string country { get; set; }
        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空。若用户更换头像，原有头像URL将失效
        /// </summary>
        public string headimgurl { get; set; }

        public string[] privilege { get; set; }

        /// <summary>
        /// 调用失败错误码
        /// </summary>
        public int errcode { get; set; }
        /// <summary>
        /// 调用失败错误信息
        /// </summary>
        public string errmsg { get; set; }
    }

    [Serializable]
    public class MessageInput
    {
        public int errcode { get; set; }

        public string errmsg { get; set; }
    }
}
