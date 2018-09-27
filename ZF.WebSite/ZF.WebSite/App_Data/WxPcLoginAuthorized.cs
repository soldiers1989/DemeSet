using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace ZF.WebSite.App_Data
{
    public class WxPcLoginAuthorized
    {
        private readonly string appId = ConfigurationManager.AppSettings["WxPCLoginAppId"];
        private readonly string secret = ConfigurationManager.AppSettings["WxPCLoginSecret"];


        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public WxPcUserInfo GetWikiUserInfo(string code)
        {
            WxPcAccessTokenInfo accessTokenInfo = GetAccessToken(code);
            WxPcUserInfo userInfo = new WxPcUserInfo();
            //Code无效错误
            if (!string.IsNullOrEmpty(accessTokenInfo.errmsg))
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
        public WxPcAccessTokenInfo GetAccessToken(string code)
        {
            //获取access_token
            try
            {
                string accessToken = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={1}&secret={2}&code={0}&grant_type=authorization_code", code, appId, secret);
                WebClient wc = new WebClient();
                Encoding enc = Encoding.GetEncoding("UTF-8");
                Byte[] pageData = wc.DownloadData(accessToken);
                string re = enc.GetString(pageData);
                JavaScriptSerializer js = new JavaScriptSerializer();
                WxPcAccessTokenInfo info = js.Deserialize<WxPcAccessTokenInfo>(re);
                //当接口调用正常，刷新accesstoken, refresh_token拥有较长的有效期（30天），当refresh_token失效的后，需要用户重新授权。
                if (string.IsNullOrEmpty(info.errmsg))
                {
                    accessToken = string.Format("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={1}&grant_type=refresh_token&refresh_token={0}",info.refresh_token, appId);
                    pageData = wc.DownloadData(accessToken);
                    re = enc.GetString(pageData);
                    info = js.Deserialize<WxPcAccessTokenInfo>(re);
                }
                return info;
            }
            catch (Exception ex)
            {

                return null;
            }
        }


        public WxPcAccessTokenInfo GetAccessToken(string code,string token)
        {
            //获取access_token
            try
            {
                string accessToken = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={1}&secret={2}&code={0}&grant_type=authorization_code", code, appId, secret);
                WebClient wc = new WebClient();
                Encoding enc = Encoding.GetEncoding("UTF-8");
                Byte[] pageData = wc.DownloadData(accessToken);
                string re = enc.GetString(pageData);
                JavaScriptSerializer js = new JavaScriptSerializer();
                WxPcAccessTokenInfo info = js.Deserialize<WxPcAccessTokenInfo>(re);
                //当接口调用正常，刷新accesstoken, refresh_token拥有较长的有效期（30天），当refresh_token失效的后，需要用户重新授权。
                if (string.IsNullOrEmpty(info.errmsg))
                {
                    accessToken = string.Format("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={1}&grant_type=refresh_token&refresh_token={0}", info.refresh_token, appId);
                    pageData = wc.DownloadData(accessToken);
                    re = enc.GetString(pageData);
                    info = js.Deserialize<WxPcAccessTokenInfo>(re);
                }
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
        public bool InspectionAccessToken(WxPcAccessTokenInfo info)
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
        public WxPcUserInfo PullWxUserInfo(WxPcAccessTokenInfo info)
        {
            try
            {
                string url = string.Format(" https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN ", info.access_token, info.openid);
                WebClient client = new WebClient();
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                string strResult = Encoding.GetEncoding("UTF-8").GetString(client.DownloadData(url));
                JavaScriptSerializer js = new JavaScriptSerializer();
                WxPcUserInfo userInfo = js.Deserialize<WxPcUserInfo>(strResult);
                return userInfo;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    [Serializable]
    public class WxPcAccessTokenInfo
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
    public class WxPcUserInfo
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

        /// <summary>
        /// 用户IP
        /// </summary>
        public string userip { get; set; }

        /// <summary>
        /// 只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段。唯一标识
        /// </summary>
        public string unionid { get; set; }

    }

    [Serializable]
    public class MessageInput
    {
        public int errcode { get; set; }

        public string errmsg { get; set; }
    }
}