
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure;
using ZF.Infrastructure.SmsService;
using System.Configuration;

namespace ZF.Application.AppService
{
    /// <summary>
    /// 数据表实体应用服务现实：SmsendLog 
    /// </summary>
    public class SmsendLogAppService : BaseAppService<SmsendLog>
    {
        private readonly ISmsendLogRepository _iSmsendLogRepository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iSmsendLogRepository"></param>
        /// <param name="operatorLogAppService"></param>
        public SmsendLogAppService(ISmsendLogRepository iSmsendLogRepository, OperatorLogAppService operatorLogAppService) : base(iSmsendLogRepository)
        {
            _iSmsendLogRepository = iSmsendLogRepository;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询列表实体：SmsendLog 
        /// </summary>
        public List<SmsendLogOutput> GetList(SmsendLogListInput input, out int count)
        {
            const string sql = "select  a.* ";
            var strSql = new StringBuilder(" from t_Base_SmsendLog  a  where 1=1  ");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.TelphoneNum))
            {
                strSql.Append(" and a.TelphoneNum like @TelphoneNum ");
                dynamicParameters.Add(":TelphoneNum", "%" + input.TelphoneNum + "%", DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.BeginDateTime) && !string.IsNullOrWhiteSpace(input.EndDateTime))
            {
                strSql.Append(" and CONVERT(varchar(12),a.SendTime,23) between @BeginDateTime and @EndDateTime ");
                dynamicParameters.Add(":BeginDateTime", input.BeginDateTime, DbType.String);
                dynamicParameters.Add(":EndDateTime", input.EndDateTime, DbType.String);
            }
            else if (!string.IsNullOrWhiteSpace(input.BeginDateTime) && string.IsNullOrWhiteSpace(input.EndDateTime))
            {
                strSql.Append(" and CONVERT(varchar(12),a.SendTime,23) >= @BeginDateTime ");
                dynamicParameters.Add(":BeginDateTime", input.BeginDateTime, DbType.String);
            }
            else if (string.IsNullOrWhiteSpace(input.BeginDateTime) && !string.IsNullOrWhiteSpace(input.EndDateTime))
            {
                strSql.Append(" and CONVERT(varchar(12),a.SendTime,23) <= @EndDateTime ");
                dynamicParameters.Add(":EndDateTime", input.EndDateTime, DbType.String);
            }

            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<SmsendLogOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 短信发送
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut SendSmsInfo(SmsModel input)
        {
            var smsMessageOutPut = SendSmsService.SendSms(input);
            var failureTime = ConfigurationManager.AppSettings["FailureTime"];
            //短信发送成功
            if (smsMessageOutPut.Code == 200)
            {
                SmsendLog model = new SmsendLog();
                model.Id = Guid.NewGuid().ToString();
                model.TelphoneNum = smsMessageOutPut.PhoneNumber;
                model.SendTime = DateTime.Now;
                model.FailureTime = Convert.ToDateTime(model.SendTime.AddSeconds(Convert.ToInt32(failureTime)));
                model.Code = smsMessageOutPut.VerificationCode;
                _iSmsendLogRepository.InsertGetId(model);
                //_operatorLogAppService.Add(new OperatorLogInput
                //{
                //    KeyId = model.Id,
                //    ModuleId = (int)Model.SmsendLog,
                //    OperatorType = (int)OperatorType.Add,
                //    Remark = "短信发送成功!" + model.FailureTime.ToString("yyyy-MM-dd hh:mm:ss")
                //});
                return new MessagesOutPut
                {
                    Success = true,
                    Message = "短信发送成功!",
                };
            }
            //_operatorLogAppService.Add(new OperatorLogInput
            //{
            //    KeyId = smsMessageOutPut.PhoneNumber,
            //    ModuleId = (int)Model.SmsendLog,
            //    OperatorType = (int)OperatorType.Add,
            //    Remark = smsMessageOutPut.LogMessage
            //});
            return new MessagesOutPut { Success = false, Message = "短信发送失败!" };
        }
    }
}

