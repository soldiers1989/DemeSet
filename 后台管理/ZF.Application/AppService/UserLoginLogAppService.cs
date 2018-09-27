using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure;
using ZF.Infrastructure.Enum;

namespace ZF.Application.AppService
{
    public class UserLoginLogAppService : BaseAppService<UserLoginLog>
    {
        private readonly OperatorLogAppService _operatorLogAppService;
        private readonly IUserLoginLogRepository _repository;

        public UserLoginLogAppService(IUserLoginLogRepository respository, OperatorLogAppService logService) : base(respository)
        {
            _repository = respository;
            _operatorLogAppService = logService;
        }


        /// <summary>
        /// 添加用户登陆日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut AddLoginLog(UserLoginLogInput input, string TelphoneNum)
        {
            var ip = GetAddressIp();
            var id = _repository.InsertGetId(new UserLoginLog
            {
                Id = Guid.NewGuid().ToString(),
                LoginTime = DateTime.Now,
                LoginType = input.LoginType,
                UserId = input.UserId,
                LoginIp = ip
            });

            var loginName = string.Empty;
            switch (input.LoginType)
            {
                case 0:
                    loginName = "密码登录";
                    break;
                case 1:
                    loginName = "短信登录";
                    break;
                case 2:
                    loginName = "扫码登录";
                    break;
            }

            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = id,
                ModuleId = (int)Model.UserLoginLog,
                OperatorType = (int)OperatorType.Add,
                Remark = "添加用户登陆日志：手机号码或微信用户:" + TelphoneNum + "通过" + loginName
            });
            return new MessagesOutPut { Success = true, Message = "添加成功" };
        }

        /// <summary>
        /// 获取登陆日志
        /// </summary>
        public List<UserLoginLogOutput> GetList(UserLoginLogListInput input, out int count)
        {
            const string sql = " SELECT DISTINCT a.Id,b.TelphoneNum as TelphoneNum,LoginTime,LoginIp,LoginType,b.NickNamw  ";
            var strSql = new StringBuilder(" FROM dbo.t_Base_UserLoginLog a LEFT JOIN dbo.t_Base_RegisterUser b ON a.UserId=b.Id   where 1=1 and b.TelphoneNum is not null ");
            var dynamicParameters = new DynamicParameters();
            //if(!string.IsNullOrWhiteSpace( input.UserId )){
            //    strSql.Append( " and a.UserId=@UserId " );
            //    dynamicParameters.Add( ":UserId",input.UserId,DbType.String);
            //}

            if (input.BeginDateTime != null && input.BeginDateTime > DateTime.MinValue)
            {
                strSql.Append(" and a.LoginTime >= @BeginDateTime ");
                dynamicParameters.Add(":BeginDateTime ", input.BeginDateTime, DbType.DateTime);
            }
            if (input.EndDateTime != null && input.EndDateTime > input.BeginDateTime)
            {
                strSql.Append(" and a.LoginTime <=@EndDateTime ");
                dynamicParameters.Add(":EndDateTime ", input.EndDateTime, DbType.DateTime);
            }
            else
            {
                strSql.Append(" and a.LoginTime <@EndDateTime ");
                dynamicParameters.Add(":EndDateTime", DateTime.Now);
            }
            const string sqlCount = "select count(*) ";
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<UserLoginLogOutput>(GetPageSql(sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, "LoginTime", "desc"), dynamicParameters);
            //foreach (var item in list)
            //{
            //    item.LoginTypeName = EnumHelper.GetEnumName<OrderType>((int)(OrderType)Enum.Parse(typeof(OrderType), item.LoginType));
            //}
            return list;
        }

        /// <summary>
        /// 获取用户登陆统计信息
        /// </summary>
        /// <param name="yearPart"></param>
        /// <returns></returns>
        public UserLoginLogCountOutput GetStatics(int yearPart)
        {


            var sql = "select UserId ,";
            var strSql = new StringBuilder();
            for (int i = 1; i <= 12; i++)
            {
                strSql.Append(" (select count(*) from (select UserId,Year(LoginTime) as yearPart ,month(LoginTime) as monthPart ,LoginType from t_Base_UserLoginLog)a where a.yearPart=@yearPart and a.monthPart=" + i + "  group by a.monthPart) as Month" + i + ",");
            }
            var strTable = new StringBuilder(" FROM dbo.t_Base_UserLoginLog where 1 = 1 ");
            var dynamicPatameters = new DynamicParameters();
            dynamicPatameters.Add(":yearPart", yearPart, DbType.Int16);
            if (UserObject.IsAdmin != 1)
            {
                strTable.Append(" and UserId=@UserId ");
                dynamicPatameters.Add(":UserId ", UserObject.Id, DbType.String);
            }
            var strGroupBy = " GROUP BY UserId ";
            UserLoginLogCountOutput result = Db.QueryFirstOrDefault<UserLoginLogCountOutput>(sql + strSql.ToString().TrimEnd(',') + strTable + strGroupBy, dynamicPatameters);
            return result;
        }

        /// <summary>
        /// 分类统计不同登陆方式
        /// </summary>
        /// <param name="yearPart"></param>
        /// <returns></returns>
        public List<UserLoginLogCountOutput> GetClassStatics(int yearPart)
        {

            var list = new List<UserLoginLogCountOutput>();

            var sql1 = "select UserId ,";
            var strSql1 = new StringBuilder();
            for (int i = 1; i <= 12; i++)
            {
                strSql1.Append(" (select count(*) from (select UserId,Year(LoginTime) as yearPart ,month(LoginTime) as monthPart ,LoginType from t_Base_UserLoginLog)a where a.yearPart=@yearPart and a.monthPart=" + i + " and a.LoginType='PC'  group by a.monthPart) as Month" + i + ",");
            }
            var strTable1 = new StringBuilder(" FROM dbo.t_Base_UserLoginLog where 1 = 1 ");
            var dynamicPatameters1 = new DynamicParameters();
            dynamicPatameters1.Add(":yearPart", yearPart, DbType.Int16);
            if (UserObject.IsAdmin != 1)
            {
                strTable1.Append(" and UserId=@UserId ");
                dynamicPatameters1.Add(":UserId ", UserObject.Id, DbType.String);
            }
            var strGroupBy = " GROUP BY UserId ";
            UserLoginLogCountOutput result = Db.QueryFirstOrDefault<UserLoginLogCountOutput>(sql1 + strSql1.ToString().TrimEnd(',') + strTable1 + strGroupBy, dynamicPatameters1);

            result.IsPc = 1;
            list.Add(result);


            var sql = "select UserId ,";
            var strSql = new StringBuilder();
            for (int i = 1; i <= 12; i++)
            {
                strSql.Append(" (select count(*) from (select UserId,Year(LoginTime) as yearPart ,month(LoginTime) as monthPart ,LoginType from t_Base_UserLoginLog)a where a.yearPart=@yearPart and a.monthPart=" + i + " and a.LoginType='Phone'  group by a.monthPart) as Month" + i + ",");
            }
            var strTable = new StringBuilder(" FROM dbo.t_Base_UserLoginLog where 1 = 1 ");
            var dynamicPatameters = new DynamicParameters();
            dynamicPatameters.Add(":yearPart", yearPart, DbType.Int16);
            if (UserObject.IsAdmin != 1)
            {
                strTable.Append(" and UserId=@UserId ");
                dynamicPatameters.Add(":UserId ", UserObject.Id, DbType.String);
            }
            UserLoginLogCountOutput result2 = Db.QueryFirstOrDefault<UserLoginLogCountOutput>(sql + strSql.ToString().TrimEnd(',') + strTable + strGroupBy, dynamicPatameters);

            result2.IsPc = 0;
            list.Add(result2);
            return list;
        }

    }
}
