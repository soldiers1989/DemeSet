using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.WebApiAppService.SystemModule;
using ZF.Application.WebApiDto.SystemModule;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Dapper.Db.Repository;
using ZF.Infrastructure;
using ZF.Infrastructure.Des3;
using ZF.Infrastructure.SmsService;
using ZF.Infrastructure.WxLogin;

namespace ZF.API.Controllers
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class RegisterController : BaseApiController
    {
        RegisterUserAppService _registerUserAppService;
        private readonly RegisterUserAppApiService _registerUserAppApiService;
        private readonly IRegisterUserRepository _repository;
        /// <summary>
        /// 单例
        /// </summary>
        /// <param name="registerUserAppService"></param>
        public RegisterController(RegisterUserAppService registerUserAppService, RegisterUserAppApiService registerUserAppApiService, IRegisterUserRepository repository)
        {
            _registerUserAppService = registerUserAppService;
            _registerUserAppApiService = registerUserAppApiService;
            _repository = repository;
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public RegisterUser GetOne()
        {
            return _registerUserAppService.GetOne(UserObject.Id);
        }
        /// <summary>
        /// 修改用户基本信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut UpdateOne(RegisterUser input)
        {
            if (string.IsNullOrEmpty(input.Id))
            {
                input.Id = UserObject.Id;
            }
            return _registerUserAppService.UpdateOne(input);
        }

        /// <summary>
        /// 获取指定用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public RegisterUser GetUser(RegisterUserInput input)
        {
            return _registerUserAppService.GetOne(input.Id);
        }

        /// <summary>
        /// 绑定微信号
        /// </summary>
        /// <param name="userinfo"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public object BindWiki(WxUserInfo userinfo)
        {
            if (!string.IsNullOrEmpty(UserObject.Id))
            {
                var ismodel = _repository.Get(userinfo.unionid);
                if (ismodel == null)
                {
                    RegisterUser user = new RegisterUser()
                    {
                        WechatId = userinfo.unionid,
                        State = (int)QuestionState.Enable,
                        AddTime = DateTime.Now,
                        UserName = SendSmsService.GetRandomString(25),
                        NickNamw = userinfo.nickname,
                        HeadImage = userinfo.headimgurl,
                        Sex = userinfo.sex,
                        Province = userinfo.province,
                        City = userinfo.city,
                        Country = userinfo.country,
                        Code = userinfo.code,
                        IsBindWiki = 1
                    };
                    try
                    {
                        _registerUserAppApiService.BindWiki(user, UserObject.Id);
                        RegisterUserOutputDto registerUser = new RegisterUserRepository().GetLogin(userinfo.unionid);
                        var ticket = $"{userinfo.unionid}&{registerUser.LoginPwd}";

                        return Json(new { Success = true, data = registerUser.SubjectName + "@" + registerUser.SubjectId, Message = "绑定成功!", Ticket = Des3Cryption.Encrypt3DES(ticket), telphone = "yes" });
                    }
                    catch (Exception)
                    {
                        return Json(new { Success = false, data = "", Message = "绑定失败!", telphone = "no" });
                    }
                }
                else
                {
                    return Json(new { Success = false, data = "", Message = "该微信号已做过绑定!", telphone = "no" });
                }
            }
            else
            {
                return Json(new { Success = false, data = "", Message = "请先登录!", telphone = "no" });
            }
        }
    }
}
