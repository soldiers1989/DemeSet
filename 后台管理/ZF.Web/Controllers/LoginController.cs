using System;
using System.Web;
using System.Web.Mvc;
using ZF.Application.AppService;
using ZF.Application.Dto;
using ZF.Infrastructure.AlipayService;
using ZF.Infrastructure.Des3;
using ZF.Infrastructure.Json;
using ZF.Web.Models;

namespace ZF.Web.Controllers
{
    public class LoginController : BaseController
    {

        private readonly UserAppService _userAppService;
        private readonly UserLoginLogAppService _userLoginLogAppService;

        private readonly CourseVideoFileAppService _ourseVideoFileAppService;

        public LoginController(UserAppService userAppService, UserLoginLogAppService userLoginLogAppService, CourseVideoFileAppService ourseVideoFileAppService)
        {
            _userAppService = userAppService;
            _userLoginLogAppService = userLoginLogAppService;
            _ourseVideoFileAppService = ourseVideoFileAppService;
        }

        // GET: Login

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }


        [AllowAnonymous]
        public ActionResult TranscodeComplete()
        {
           
            byte[] byts = new byte[Request.InputStream.Length];
            Request.InputStream.Read(byts, 0, byts.Length);
            string req = System.Text.Encoding.Default.GetString(byts);
            req = Server.UrlDecode(req);

            try
            {
                var model = (Root)JsonHelper.jsonDes<Root>(req);
                var model1 = Aliyun.GetVideoPlayAuth(model.VideoId);
                Logger.Debug(model1.CoverURL);
                _ourseVideoFileAppService.Update(new CourseVideoFileInput
                {
                    Id = model.VideoId,
                    CoverURL = model1.CoverURL,
                    Duration = model.StreamInfos[1].Duration,
                    VideoUrl = model.StreamInfos[1].FileUrl
                });
            }
            catch (Exception ex)
            {

                Logger.Debug(ex.Message);
            }
           
            return View();
        }

        [AllowAnonymous]
        public JsonResult Login()
        {

            return Json(new { Success = true, data = "", Message = "登录成功!" });
        }

        [AllowAnonymous]
        public JsonResult LoginIn(LoginInput input)
        {
            if (string.IsNullOrEmpty(input.LoginName))
            {
                return Json(new { Success = false, data = "", Message = "登录名不能为空!" });
            }
            if (string.IsNullOrEmpty(input.PassWord))
            {
                return Json(new { Success = false, data = "", Message = "密码不能为空!" });
            }
            var model = _userAppService.GetLogin(input);
            if (model == null)
            {
                return Json(new { Success = false, data = "", Message = "登录名不存在!" });
            }
            if (model.PassWord == input.PassWord)
            {
                if (input.Check == "1")
                {
                    /*****写入cookie******/
                    HttpCookie httpCookie = System.Web.HttpContext.Current.Request.Cookies["UserInfo"];
                    if (httpCookie != null)
                    {
                        httpCookie.Expires = DateTime.Now.AddHours(-1);
                        System.Web.HttpContext.Current.Request.Cookies.Add(httpCookie);
                    }
                    httpCookie = new HttpCookie("UserInfo");
                    httpCookie.Values.Add("User", Des3Cryption.Encrypt3DES(JsonHelper.json(model)));
                    httpCookie.Expires = DateTime.Now.AddDays(1);
                    System.Web.HttpContext.Current.Response.Cookies.Add(httpCookie);
                }
                else
                {
                    /*****写入cookie******/
                    HttpCookie httpCookie = System.Web.HttpContext.Current.Request.Cookies["UserInfo"];
                    if (httpCookie != null)
                    {
                        httpCookie.Expires = DateTime.Now.AddHours(-1);
                        System.Web.HttpContext.Current.Request.Cookies.Add(httpCookie);
                    }
                    httpCookie = new HttpCookie("UserInfo");
                    httpCookie.Values.Add("User", Des3Cryption.Encrypt3DES(JsonHelper.json(model)));
                    System.Web.HttpContext.Current.Response.Cookies.Add(httpCookie);
                }


                //登陆成功,写入登陆日志
                //_userLoginLogAppService.AddLoginLog(new UserLoginLogInput
                //{
                //    UserId = model.Id,
                //    LoginType = input.LoginType
                //});

                return Json(new { Success = true, data = "", Message = "登录成功!" });
            }
            return Json(new { Success = false, data = "", Message = "密码错误!" });
        }

        public ActionResult Modify()
        {
            return PartialView();
        }
    }
}