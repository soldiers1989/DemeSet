using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Application.WebApiAppService;
using ZF.Application.WebApiAppService.SystemModule;
using ZF.Application.WebApiDto;
using ZF.Application.WebApiDto.SystemModule;
using ZF.API.Model;
using ZF.Dapper.Db.Repository;
using ZF.Infrastructure.Des3;
using ZF.Infrastructure.SmsService;
using LoginInput = ZF.API.Model.LoginInput;
using ZF.Infrastructure.WxLogin;
using ZF.Core.Entity;
using ZF.Infrastructure;
using ZF.Application.WebApiDto.SheetDtoModule;
using ZF.Application.WebApiAppService.SheetModule;
using ZF.Core.IRepository;
using ZF.Infrastructure.RedisCache;

namespace ZF.API.Controllers
{
    /// <summary>
    /// 用户信息控制器
    /// </summary>
    public class AccountController : ApiController
    {

        private readonly SmsendLogAppService _smsendLogAppService;
        private readonly RegisterUserAppApiService _registerUserAppApiService;
        private readonly UserLoginLogAppService _userLoginLogAppService;

        private readonly OperatorLogAppService _operatorLogAppService;
        private readonly SheetApiService sheetApiService;
        private readonly IRegisterUserRepository _repository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="smsendLogAppService"></param>
        /// <param name="registerUserAppApiService"></param>
        /// <param name="userLoginLogAppService"></param>
        public AccountController(SmsendLogAppService smsendLogAppService, RegisterUserAppApiService registerUserAppApiService,
            UserLoginLogAppService userLoginLogAppService, OperatorLogAppService operatorLogAppService, SheetApiService _sheetApiService, IRegisterUserRepository repository)
        {
            _smsendLogAppService = smsendLogAppService;
            _registerUserAppApiService = registerUserAppApiService;
            _userLoginLogAppService = userLoginLogAppService;
            _operatorLogAppService = operatorLogAppService;
            sheetApiService = _sheetApiService;
            _repository = repository;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        [System.Web.Mvc.AllowAnonymous]
        public object Login(LoginInput input)
        {
            var model = new RegisterUserRepository().GetLogin(input.TelphoneNum);
            if (model == null)
            {
                return Json(new { Success = false, data = "", Message = "手机号未注册或未绑定" });
            }
            switch (input.PcLoginType)
            {
                case 0:
                    if (model.LoginPwd != input.LoginPwd) return Json(new { Success = false, data = "", Message = "密码错误!" });
                    break;
                case 1:
                    bool codeIsTrue = _registerUserAppApiService.CheckRegisterMobileVerificationCode(input.Code, input.TelphoneNum);
                    if (!codeIsTrue) { return Json(new { Success = false, data = "", Message = "验证码错误!" }); }
                    break;
                case 2:
                    break;
            }
            if (model == null && input.PcLoginType == 1)//手机短信登录,未注册的情况下自动注册用户
            {
                //默认密码是：123
                RegisterUserInput user = new RegisterUserInput()
                {
                    TelphoneNum = input.TelphoneNum,
                    LoginPwd = "202cb962ac59075b964b07152d234b70",
                    RegiesterType = input.RegiesterType,
                    RegisterIp = input.RegisterIp
                };
                if (_registerUserAppApiService.Registered(user))
                {
                    model = new RegisterUserRepository().GetLogin(input.TelphoneNum);
                }
                else
                {
                    return Json(new { Success = false, data = "", Message = "登录失败!" });
                }
            }
            if (model.State != 0)
            {
                return Json(new { Success = false, data = "", Message = "该手机号已经失效!" });
            }
            //登陆成功,写入登陆日志
            _userLoginLogAppService.AddLoginLog(new UserLoginLogInput
            {
                UserId = model.Id,
                LoginType = input.LoginType
            }, input.TelphoneNum);
            var guid = Guid.NewGuid().ToString();
            RedisCacheHelper.Add(model.Id, guid);
            var ticket = $"{model.Id}&{model.LoginPwd}&{guid}";
            return Json(new { Success = true, data = model.SubjectName + "@" + model.SubjectId, Message = "登录成功!", Ticket = Des3Cryption.Encrypt3DES(ticket), isBindWiki = model.IsBindWiki });
        }

        /// <summary>
        /// 生成登录的二维码
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        [System.Web.Mvc.AllowAnonymous]
        public MessagesOutPut CreateWikiTwoDimensionalCode()
        {
            string towDimensionalCodePath = CreateWikiLoginTwoDimensionalCode.WikiLoginTwoDimensionalCode();
            if (string.IsNullOrEmpty(towDimensionalCodePath))
            {
                return new MessagesOutPut { Success = false, Message = "登录二维码生成失败" };
            }
            else
            {
                return new MessagesOutPut { Success = true, Message = towDimensionalCodePath };
            }
        }
        /// <summary>
        /// 微信授权回调页
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        [System.Web.Mvc.AllowAnonymous]
        public object WikiLogin(WxUserInfo userinfo)
        {
            var guid = Guid.NewGuid().ToString();
            //用数据拉取成功之后写入数据库
            var model = new RegisterUserRepository().GetLogin(userinfo.unionid);
            if (model == null)
            {
                //默认密码是：123
                RegisterUser user = new RegisterUser()
                {
                    Id = userinfo.unionid,
                    TelphoneNum = "",
                    LoginPwd = "202cb962ac59075b964b07152d234b70",
                    RegiesterType = string.IsNullOrEmpty(userinfo.regiestertype) ? "wiki" : userinfo.regiestertype,
                    RegisterIp = string.IsNullOrEmpty(userinfo.userip) ? "未获取到Ip" : userinfo.userip,
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
                    InstitutionsId = userinfo.InstitutionsId,
                    IsBindWiki = 1,
                    WechatId = userinfo.unionid
                };
                if (_registerUserAppApiService.WikiRegistered(user))
                {
                    if (!string.IsNullOrEmpty(userinfo.regiestertype))
                    {
                        RedisCacheHelper.Add(userinfo.unionid, guid);
                    }
                    else
                    {
                        guid = "";
                        RedisCacheHelper.Add(userinfo.unionid, "");
                    }
                    RegisterUserOutputDto registerUser = new RegisterUserRepository().GetLogin(userinfo.unionid);
                    var ticket = $"{userinfo.unionid}&{registerUser.LoginPwd}&{guid}";

                    //登陆成功,写入登陆日志
                    _userLoginLogAppService.AddLoginLog(new UserLoginLogInput
                    {
                        UserId = registerUser.Id,
                        LoginType = 1
                    }, registerUser.NickNamw);
                    return Json(new { Success = true, data = registerUser.SubjectName + "@" + registerUser.SubjectId, Message = "登录成功!", Ticket = Des3Cryption.Encrypt3DES(ticket), telphone = "", IsRegistered = 1 });
                }
                else
                {
                    return Json(new { Success = false, data = "", Message = "登录失败!" });
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(userinfo.regiestertype))
                {
                    RedisCacheHelper.Add(userinfo.unionid, guid);
                }
                else
                {
                    guid = "";
                    RedisCacheHelper.Add(userinfo.unionid, "");
                }
                //登陆成功,写入登陆日志
                _userLoginLogAppService.AddLoginLog(new UserLoginLogInput
                {
                    UserId = userinfo.unionid,
                    LoginType = 1
                }, model.NickNamw);
                var ticket = $"{userinfo.unionid}&{model.LoginPwd}&{guid}";
                return Json(new { Success = true, data = model.SubjectName + "@" + model.SubjectId, Message = "登录成功!", Ticket = Des3Cryption.Encrypt3DES(ticket), telphone = model.TelphoneNum, IsRegistered = 0 });
            }
        }


        /// <summary>
        /// 短信发送接口
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        [System.Web.Http.HttpPost]
        public MessagesOutPut SendSmsInfo(SmsModel input)
        {
            return _smsendLogAppService.SendSmsInfo(input);
        }

        /// <summary>
        ///  用户注册 验证是否注册  false  验证失败  true  成功
        /// </summary>
        /// <returns></returns>

        [System.Web.Http.HttpPost]
        public bool CheckIfMobileDuplicated(VerificationCode input)
        {
            return _registerUserAppApiService.CheckIfMobileDuplicated(input.TelphoneNum);
        }


        /// <summary>
        /// 用户注册 验证是否存在
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public bool CheckIfMobileDuplicatedIsTrue(VerificationCode input)
        {
            return _registerUserAppApiService.CheckIfMobileDuplicatedIsTrue(input.TelphoneNum);
        }


        /// <summary>
        /// 修改手机号码是否是唯一
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public bool ModifyUserPhoneCheck(VerificationCode input)
        {
            return _registerUserAppApiService.ModifyUserPhoneCheck(input.TelphoneNum, input.Id);
        }
        /// <summary>
        ///  验证注册验证码是否正确
        /// </summary>
        /// <returns></returns>

        [System.Web.Http.HttpPost]
        public bool CheckRegisterMobileVerificationCode(VerificationCode input)
        {
            return _registerUserAppApiService.CheckRegisterMobileVerificationCode(input.Code, input.TelphoneNum);
        }

        /// <summary>
        ///  验证推广码是否存在
        /// </summary>
        /// <returns></returns>

        [System.Web.Http.HttpPost]
        public bool VerificationExtensionCode(VerificationCode input)
        {
            return _registerUserAppApiService.VerificationExtensionCode(input.Code);
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <returns></returns>

        [System.Web.Http.HttpPost]
        public MessagesOutPut Registration(RegisterUserInput input)
        {
            input.RegisterIp = input.RegisterIp.Contains(",") ? input.RegisterIp.Split(',')[0] : input.RegisterIp;
            if (_registerUserAppApiService.Registered(input))
            {
                return new MessagesOutPut { Message = "注册成功", Success = true };
            }
            else
            {
                return new MessagesOutPut { Message = "注册失败", Success = false };
            }
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <returns></returns>

        [System.Web.Http.HttpPost]
        public MessagesOutPut ForgetPassword(RegisterUserInput input)
        {
            return _registerUserAppApiService.ForgetPassword(input);
        }

        /// <summary>
        /// 修改用户绑定手机号
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public MessagesOutPut ModifyUserPhone(RegisterUserInput input)
        {
            return _registerUserAppApiService.ModifyUserPhone(input);
        }

        /// <summary>
        /// 用户类
        /// </summary>
        protected RegisterUserOutputDto UserObject
        {
            get
            {
                if (Request.Headers.Authorization?.Parameter != null)
                {
                    var authorization = Request.Headers.Authorization.Parameter;
                    try
                    {
                        //解密Ticket
                        var formsAuthenticationTicket = Des3Cryption.Decrypt3DES(authorization);
                        if (!string.IsNullOrEmpty(formsAuthenticationTicket))
                        {
                            var strTicket = formsAuthenticationTicket;
                            //从Ticket里面获取用户名和密码
                            string telphoneNum = "";
                            if (!string.IsNullOrEmpty(strTicket))
                            {
                                var index = strTicket.Split('&');
                                telphoneNum = index[0];
                            }
                            var model = new RegisterUserRepository().GetLogin(telphoneNum);
                            return model;
                        }
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                    return null;
                }
                return null;
            }
        }

        /// <summary>
        /// 根据微信支付状态修改订单状态
        /// </summary>
        /// <param name="input"></param>
        [System.Web.Http.HttpPost]
        public MessagesOutPut EnditSheetStates(SheetModelInput input)
        {
            return sheetApiService.EnditSheetStates(input);
        }
    }
}
