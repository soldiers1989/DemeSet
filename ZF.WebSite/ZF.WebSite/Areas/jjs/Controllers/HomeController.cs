using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using ZF.Infrastructure.Des3;
using ZF.Infrastructure.Json;
using ZF.WebSite.Areas.jjs.Models;
using ZF.WebSite.Models;

namespace ZF.WebSite.Areas.jjs.Controllers
{
    public class HomeController : Controller
    {

        public string WebApi = ConfigurationManager.AppSettings["WebApi"];

        public string JsapiTicket = ConfigurationManager.AppSettings["JsapiTicketUrl"];


        [HttpPost]
        public string GetJsapiTicket()
        {
            var jsapi_ticket = WeiXinServiceManager.GetTickect().ticket;
            string timestamp = CommonMethod.ConvertDateTimeInt(DateTime.Now).ToString();//生成签名的时间戳
            string nonceStr = CommonMethod.GetRandCode(16);//生成签名的随机串
            string url = JsapiTicket;//当前的地址

            string[] ArrayList = { "jsapi_ticket=" + jsapi_ticket, "timestamp=" + timestamp, "noncestr=" + nonceStr, "url=" + url };
            Array.Sort(ArrayList);
            string signature = string.Join("&", ArrayList);
            var hashPasswordForStoringInConfigFile = FormsAuthentication.HashPasswordForStoringInConfigFile(signature, "SHA1");
            if (hashPasswordForStoringInConfigFile != null)
                signature = hashPasswordForStoringInConfigFile.ToLower();
            var ticket = AngelSession.Get("jsapi_ticket");
            if (ticket != null)
            {
                return ticket.ToString();
            }
            ticket = "{\"appId\":\"" + ConfigurationManager.AppSettings["WeiXinAccountAppId"] + "\", \"timestamp\":" +
                     timestamp + ",\"nonceStr\":\"" + nonceStr + "\",\"signature\":\"" + signature + "\"}";
            AngelSession.Set("jsapi_ticket", ticket, 60, 0);
            return ticket.ToString();
        }

        // GET: jjs/Home
        public ActionResult Index(string code)
        {
            var subjectId = "";
            var Ticket = "";
            //判断是否获取到code  没有则请求微信接口获取code
            if (!string.IsNullOrWhiteSpace(code))
            {
                try
                {
                    //通过code 去公众号关注列表获取用户access_token  openid
                    var model = WeiXinServiceManager.GetWebAccessToken(code);
                    if (model != null)
                    {
                        //获取用户微信信息  subscribe为0表示未关注公众号  则未关注则调到用户授权页面  关注则直接往后台写登录数据
                        var userInfo = WeiXinServiceManager.GetGetWeiXinUser(model.openid);
                        AngelSession.Set("openid", userInfo.openid, 60, 0);
                        if (userInfo.subscribe == 0)
                        {
                            var redirectUri = ConfigurationManager.AppSettings["redirect_uri1"];
                            var url =
                                string.Format(
                                    WeiXinServiceManager.snsapi_userinfoUrl,
                                    ConfigurationManager.AppSettings["WeiXinAccountAppId"], redirectUri);
                            Response.Redirect(url);
                        }
                        else
                        {
                            var url = WebApi + "api/Account/WikiLogin";
                            var _weixinApi = new WeiXinApi();
                            userInfo.userip = PublicCommon.GetIpAddress();
                            var controller = "HomePage";
                            var action = "Index";
                            var query = "";
                            if (AngelSession.Get("action") != null)
                            {
                                var controllerAction = AngelSession.Get("action").ToString();
                                var data = Des3Cryption.Decrypt3DES(controllerAction).Split(',');
                                controller = data[0];
                                action = data[1];
                                query = data[2];
                                if (!string.IsNullOrWhiteSpace(query))
                                {
                                    try
                                    {
                                        if (query.IndexOf("?PromotionCode=", StringComparison.Ordinal) >= 0)
                                        {
                                            if (query.Split('=').Length > 1)
                                            {
                                                userInfo.code = query.Split('=')[1];
                                            }
                                        }
                                        if (query.IndexOf("InstitutionsId=", StringComparison.Ordinal) >= 0)
                                        {
                                            if (query.Split('=').Length > 1)
                                            {
                                                userInfo.InstitutionsId = query.Split('=')[1];
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        // ignored
                                    }
                                }
                            }
                            string serialValue = JsonConvert.SerializeObject(userInfo);
                            var ddd = _weixinApi.WikiLogin(url, serialValue);
                            Class qtinfo = (Class)JsonHelper.jsonDes<Class>(ddd);
                            Ticket = qtinfo.Result.Ticket;
                            if (qtinfo.Result.data.Split('@').Length > 1)
                            {
                                subjectId = qtinfo.Result.data.Split('@')[1];
                            }
                            var session = AngelSession.Get("UserInfo");
                            if (session != null)
                            {
                                AngelSession.Remove("UserInfo");
                            }
                            AngelSession.Add("UserInfo", Des3Cryption.Encrypt3DES(JsonHelper.json(userInfo)));
                            return RedirectToAction("Jump", "Home", new
                            {
                                url = controller + "/" + action + query,
                                Ticket = Ticket,
                                subjectId = subjectId,
                                IsRegistered = qtinfo.Result.IsRegistered
                            });
                        }
                    }
                    else
                    {
                        ViewBag.Ticket = "";
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }
            else
            {
                //获取code
                var redirectUri = ConfigurationManager.AppSettings["redirect_uri"];
                var url =
                    string.Format(
                         WeiXinServiceManager.snsapi_baseUrl,
                        ConfigurationManager.AppSettings["WeiXinAccountAppId"], redirectUri);
                Response.Redirect(url);
            }
            return RedirectToAction("Jump", "Home", new
            {
                url = "HomePage/Index",
                Ticket = Ticket,
                subjectId = subjectId,
            });
        }

        /// <summary>
        /// 用户未关注公众号授权
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult Index1(string code)
        {
            var Ticket = "";
            var subjectId = "";
            if (!string.IsNullOrWhiteSpace(code))
            {
                try
                {
                    var model = WeiXinServiceManager.GetWebAccessToken(code);
                    if (model != null)
                    {
                        var userInfo = WeiXinServiceManager.GetSNSUserInfo(model.access_token);
                        AngelSession.Set("openid", userInfo.openid, 60, 0);
                        userInfo.userip = PublicCommon.GetIpAddress();
                        var controller = "HomePage";
                        var action = "Index";
                        var query = "";
                        if (AngelSession.Get("action") != null)
                        {
                            var controllerAction = AngelSession.Get("action").ToString();
                            var data = Des3Cryption.Decrypt3DES(controllerAction).Split(',');
                            controller = data[0];
                            action = data[1];
                            query = data[2];
                            if (!string.IsNullOrWhiteSpace(query))
                            {
                                try
                                {
                                    if (query.IndexOf("?PromotionCode=", StringComparison.Ordinal) >= 0)
                                    {
                                        if (query.Split('=').Length > 1)
                                        {
                                            userInfo.code = query.Split('=')[1];
                                        }
                                    }
                                    if (query.IndexOf("InstitutionsId=", StringComparison.Ordinal) >= 0)
                                    {
                                        if (query.Split('=').Length > 1)
                                        {
                                            userInfo.InstitutionsId = query.Split('=')[1];
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    // ignored
                                }
                            }
                        }
                        var url = WebApi + "api/Account/WikiLogin";
                        var _weixinApi = new WeiXinApi();
                        string serialValue = JsonConvert.SerializeObject(userInfo);
                        var ddd = _weixinApi.WikiLogin(url, serialValue);
                        Class qtinfo = (Class)JsonHelper.jsonDes<Class>(ddd);
                        Ticket = qtinfo.Result.Ticket;
                        if (qtinfo.Result.data.Split('@').Length > 1)
                        {
                            subjectId = qtinfo.Result.data.Split('@')[1];
                        }
                        var session = AngelSession.Get("UserInfo");
                        if (session != null)
                        {
                            AngelSession.Remove("UserInfo");
                        }
                        AngelSession.Add("UserInfo", Des3Cryption.Encrypt3DES(JsonHelper.json(userInfo)));
                        //  return RedirectToAction(action, controller);
                        return RedirectToAction("Jump", "Home", new
                        {
                            url = controller + "/" + action + query,
                            Ticket = Ticket,
                            subjectId = subjectId,
                            IsRegistered = qtinfo.Result.IsRegistered
                        });
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return RedirectToAction("Jump", "Home", new
            {
                url = "HomePage/Index",
                Ticket = Ticket,
                subjectId = subjectId,
            });
        }

        public ActionResult Jump(string url, string ticket, string subjectId, int? isRegistered = 0)
        {
            ViewBag.Url = url;
            ViewBag.Ticket = ticket;
            ViewBag.SubjectId = subjectId;
            ViewBag.IsRegistered = isRegistered;
            return View();
        }



        [HttpPost]
        public bool GetSubscribe()
        {
            string openid = AngelSession.Get("openid").ToString();
            var userInfo = WeiXinServiceManager.GetGetWeiXinUser(openid);
            return userInfo.subscribe == 1;
        }

        public class Class
        {
            public Class1 Result { get; set; }

            public bool Success { get; set; }

        }

        public class Class1
        {
            public bool Success { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string data { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string Ticket { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public int? IsRegistered { get; set; }

        }

        /// <summary>
        /// 通用方法类
        /// </summary>
        public class CommonMethod
        {
            #region Post/Get提交调用抓取
            /// <summary>
            /// Post/get 提交调用抓取
            /// </summary>
            /// <param name="url">提交地址</param>
            /// <param name="param">参数</param>
            /// <returns>string</returns>
            public static string WebRequestPostOrGet(string sUrl, string sParam)
            {
                byte[] bt = System.Text.Encoding.UTF8.GetBytes(sParam);

                Uri uriurl = new Uri(sUrl);
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(uriurl);//HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url + (url.IndexOf("?") > -1 ? "" : "?") + param);
                req.Method = "Post";
                req.Timeout = 120 * 1000;
                req.ContentType = "application/x-www-form-urlencoded;";
                req.ContentLength = bt.Length;

                using (Stream reqStream = req.GetRequestStream())//using 使用可以释放using段内的内存
                {
                    reqStream.Write(bt, 0, bt.Length);
                    reqStream.Flush();
                }
                try
                {
                    using (WebResponse res = req.GetResponse())
                    {
                        //在这里对接收到的页面内容进行处理 

                        Stream resStream = res.GetResponseStream();

                        StreamReader resStreamReader = new StreamReader(resStream, System.Text.Encoding.UTF8);

                        string resLine;

                        System.Text.StringBuilder resStringBuilder = new System.Text.StringBuilder();

                        while ((resLine = resStreamReader.ReadLine()) != null)
                        {
                            resStringBuilder.Append(resLine + System.Environment.NewLine);
                        }

                        resStream.Close();
                        resStreamReader.Close();

                        return resStringBuilder.ToString();
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;//url错误时候回报错
                }
            }
            #endregion Post/Get提交调用抓取

            #region unix/datatime 时间转换
            /// <summary>
            /// unix时间转换为datetime
            /// </summary>
            /// <param name="timeStamp"></param>
            /// <returns></returns>
            public static DateTime UnixTimeToTime(string timeStamp)
            {
                DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                long lTime = long.Parse(timeStamp + "0000000");
                TimeSpan toNow = new TimeSpan(lTime);
                return dtStart.Add(toNow);
            }

            /// <summary>
            /// datetime转换为unixtime
            /// </summary>
            /// <param name="time"></param>
            /// <returns></returns>
            public static int ConvertDateTimeInt(System.DateTime time)
            {
                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                return (int)(time - startTime).TotalSeconds;
            }
            #endregion

            #region 生成随机字符
            /// <summary>
            /// 生成随机字符
            /// </summary>
            /// <param name="iLength">生成字符串的长度</param>
            /// <returns>返回随机字符串</returns>
            public static string GetRandCode(int iLength)
            {
                string sCode = "";
                if (iLength == 0)
                {
                    iLength = 4;
                }
                string codeSerial = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
                string[] arr = codeSerial.Split(',');
                int randValue = -1;
                Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
                for (int i = 0; i < iLength; i++)
                {
                    randValue = rand.Next(0, arr.Length - 1);
                    sCode += arr[randValue];
                }
                return sCode;
            }
            #endregion
        }
    }
}