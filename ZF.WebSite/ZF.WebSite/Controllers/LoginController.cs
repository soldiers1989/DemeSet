using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using ZF.Infrastructure.WxLogin;
using System.Web.Script.Serialization;
using ZF.WebSite.App_Data;

namespace ZF.WebSite.Controllers
{
    
    public class LoginController : Controller
    {
        private static string ApiUrl = ConfigurationManager.AppSettings[""];

        // GET: Login
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Regist()
        {
            ViewBag.userIp = PublicCommon.GetWebClientIp();
            return View();
        }

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult ForgetPassword()
        {
            return View();
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        public ActionResult UserLogin()
        {
            ViewBag.userIp = PublicCommon.GetWebClientIp();
            return View();
        }

        /// <summary>
        /// 微信扫码登录
        /// </summary>
        /// <returns></returns>
        public ActionResult WikiLogin()
        {
            //获取CODE
            //code说明 ： code作为换取access_token的票据，每次用户授权带上的code将不一样，code只能使用一次，5分钟未被使用自动过期。
            string code = System.Web.HttpContext.Current.Request.QueryString["code"];
            WxPcLoginAuthorized wxLoginAuthorized = new WxPcLoginAuthorized();
            WxPcUserInfo userinfo = wxLoginAuthorized.GetWikiUserInfo(code);
            //用数据拉取成功之后写入数据库
            if (!string.IsNullOrEmpty(userinfo.openid))
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonData = js.Serialize(userinfo);//序列化
                ViewBag.parment = jsonData;
            }
            else
            {
                ViewBag.parment = "";
            }
           return View();
        }
    }
}