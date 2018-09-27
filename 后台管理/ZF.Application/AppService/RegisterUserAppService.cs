using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Core;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Application.Dto;
using Dapper;
using System.Data;
using ZF.AutoMapper.AutoMapper;
using ZF.Application.BaseDto;
using ZF.Infrastructure;
using ZF.Infrastructure.Md5;
using ZF.Infrastructure.SmsService;

namespace ZF.Application.AppService
{
    public class RegisterUserAppService : BaseAppService<RegisterUser>
    {

        private readonly IRegisterUserRepository _repository;

        private readonly ISmsendLogRepository _iSmsendLogRepository;

        public RegisterUserAppService(IRegisterUserRepository repository, ISmsendLogRepository iSmsendLogRepository) : base(repository)
        {
            _repository = repository;
            _iSmsendLogRepository = iSmsendLogRepository;
        }

        /// <summary>
        /// 查询用户注册信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<RegisterUserOutput> GetList(RegisterUserListInput input, out int count)
        {
            const string sql = "select  * ";
            var strSql = new StringBuilder(" from t_Base_RegisterUser  where 1 = 1 ");
            var dynamicParameters = new DynamicParameters();
            const string sqlCount = "select count(*) ";
            if (!string.IsNullOrEmpty(input.Id))
            {
                strSql.Append(" and Id = @Id ");
                dynamicParameters.Add(":Id", input.Id, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.UserName))
            {
                strSql.Append(" and UserName like  @UserName ");
                dynamicParameters.Add(":UserName", '%' + input.UserName + '%', DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.NickNamw))
            {
                strSql.Append(" and  ( NickNamw like  @NickNamw or TelphoneNum like  @NickNamw )  ");
                dynamicParameters.Add(":NickNamw", '%' + input.NickNamw + '%', DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<RegisterUserOutput>(GetPageSql(sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord), dynamicParameters);

            return list;
        }

        /// <summary>
        /// 所有用户列表
        /// </summary>
        /// <returns></returns>
        public List<RegisterUserOutput> GetList()
        {
            var sql = "select  * from t_Base_RegisterUser  where 1 = 1 ";
            var list = Db.QueryList<RegisterUserOutput>(sql, null);
            return list;
        }


        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="uid">用户Id</param>
        /// <returns></returns>
        public RegisterUser GetOne(string uid)
        {
            return _repository.Get(uid);
        }
        /// <summary>
        /// 修改用户基本信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut UpdateOne(RegisterUser input)
        {
            try
            {
                RegisterUser model;
                if (!string.IsNullOrEmpty(input.Id))
                {
                    model = _repository.Get(input.Id);
                    model.Id = input.Id;
                    model.QQ = input.QQ;
                    model.Email = input.Email;
                    model.HeadImage = string.IsNullOrEmpty(input.HeadImage) ? model.HeadImage : input.HeadImage;
                    model.NickNamw = input.NickNamw;
                    model.TelphoneNum = string.IsNullOrEmpty(input.TelphoneNum) ? model.TelphoneNum : input.TelphoneNum;
                    _repository.Update(model);
                }
                else
                {
                    return new MessagesOutPut { Success = false, Message = "修改失败" };
                }

                return new MessagesOutPut { Success = true, Message = "修改成功" };
            }
            catch (Exception)
            {
                return new MessagesOutPut { Success = false, Message = "修改失败" };
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="Phone"></param>
        /// <returns></returns>

        public string AddRegiest(string Phone)
        {

            var strSql = "SELECT * FROM dbo.t_Base_RegisterUser WHERE State=0 AND TelphoneNum=@TelphoneNum  ";
            var dy = new DynamicParameters();
            dy.Add("TelphoneNum", Phone, DbType.String);
            var model = Db.QueryFirstOrDefault<RegisterUser>(strSql, dy);
            if (model != null)
            {
                return model.Id;
            }
            var name = SendSmsService.GetRandomString(25);
            Random rd = new Random();
            int num = rd.Next(100000, 1000000);
            var smsMessageOutPut = SendSmsService.SendSms1(Phone, num.ToString());
            if (smsMessageOutPut.Code == 200)
            {
                var failureTime = ConfigurationManager.AppSettings["FailureTime"];
                SmsendLog model1 = new SmsendLog();
                model1.Id = Guid.NewGuid().ToString();
                model1.TelphoneNum = smsMessageOutPut.PhoneNumber;
                model1.SendTime = DateTime.Now;
                model1.FailureTime = Convert.ToDateTime(model1.SendTime.AddSeconds(Convert.ToInt32(failureTime)));
                model1.Code = smsMessageOutPut.VerificationCode;
                _iSmsendLogRepository.InsertGetId(model1);
            }
            return _repository.InsertGetId(new RegisterUser
            {
                TelphoneNum = Phone,
                RegiesterType = "houtai",
                Id = Guid.NewGuid().ToString(),
                AddTime = DateTime.Now,
                HeadImage = "http://zhuofancbs.oss-cn-shenzhen.aliyuncs.com/small.jpg",
                IsBindWiki = 0,
                LoginPwd = Md5.GetMd5(num.ToString()),
                State = (int)QuestionState.Enable,
                NickNamw = name,
                UserName = name,
                Sex = "0",
                RegisterIp = GetAddressIp()
            });

        }

        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public MessagesOutPut UpdateUserSate(IdInputIds id)
        {
            RegisterUser model;
            try
            {
                foreach (string item in id.Ids.Split(','))
                {
                    model = _repository.Get(item.Split('#')[0]);
                    model.Id = item.Split('#')[0];
                    model.State = Convert.ToInt32(item.Split('#')[1]);
                    _repository.Update(model);
                }
                return new MessagesOutPut { Success = true, Message = "修改成功" };
            }
            catch
            {

                return new MessagesOutPut { Success = false, Message = "修改失败" };
            }
        }
        /// <summary>
        /// 根据注册终端统计用户数量
        /// </summary>
        /// <returns></returns>
        public List<RegisterUserOutput> GetRegisterUserCountByType(string addtime)
        {
            var sql = " select RegiesterType,COUNT(RegiesterType)RegiesterCount,Year(addtime)RegisYear ";

            var countsql = string.Empty;
            for (int i = 1; i <= 12; i++)
            {
                sql += ",case when MONTH(addtime)=" + i + " then COUNT(RegiesterType) else 0  end  month" + i + "";

                if (string.IsNullOrEmpty(countsql))
                {
                    countsql = ",(cast(sum(month" + i + ") as nvarchar(10))";
                }
                else
                {
                    countsql += " +','+ cast(sum(month" + i + ") as nvarchar(10))";
                }


            }
            countsql += " )as RegiesterCount ";

            var strSql = new StringBuilder(" from t_Base_RegisterUser  where 1 = 1 ");
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrEmpty(addtime))
            {
                strSql.Append(" and year(addtime) = @addtime ");
                dynamicParameters.Add(":addtime", addtime, DbType.String);
            }
            else
            {
                strSql.Append(" and year(addtime) = @addtime ");
                dynamicParameters.Add(":addtime", DateTime.Now.Year.ToString(), DbType.String);
            }

            strSql.Append(" group by RegiesterType ,AddTime  ");

            sql = " select RegisYear,RegiesterType" + countsql + " from (" + sql + strSql + ")a group by RegiesterType ,RegisYear";
            var list = Db.QueryList<RegisterUserOutput>(sql, dynamicParameters);
            return list;
        }
    }
}
