using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ZF.WebSite.App_Data;
using ZF.WebSite.Models;

namespace ZF.WebSite.Controllers
{
    public class HomeController : BaseController
    {

        public ActionResult Index()
        {
            ViewBag.DefuleDomain = DefuleDomain;
            var model = GetTitle("", 1);
            if (model != null)
            {
                ViewBag.Title = string.IsNullOrEmpty(model.Title) ? "中国人事考试在线|人事社经济师课堂" : model.Title;
                ViewBag.Description = string.IsNullOrEmpty(model.Description) ? "" : model.Description;
                ViewBag.KeyWord = string.IsNullOrEmpty(model.KeyWord) ? "" : model.KeyWord;
            }

            #region 微信扫码登录

            //获取CODE
            //code说明 ： code作为换取access_token的票据，每次用户授权带上的code将不一样，code只能使用一次，5分钟未被使用自动过期。
            string code = System.Web.HttpContext.Current.Request.QueryString["code"];
            string state = System.Web.HttpContext.Current.Request.QueryString["state"];
            if (!string.IsNullOrEmpty(code))
            {
                WxPcLoginAuthorized wxLoginAuthorized = new WxPcLoginAuthorized();
                WxPcUserInfo userinfo = wxLoginAuthorized.GetWikiUserInfo(code);

                //用数据拉取成功之后写入数据库
                if (!string.IsNullOrEmpty(userinfo.openid))
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    userinfo.userip = PublicCommon.GetIpAddress();
                    string jsonData = js.Serialize(userinfo);//序列化
                    ViewBag.parment = jsonData;
                    ViewBag.bindwiki = state;
                }
                else
                {
                    ViewBag.parment = "";
                }
            }
            else
            {
                ViewBag.parment = "";
            }
            #endregion

            return View();
        }
        public ActionResult Indexbak()
        {
            ViewBag.DefuleDomain = DefuleDomain;
            var model = GetTitle("", 1);
            if (model != null)
            {
                ViewBag.Title = string.IsNullOrEmpty(model.Title) ? "中国人事考试在线|人事社经济师课堂" : model.Title;
                ViewBag.Description = string.IsNullOrEmpty(model.Description) ? "" : model.Description;
                ViewBag.KeyWord = string.IsNullOrEmpty(model.KeyWord) ? "" : model.KeyWord;
            }
            return View();
        }


        public ActionResult Index2()
        {
            ViewBag.DefuleDomain = DefuleDomain;
            var model = GetTitle("", 1);
            if (model != null)
            {
                ViewBag.Title = string.IsNullOrEmpty(model.Title) ? "中国人事考试在线|人事社经济师课堂" : model.Title;
                ViewBag.Description = string.IsNullOrEmpty(model.Description) ? "" : model.Description;
                ViewBag.KeyWord = string.IsNullOrEmpty(model.KeyWord) ? "" : model.KeyWord;
            }

            #region 微信扫码登录

            //获取CODE
            //code说明 ： code作为换取access_token的票据，每次用户授权带上的code将不一样，code只能使用一次，5分钟未被使用自动过期。
            string code = System.Web.HttpContext.Current.Request.QueryString["code"];
            if (!string.IsNullOrEmpty(code))
            {
                WxPcLoginAuthorized wxLoginAuthorized = new WxPcLoginAuthorized();
                WxPcUserInfo userinfo = wxLoginAuthorized.GetWikiUserInfo(code);
                //用数据拉取成功之后写入数据库
                if (!string.IsNullOrEmpty(userinfo.openid))
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    userinfo.userip = PublicCommon.GetIpAddress();
                    string jsonData = js.Serialize(userinfo);//序列化
                    ViewBag.parment = jsonData;
                }
                else
                {
                    ViewBag.parment = "";
                }
            }
            else
            {
                ViewBag.parment = "";
            }
            #endregion

            return View();
        }

    }
}