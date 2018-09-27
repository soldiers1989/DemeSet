using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using log4net;
using ZF.WebSite.Models.Entity;

namespace ZF.WebSite.Models
{
    public class WeiXinServiceManager
    {
        private static readonly WeiXinApi _weixin = new WeiXinApi();

        public const string snsapi_userinfoUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=123";

        public const string snsapi_baseUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state=123";

        private const string weixinUserUrl = "https://api.weixin.qq.com/cgi-bin/user/get?access_token=";
        private static string tokenUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
        private const string weixinUserInfoUrl = "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN";
        private const string weixinGroupUrl = "https://api.weixin.qq.com/cgi-bin/groups/create?access_token={0}";
        private const string weixinGroupGetUrl = "https://api.weixin.qq.com/cgi-bin/groups/get?access_token={0}";
        private const string weixinSendUrl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}";
        private const string weixinMenuUrl = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}";
        private const string weixinMenuGetUrl = "https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}";
        private const string weixinMenuDelete = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}";
        private const string weixinGetMedia = "http://file.api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}";
        private const string weixinUploadMedia = "http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}";
        private const string weixinUpdateRemarkName = "https://api.weixin.qq.com/cgi-bin/user/info/updateremark?access_token={0}";
        private const string weixinUpdateGroupName = "https://api.weixin.qq.com/cgi-bin/groups/update?access_token={0}";
        private const string weixinMoveUser = "https://api.weixin.qq.com/cgi-bin/groups/members/update?access_token={0}";
        private const string weixinBatchUpdateUser = "https://api.weixin.qq.com/cgi-bin/groups/members/batchupdate?access_token={0}";
        private const string weixinTemplateUrl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";

        //发送图文消息素材
        private const string weixinUploadNews = "https://api.weixin.qq.com/cgi-bin/media/uploadnews?access_token={0}";
        //高级群发
        private const string weixinMultiSendUrl = "https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token={0}";


        private const string pageUrl = "https://mp.weixin.qq.com/cgi-bin/contactmanage?t=user/index&pagesize=1&pageidx=0&type=0&groupid=0&token={0}&lang=zh_CN";

        //网页授权
        private static readonly string webTokeUrl = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&grant_type=authorization_code"
        , ConfigurationManager.AppSettings["WeiXinAccountAppId"], ConfigurationManager.AppSettings["WeiXinAccountSecret"]);//通过code换取网页授权access_token
        private static readonly string webUserInfo = string.Format("https://api.weixin.qq.com/sns/userinfo?openid={0}&lang=zh_CN", ConfigurationManager.AppSettings["WeiXinAccountAppId"]);//拉取用户信息(需scope为 snsapi_userinfo)

        //JS-SDK签名
        private static readonly string apiticketUrl = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi";

        private static AccessToken _accessToken = null;
        public static object lockObject = new object();

        private static ApiTicket _apiticket = null;
        public static object lockApiObj = new object();

        private string Token = string.Empty;
        private string Ticket = string.Empty;
        private static CookieContainer cookieContainer;

        public static string Access_token
        {
            get
            {
                lock (lockObject)
                {
                    if (_accessToken != null)
                    {
                        lock (lockObject)
                        {
                            double secords = DateTime.Now.Subtract(_accessToken.CreateDate).TotalSeconds;
                            if ((double)_accessToken.expires_in > secords)
                            {
                                return _accessToken.access_token;
                            }
                        }
                    }
                    _accessToken = _weixin.GetAccessToken(tokenUrl);
                    _accessToken.CreateDate = DateTime.Now;
                    return _accessToken.access_token;
                }
            }
        }

        public static string Api_ticket
        {
            get
            {
                lock (lockApiObj)
                {
                    if (_apiticket != null)
                    {
                        lock (lockApiObj)
                        {
                            double secords = DateTime.Now.Subtract(_apiticket.CreateDate).TotalSeconds;
                            if ((double)_apiticket.expires_in > secords)
                            {
                                return _apiticket.ticket;
                            }
                        }
                    }
                    _apiticket = GetTickect();
                    _apiticket.CreateDate = DateTime.Now;
                    return _apiticket.ticket;
                }
            }
        }

        /// <summary>
        /// 强制重置access_token
        /// </summary>
        public static bool IsAccess_tokenError(string errcode)
        {
            /*
             40001:获取access_token时AppSecret错误，或者access_token无效。请开发者认真比对AppSecret的正确性，或查看是否正在为恰当的公众号调用接口
             40014:不合法的access_token，请开发者认真比对access_token的有效性（如是否过期），或查看是否正在为恰当的公众号调用接口
             42001:access_token超时，请检查access_token的有效期，请参考基础支持-获取access_token中，对access_token的详细机制说明
             */
            if (errcode == "40001" || errcode == "40014" || errcode == "42001")
            {
                _accessToken = _weixin.GetAccessToken(tokenUrl);
                _accessToken.CreateDate = DateTime.Now;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 强制重置api_ticket
        /// </summary>
        public static bool IsApi_ticketError(int errcode)
        {
            if (errcode != 0)
            {
                _apiticket = GetTickect();
                _apiticket.CreateDate = DateTime.Now;
                return true;
            }
            return false;
        }

        static WeiXinServiceManager()
        {
            tokenUrl = string.Format(tokenUrl, ConfigurationManager.AppSettings["WeiXinAccountAppId"], ConfigurationManager.AppSettings["WeiXinAccountSecret"]);
        }

        /// <summary>
        /// 判断Token是否有效
        /// 模拟网页登陆的令牌
        /// </summary>
        /// <returns></returns>
        private static string GetToken()
        {
            string token = string.Empty;
            string userName = ConfigurationManager.AppSettings["WeiXinAccountLoginName"];
            string password = ConfigurationManager.AppSettings["WeiXinAccountLoginPassword"];

            password = MD5Convert.GetMD5(password);
            string verifyCode = string.Empty; // 验证码
            // 登录要发送的数据
            string padata = "username=" + userName + "&pwd=" + password + "&imgcode=&f=json";
            string url = "https://mp.weixin.qq.com/cgi-bin/login?lang=zh_CN ";//请求登录的URL
            try
            {
                CookieContainer cc = new CookieContainer();//接收缓存

                byte[] byteArray = Encoding.UTF8.GetBytes(padata); // 转化
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);  //新建一个WebRequest对象用来请求或者响应url
                webRequest.Referer = "https://mp.weixin.qq.com/cgi-bin/loginpage?lang=zh_CN&t=wxm2-login";
                webRequest.CookieContainer = cc;                                      //保存cookie  
                webRequest.Method = "POST";                                          //请求方式是POST
                webRequest.ContentType = "application/x-www-form-urlencoded";       //请求的内容格式为application/x-www-form-urlencoded
                webRequest.ContentLength = byteArray.Length;

                Stream newStream = webRequest.GetRequestStream();           //返回用于将数据写入 Internet 资源的 Stream。
                newStream.Write(byteArray, 0, byteArray.Length);    //写入参数
                newStream.Close();
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                StreamReader reader = new StreamReader(webResponse.GetResponseStream(), Encoding.Default);
                string jsonResult = reader.ReadToEnd();

                //此处用到了newtonsoft来序列化
                LoginResultN loginResult = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginResultN>(jsonResult);
                if (loginResult.base_resp.err_msg.Equals("ok"))
                {
                    token = loginResult.redirect_url.Split(new char[] { '&' })[2].Split(new char[] { '=' })[1].ToString();//取得令牌
                    cookieContainer = cc;
                    //GetWXMPInfo(cc, Token);
                }
            }
            catch (Exception er)
            {
                LogManager.GetLogger("").Error(er);
            }
            return token;
        }

        #region 用户管理

        /// <summary>
        /// 获取微信用户列表
        /// </summary>
        /// <returns></returns>
        public static WeiXinUserList GetWeiXinUserList()
        {
            return _weixin.GetWeiXinUserList(weixinUserUrl + Access_token);
        }

        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static WeiXinUser GetGetWeiXinUser(string id)
        {
            return _weixin.GetUserInfo(string.Format(weixinUserInfoUrl, Access_token, id));
        }

        /// <summary>
        /// 创建分组
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int CreateWeiXinGroup(string name)
        {
            return _weixin.CreateGroup(string.Format(weixinGroupUrl, Access_token), name);
        }

        /// <summary>
        /// 查询所有分组
        /// </summary>
        /// <returns></returns>
        public static WeiXinGroupList GetWeiXinGroups()
        {
            return _weixin.GetWeiXinGroupList(string.Format(weixinGroupGetUrl, Access_token));
        }

        /// <summary>
        /// 设置用户的备注名
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="remarkName"></param>
        /// <returns></returns>
        public static WeiXinReturnMessag UpdateRemarkName(string openId, string remarkName)
        {
            return _weixin.UpdateRemarkName(string.Format(weixinUpdateRemarkName, Access_token), openId, remarkName);
        }

        /// <summary>
        /// 修改用户组名
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="remarkName"></param>
        /// <returns></returns>
        public static WeiXinReturnMessag UpdateGroupName(string groupId, string groupName)
        {
            return _weixin.UpdateGroupName(string.Format(weixinUpdateGroupName, Access_token), groupId, groupName);
        }

        /// <summary>
        /// 批量移动用户到其它用户组
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="remarkName"></param>
        /// <returns></returns>
        public static WeiXinReturnMessag BatchMoveUser(List<string> openIdList, string groupId)
        {
            return _weixin.BatchMoveUser(string.Format(weixinBatchUpdateUser, Access_token), openIdList, groupId);
            //WeiXinReturnMessag ms =new WeiXinReturnMessag();
            //foreach (var item in openIdList)
            //{
            //  ms = _weixin.MoveUser(string.Format(weixinMoveUser, Access_token), item, groupId);
            //}
            //return ms;
        }

        public static string GetUserBaseByNow()
        {
            string result = string.Empty;
            string token = GetToken();
            string url = string.Format(pageUrl, token);

            string html = RequestUtility.GetResponseString(url, cookieContainer);

            int startindex = html.IndexOf("contacts\":") + 10;
            int endindex = html.IndexOf("}).contacts");
            string jsonUser = html.Substring(startindex, endindex - startindex);

            if (string.IsNullOrEmpty(jsonUser))
            {
                return null;
            }

            try
            {
                Newtonsoft.Json.Linq.JArray userArr = Newtonsoft.Json.Linq.JArray.Parse(jsonUser);
                if (userArr != null && userArr.Count > 0)
                {
                    result = userArr[0]["id"].ToString();
                }
            }
            catch (Exception er)
            {
              
                return null;
            }
            return result;
        }
        #endregion

        #region 发送消息

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static WeiXinReturnMessag SendTextMessage(string openId, string context)
        {
            TextMessage tm = new TextMessage();
            tm.touser = openId;
            tm.msgtype = "text";
            tm.text = new ContentClass() { content = context };
            return _weixin.SendTextMessage(string.Format(weixinSendUrl, Access_token), tm);
        }


        /// <summary>
        /// 发送图片消息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="openId"></param>
        /// <param name="mediaId"></param>
        public static WeiXinReturnMessag SendPicMessage(string url, string openId, string mediaid)
        {
            ImgMessage mess = new ImgMessage();
            mess.touser = openId;
            mess.msgtype = "image";
            mess.image = new MediaClass() { media_id = mediaid };
            return _weixin.SendPicMessage(string.Format(weixinSendUrl, Access_token), mess);
        }

        /// <summary>
        /// 发送语音消息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="openId"></param>
        /// <param name="mediaId"></param>
        public static WeiXinReturnMessag SendVioMessage(string url, string openId, string mediaid)
        {
            VoiMessage mess = new VoiMessage();
            mess.touser = openId;
            mess.msgtype = "voice";
            mess.voice.Add(new MediaClass() { media_id = mediaid });
            return _weixin.SendVioMessage(string.Format(weixinSendUrl, Access_token), mess);
        }

        /// <summary>
        /// 发送视频消息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="openId"></param>
        /// <param name="mediaId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        public static WeiXinReturnMessag SendVidMessage(string url, string openId, string mediaid, string title, string description)
        {
            VidMessage mess = new VidMessage();
            mess.touser = openId;
            mess.msgtype = "video";
            mess.video.Add(new VideoClass() { media_id = mediaid, title = title, description = description });
            return _weixin.SendVidMessage(string.Format(weixinSendUrl, Access_token), mess);
        }

        /// <summary>
        /// 发送图文消息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="mess"></param>
        public static WeiXinReturnMessag SendNewsMessage(string url, string openId, List<NewsClass> newsMessage)
        {
            NewsMessage mess = new NewsMessage();
            mess.touser = openId;
            mess.msgtype = "news";
            mess.news.articles = newsMessage;
            return _weixin.SendNewsMessage(string.Format(weixinSendUrl, Access_token), mess);
        }

        /// <summary>
        /// 发送图文消息（点击跳转到图文消息页面）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="mess"></param>
        public static WeiXinReturnMessag SendNewsMessage(string openId, string medicaId)
        {
            MpNews mess = new MpNews();
            mess.touser = openId;
            mess.msgtype = "mpnews";
            mess.mpnews = new MediaClass();
            mess.mpnews.media_id = medicaId;
            return _weixin.SendNewsMessage(string.Format(weixinSendUrl, Access_token), mess);
        }


        /// <summary>
        /// 发送音乐消息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="mess"></param>
        public static WeiXinReturnMessag SendMusicMessage(string url, string openId, List<NewsClass> newsMessage)
        {
            NewsMessage mess = new NewsMessage();
            mess.touser = openId;
            mess.msgtype = "news";
            mess.news.articles = newsMessage;

            return _weixin.SendNewsMessage(string.Format(weixinSendUrl, Access_token), mess);
        }

        #endregion

        #region 创建菜单

        /// <summary>
        /// 创建一级菜单
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static WeiXinReturnMessag CreateFirstMenu<T>(FirstMenu<T> m)
        {
            return _weixin.CreateFirstMenu<T>(string.Format(weixinMenuUrl, Access_token), m);
        }

        /// <summary>
        /// 创建一级菜单, 同时二级菜单
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static WeiXinReturnMessag CreateSecondMenu<T>(SecondMenu<T> m)
        {
            return _weixin.CreateSecondMenu(string.Format(weixinMenuUrl, Access_token), m);
        }

        public static WeiXinReturnMessag DeleteSecondMenu()
        {
            return _weixin.DeleteSecondMenu(string.Format(weixinMenuDelete, Access_token));
        }



        #endregion

        #region 多媒体

        public static Stream GetWeiXinMedia(string mediaId)
        {
            return _weixin.GetWeiXinMedia(string.Format(weixinGetMedia, Access_token, mediaId));
        }

        public static WeiXinUploadMediaResult UploadWeiXinMedia(string mediaType, byte[] m)
        {
            return _weixin.UploadWeiXinMedia(string.Format(weixinUploadMedia, Access_token, mediaType), m);
        }


        #endregion


        public static WeiXInMediaTypeEnum GetMsgTypeEnum(string typeString)
        {
            switch (typeString)
            {
                case "event":
                    return WeiXInMediaTypeEnum.MenuEvent;
                case "image":
                    return WeiXInMediaTypeEnum.Image;
                case "text":
                    return WeiXInMediaTypeEnum.Text;
                case "video":
                    return WeiXInMediaTypeEnum.Video;
                case "voice":
                    return WeiXInMediaTypeEnum.Voice;
                case "location":
                    return WeiXInMediaTypeEnum.Location;
                case "link":
                    return WeiXInMediaTypeEnum.Link;
                default:
                    return WeiXInMediaTypeEnum.Text;
            }
        }

        public static WeiXinMessageBass GetMsgEntity(string msg)
        {
            //LogHelper.Log.Info(string.Format("接收到微信信息：{0}", msg));
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(msg);
            string msgTypestring = xml.SelectSingleNode("xml/MsgType").InnerText;
            //LogHelper.Log.Info(string.Format("微信内容：{0}", msgTypestring));
            WeiXInMediaTypeEnum msgType = GetMsgTypeEnum(msgTypestring);
            WeiXinMessageBass wxmsg = new WeiXinMessageBass();
            //LogHelper.Log.Info(string.Format("微信类型：{0}", msgType.ToString()));
            if (xml.ChildNodes.Count > 0)
            {
                XmlNodeList nl = xml.ChildNodes[0].ChildNodes;
                foreach (XmlNode node in nl)
                {
                    switch (node.Name)
                    {
                        case "ToUserName"://开发者微信号
                            wxmsg.ToUserName = node.InnerText;
                            break;
                        case "FromUserName"://发送方帐号（一个OpenID）
                            wxmsg.FromUserID = node.InnerText;
                            WeiXinUser wuser = GetGetWeiXinUser(wxmsg.FromUserID);
                            if (wuser != null)
                            {
                                wxmsg.FromUserName = wuser.nickname;
                            }
                            break;
                        case "CreateTime"://消息创建时间 （整型）
                            wxmsg.CreateTime = int.Parse(node.InnerText);
                            break;
                        case "MsgType"://text
                            wxmsg.MsgType = node.InnerText;
                            break;
                        case "MsgId"://消息id，64位整型
                            wxmsg.MsgId = long.Parse(node.InnerText);
                            break;
                        case "Content"://文本消息内容
                            wxmsg.Content = node.InnerText;
                            break;
                        case "MediaId"://媒体id，可以调用多媒体文件下载接口拉取数据。
                            wxmsg.MediaId = node.InnerText;
                            break;
                        case "PicUrl"://图片消息
                            wxmsg.PicUrl = node.InnerText;
                            break;
                        case "Format"://语音消息
                            wxmsg.Format = node.InnerText;
                            break;
                        case "ThumbMediaId"://视频消息
                            wxmsg.ThumbMediaId = node.InnerText;
                            break;
                        case "Location_X"://地理位置消息
                            wxmsg.Location_X = node.InnerText;
                            break;
                        case "Location_Y"://地理位置消息
                            wxmsg.Location_Y = node.InnerText;
                            break;
                        case "Scale"://地理位置消息
                            wxmsg.Scale = node.InnerText;
                            break;
                        case "Label"://地理位置消息
                            wxmsg.Label = node.InnerText;
                            break;
                        case "Title"://链接消息
                            wxmsg.Title = node.InnerText;
                            break;
                        case "Description"://链接消息
                            wxmsg.Description = node.InnerText;
                            break;
                        case "Url"://链接消息
                            wxmsg.Url = node.InnerText;
                            break;
                        case "Event":
                            wxmsg.Event = node.InnerText;
                            break;
                        case "EventKey":
                            wxmsg.EventKey = node.InnerText;
                            break;
                        case "Ticket":
                            wxmsg.Ticket = node.InnerText;
                            break;
                        case "Latitude":
                            wxmsg.Latitude = node.InnerText;
                            break;
                        case "Longitude":
                            wxmsg.Longitude = node.InnerText;
                            break;
                        case "Precision":
                            wxmsg.Precision = node.InnerText;
                            break;
                        default: break;
                    }
                }
            }

            return wxmsg;
        }

        #region 高级接口

        //发送图文消息素材
        public static WeiXinUploadMediaResult UploadWeiXinNews(ArticleListData articls)
        {
            return _weixin.UploadWeiXinNews(string.Format(weixinUploadNews, Access_token), articls);
        }

        //群发接口
        public static WeiXinMediaReturn MulitySendNewsMessage(List<string> openIds, string mediaId, string msgtype)
        {
            return _weixin.MulitySendNewsMessage(string.Format(weixinMultiSendUrl, Access_token), openIds, mediaId, msgtype);
        }

        //发送模板消息
        public static WeiXinMediaReturn SendTemplateMessage(TemplateMessage msg)
        {
            return _weixin.SendTemplateMessage(string.Format(weixinTemplateUrl, Access_token), msg);
        }
        #endregion

        #region  网页授权
        /// <summary>
        /// 通过code获取web token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static WebAccessToken GetWebAccessToken(string code)
        {
            return _weixin.GetWebAccessToken(webTokeUrl, code);
        }
        /// <summary>
        /// 拉取用户信息(需scope为 snsapi_userinfo)
        /// </summary>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static WeiXinUser GetSNSUserInfo(string token)
        {
            return _weixin.GetSNSUserInfo(webUserInfo, token);
        }

        #endregion

        #region 微信JS-SDK接口
        /// 获取jsapi_ticket
        /// jsapi_ticket是公众号用于调用微信JS接口的临时票据。
        /// 正常情况下，jsapi_ticket的有效期为7200秒，通过access_token来获取。
        /// 由于获取jsapi_ticket的api调用次数非常有限，频繁刷新jsapi_ticket会导致api调用受限，影响自身业务，开发者必须在自己的服务全局缓存jsapi_ticket 。
        /// </summary>
        /// <param name="access_token">BasicAPI获取的access_token,也可以通过TokenHelper获取</param>
        /// <returns></returns>
        public static ApiTicket GetTickect()
        {
            ApiTicket jsTicket = _weixin.GetTickect(apiticketUrl, Access_token);
            if (IsAccess_tokenError(jsTicket.errcode.ToString()))
            {
                jsTicket = _weixin.GetTickect(apiticketUrl, Access_token);
            }
            return jsTicket;
        }

        #endregion
    }
}
