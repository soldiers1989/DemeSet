using System;
using System.Configuration;
using System.Data;
using Dapper;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Application.WebApiDto;
using ZF.Application.WebApiDto.SystemModule;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure;
using ZF.Infrastructure.SmsService;
using ZF.Infrastructure.Des3;

namespace ZF.Application.WebApiAppService.SystemModule
{
    /// <summary>
    /// 用户注册api服务
    /// 20180326
    /// 吴永福
    /// </summary>
    public class RegisterUserAppApiService : BaseAppService<RegisterUser>
    {
        private static string DefuleDomain = ConfigurationManager.AppSettings["DefuleDomain"];
        private readonly IRegisterUserRepository _repository;

        private readonly OperatorLogAppService _operatorLogAppService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public RegisterUserAppApiService(IRegisterUserRepository repository, OperatorLogAppService operatorLogAppService) : base(repository)
        {
            _repository = repository;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 用户注册 验证是否注册
        /// </summary>
        /// <returns></returns>
        public bool CheckIfMobileDuplicated(string telphoneNum)
        {
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add(":TelphoneNum", telphoneNum, DbType.String);
            var count =
                Db.QueryFirstOrDefault<int>(
                    "select count(1) from t_Base_RegisterUser where TelphoneNum =@TelphoneNum", dynamicParameters);
            if (count > 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RegisterUser GetUserInfo(string id)
        {
            var model = _repository.Get(id);
            return model;
        }

        /// <summary>
        /// 绑定微信
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public bool BindWiki(RegisterUser user, string userid)
        {
            string sql = @" update t_Base_RegisterUser 
                set WechatId=@WechatId, 
                    State=@state, 
                    AddTime=@AddTime, 
                    UserName=@UserName,
                    NickNamw=@NickNamw,
                    HeadImage=@HeadImage, 
                    Sex=@Sex, 
                    Province=@Province, 
                    City=@City, 
                    Country=@Country,
                IsBindWiki=1   where id=@uid ";
            var parment = new DynamicParameters();
            parment.Add(":WechatId", user.WechatId, DbType.String);
            parment.Add(":state", user.State, DbType.String);
            parment.Add(":AddTime", user.AddTime, DbType.String);
            parment.Add(":UserName", user.UserName, DbType.String);
            parment.Add(":NickNamw", user.NickNamw, DbType.String);
            parment.Add(":HeadImage", user.HeadImage, DbType.String);
            parment.Add(":Sex", user.Sex, DbType.String);
            parment.Add(":Province", user.Province, DbType.String);
            parment.Add(":City", user.City, DbType.String);
            parment.Add(":Country", user.Country, DbType.String);
            parment.Add(":uid", userid, DbType.String);
            try
            {
                Db.ExecuteNonQuery(sql, parment);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        /// <summary>
        /// 用户注册 验证是否存在
        /// </summary>
        /// <returns></returns>
        public bool CheckIfMobileDuplicatedIsTrue(string telphoneNum)
        {
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add(":TelphoneNum", telphoneNum, DbType.String);
            var count =
                Db.QueryFirstOrDefault<int>(
                    "select count(1) from t_Base_RegisterUser where TelphoneNum =@TelphoneNum", dynamicParameters);
            if (count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 修改手机号码是否是唯一
        /// </summary>
        /// <param name="telphoneNum"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ModifyUserPhoneCheck(string telphoneNum, string id)
        {
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add(":TelphoneNum", telphoneNum, DbType.String);
            dynamicParameters.Add(":Id", id, DbType.String);
            var count =
                Db.QueryFirstOrDefault<int>(
                    "select count(1) from t_Base_RegisterUser where TelphoneNum =@TelphoneNum AND Id <> @Id", dynamicParameters);
            if (count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 验证注册验证码是否正确
        /// </summary>
        /// <param name="code"></param>
        /// <param name="telphoneNum"></param>
        /// <returns></returns>
        public bool CheckRegisterMobileVerificationCode(string code, string telphoneNum)
        {
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add(":TelphoneNum", telphoneNum, DbType.String);
            dynamicParameters.Add(":Code", code, DbType.String);
            dynamicParameters.Add(":FailureTime", DateTime.Now, DbType.DateTime);
            var count =
               Db.QueryFirstOrDefault<int>(
                   "select count(1) from t_Base_SmsendLog where TelphoneNum = @TelphoneNum and Code=@Code and FailureTime>=@FailureTime", dynamicParameters);
            if (count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 验证推广码是否存在
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool VerificationExtensionCode(string code)
        {
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add(":Code", code, DbType.String);
            var count =
               Db.QueryFirstOrDefault<int>(
                   "SELECT COUNT(1) FROM dbo.t_Promote_Promoters WHERE IsDelete=0 AND  PromotionCode=@Code", dynamicParameters);
            if (count > 0)
            {
                return true;
            }
            return false;
        }




        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool Registered(RegisterUserInput input)
        {
            var model = input.MapTo<RegisterUser>();
            model.AddTime = DateTime.Now;
            model.State = (int)QuestionState.Enable;
            model.Id = Guid.NewGuid().ToString();
            model.UserName = SendSmsService.GetRandomString(25);
            model.NickNamw = model.UserName;
            if (string.IsNullOrEmpty(model.HeadImage))
                model.HeadImage = DefuleDomain + "/" + "small.jpg";
            try
            {
                _repository.Insert(model);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 微信扫码注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool WikiRegistered(RegisterUser input)
        {
            if (string.IsNullOrEmpty(input.HeadImage))
                input.HeadImage = DefuleDomain + "/" + "small.jpg";
            try
            {
                _repository.Insert(input);
                return true;
            }
            catch (Exception ex)
            {
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    Id = Guid.NewGuid().ToString(),
                    KeyId = "123",
                    ModuleId = 123,
                    Remark = ex.Message
                });
                return false;
            }
        }

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut ForgetPassword(RegisterUserInput input)
        {
            const string sql = " update t_Base_RegisterUser set LoginPwd=@LoginPwd where TelphoneNum=@TelphoneNum";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add(":LoginPwd", input.LoginPwd, DbType.String);
            dynamicParameters.Add(":TelphoneNum", input.TelphoneNum, DbType.String);

            var count = Db.ExecuteNonQuery(sql, dynamicParameters);
            if (count > 0)
            {
                return new MessagesOutPut { Message = "密码修改成功", Success = true };
            }
            else
            {
                return new MessagesOutPut { Message = "密码修改失败", Success = false };
            }

        }

        /// <summary>
        /// 修改用户绑定手机号
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut ModifyUserPhone(RegisterUserInput input)
        {
            MessagesOutPut msg = new MessagesOutPut();

            var model = _repository.Get(input.Id);
            //修改绑定手机号
            if (input.RegiesterType == "3")
            {
                bool codeIsTrue = CheckRegisterMobileVerificationCode(input.Code, input.TelphoneNum);
                if (!codeIsTrue)
                {
                    msg = new MessagesOutPut { Success = false, Message = "验证码错误!" };
                }
                else
                {
                    try
                    {
                        if (ModifyUserPhoneCheck(input.TelphoneNum, input.Id))
                        {
                            msg = new MessagesOutPut { Success = false, Message = "手机号已存在" };
                        }
                        model.TelphoneNum = input.TelphoneNum;
                        if (!string.IsNullOrEmpty(input.LoginPwd))
                        {
                            model.LoginPwd = input.LoginPwd;
                        }
                        _repository.Update(model);
                        var ticket = $"{model.Id}&{model.LoginPwd}";
                        msg = new MessagesOutPut { Success = true, Message = "绑定成功!", ModelId = Des3Cryption.Encrypt3DES(ticket) };
                    }
                    catch (Exception ex)
                    {
                        msg = new MessagesOutPut { Success = false, Message = "绑定失败" };
                    }
                }


            }//修改其他信息
            else if (input.RegiesterType == "2")
            {
                try
                {
                    model.QQ = string.IsNullOrEmpty(input.QQ) ? "" : input.QQ;
                    model.Email = string.IsNullOrEmpty(input.Email) ? "" : input.Email;
                    _repository.Update(model);
                    msg = new MessagesOutPut { Success = true, Message = "修改成功" };
                }
                catch (Exception ex)
                {

                    msg = new MessagesOutPut { Success = false, Message = "修改失败" };
                }
            }
            //修改昵称
            else if (input.RegiesterType == "1")
            {
                try
                {
                    model.NickNamw = input.NickNamw;
                    _repository.Update(model);
                    msg = new MessagesOutPut { Success = true, Message = "昵称修改成功" };
                }
                catch (Exception ex)
                {
                    msg = new MessagesOutPut { Success = false, Message = "昵称修改失败" };
                }
            }
            else if (input.RegiesterType == "4")
            {
                try
                {
                    model.HeadImage = input.HeadImage;
                    _repository.Update(model);
                    msg = new MessagesOutPut { Success = true, Message = "图像修改成功" };
                }
                catch (Exception ex)
                {
                    msg = new MessagesOutPut { Success = false, Message = "图像修改失败" };
                }
            }
            return msg;
        }

    }
}