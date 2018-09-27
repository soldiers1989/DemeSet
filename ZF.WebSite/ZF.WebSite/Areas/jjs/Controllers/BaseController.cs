using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ZF.Infrastructure.Des3;
using ZF.Infrastructure.Json;
using ZF.WebSite.Areas.jjs.Models;
using ZF.WebSite.Models;

namespace ZF.WebSite.Areas.jjs.Controllers
{


    public class BaseController : Controller
    {
        public string WebApi = ConfigurationManager.AppSettings["WebApi"];

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            System.Web.HttpContext.Current.Application["DefuleDomain"] = ConfigurationManager.AppSettings["DefuleDomain"];
            System.Web.HttpContext.Current.Application["TitleName"] = ConfigurationManager.AppSettings["TitleName"];
            AngelSession.Remove("action");
            base.OnActionExecuting(filterContext);

            bool result = false;

            //controller上是否有特性CheckLogin，以及特性的IsNeedLogin值
            var controllerAttrs =
                filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(CheckLogin), false);
            if (controllerAttrs.Any())
            {
                var conAttr = controllerAttrs[0] as CheckLogin;
                if (conAttr != null)
                {
                    if (conAttr.IsNeedLogin)
                        result = true;
                    else
                        result = false;
                }
            }

            //action上是否有特性CheckLogin，以及特性的IsNeedLogin值
            var actionAttrs = filterContext.ActionDescriptor.GetCustomAttributes(typeof(CheckLogin), false);
            if (actionAttrs.Any())
            {
                var attr = actionAttrs[0] as CheckLogin;
                if (attr != null)
                {
                    if (attr.IsNeedLogin)
                        result = true;
                    else
                        result = false;
                }
            }

            if (!IsLogin() && result)
            {

                var controller = RouteData.Values["controller"];
                var action = RouteData.Values["action"];

                var url = Request.Url?.Query ?? "";

                AngelSession.Add("action", Des3Cryption.Encrypt3DES(controller + "," + action + "," + url));

                //如果没有登录，则跳至登陆页
                //filterContext.Result = Redirect("~/jjs/Home/Index");
            }
        }

        protected bool IsLogin()
        {
            if (AngelSession.Get("UserInfo") != null)
                return true;
            return false;
        }

        /// <summary>
        /// 获取视频所属课程
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetCourseId(string code)
        {
            var url = WebApi + "api/CourseInfo/GetCourseId";
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/json";
            #region 添加Post 参数  
            byte[] data = Encoding.UTF8.GetBytes("{\"Id\": \"" + code + "\"}");
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion
            HttpWebResponse resp;
            try
            {
                resp = (HttpWebResponse)req.GetResponse();
            }
            catch (WebException ex)
            {
                return "";
            }
            Stream stream = resp.GetResponseStream();
            //获取响应内容  
            if (stream != null)
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
            ModelJob<string> qtinfo = (ModelJob<string>)JsonHelper.jsonDes<ModelJob<string>>(result);
            return qtinfo.Result;
        }
    }
}
public sealed class CheckLogin : Attribute
{
    public string IsExploit = ConfigurationManager.AppSettings["IsExploit"];

    public bool IsNeedLogin = false;

    public CheckLogin(bool isNeed)
    {
        if (IsExploit == "1")
        {
            this.IsNeedLogin = false;
        }
        else
        {
            this.IsNeedLogin = isNeed;
        }

    }
}