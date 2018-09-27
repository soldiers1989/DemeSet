using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using ZF.Application.Dto;
using ZF.AutoMapper.AutoMapper;
using ZF.Core;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure;
using ZF.Infrastructure.Enum;

namespace ZF.Application.AppService
{
    /// <summary>
    /// 
    /// </summary>
    public class OperatorLogAppService : BaseAppService<OperatorLog>
    {
        private readonly IOperatorLogRepository _iOperatorLogRepository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public OperatorLogAppService(IOperatorLogRepository repository) : base(repository)
        {
            _iOperatorLogRepository = repository;
        }

        /// <summary>
        /// 新增实体：Operatorlog 
        /// </summary>
        public void Add(OperatorLogInput input)
        {
            input.OperatorDate = DateTime.Now;
            if (UserObject?.Id != null)
            {
                input.OperatorId = UserObject.Id;
                input.OperatorName = UserObject.UserName;
            }
            input.OperatorIp = GetAddressIp();
            var item = input.MapTo<OperatorLog>();
            item.Id=Guid.NewGuid().ToString();
            _iOperatorLogRepository.Insert(item);
        }

        /// <summary>
        /// 获取操作者日志：Operatorlog 
        /// </summary>
        public List<OperatorLogOutput> GetList(GetListOperatorLogInput input, out int count)
        {
            const string sql = "select  * ";
            var strSql = new StringBuilder(" from t_Base_OperatorLog  where 1 = 1 ");
            var dynamicParameters = new DynamicParameters();
            const string sqlCount = "select count(*) ";
            if (input.BeginDateTime.HasValue)
            {
                strSql.Append(" and OperatorDate >= @BeginDateTime ");
                dynamicParameters.Add(":BeginDateTime", input.BeginDateTime.Value, DbType.DateTime);
            }
            if (input.EndDateTime.HasValue)
            {
                strSql.Append(" and OperatorDate <= @EndDateTime ");
                dynamicParameters.Add(":EndDateTime", input.EndDateTime.Value, DbType.DateTime);
            }
            if (input.ModuleType.HasValue)
            {
                strSql.Append(" and ModuleId = @ModuleType ");
                dynamicParameters.Add(":ModuleType", input.ModuleType.Value, DbType.Int32);
            }
            if (input.OperatorType.HasValue)
            {
                strSql.Append(" and OperatorType = @OperatorType ");
                dynamicParameters.Add(":OperatorType", input.OperatorType.Value, DbType.Int32);
            }
            if (!string.IsNullOrWhiteSpace(input.Content))
            {
                strSql.Append(" and Remark like  @Content ");
                dynamicParameters.Add(":Content", '%' + input.Content + '%', DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.OperatorName))
            {
                strSql.Append(" and OperatorName like  @OperatorName ");
                dynamicParameters.Add(":OperatorName", '%' + input.OperatorName + '%', DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<OperatorLogOutput>(GetPageSql(sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord), dynamicParameters);
            foreach (var item in list)
            {
                item.ModuleName = EnumHelper.GetEnumName<Model>(item.ModuleId);
                item.OperatorTypeName = EnumHelper.GetEnumName<OperatorType>(item.OperatorType.ToString());
            }
            return list;
        }
    }
}