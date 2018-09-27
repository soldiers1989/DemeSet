using System;

namespace ZF.WebSite.Models
{
    [Serializable]
    public class AccessToken
    {
        /// <summary>
        /// 获取到的凭证
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        public int expires_in { get; set; }

        public override bool Equals(object obj)
        {
            if (obj != null)
            {
                AccessToken a = obj as AccessToken;
                if (a != null && !string.IsNullOrEmpty(a.access_token) && !string.IsNullOrEmpty(this.access_token))
                {
                    return string.Compare(a.access_token, this.access_token, true) == 0;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public DateTime CreateDate { get; set; }
    }

    /// <summary>
    /// 通过code换取网页授权access_token
    /// </summary>
    [Serializable]
    public class WebAccessToken : AccessToken
    {
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
    }


    /// <summary>
    /// 通过access_token获取JSSDK签名
    /// </summary>
    [Serializable]
    public class ApiTicket
    {
        public int errcode { get; set; }

        public string errmsg { get; set; }

        /// <summary>
        /// 获取到的凭证
        /// </summary>
        public string ticket { get; set; }

        /// <summary>
        /// 凭证有效时间，单位：秒
        /// </summary>
        public int expires_in { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
