
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

namespace ZF.Application.AppService
{
    /// <summary>
    /// 数据表实体应用服务现实：OperatorLog 
    /// </summary>
    public class OperatorLogAppService : BaseAppService<OperatorLog>
    {
	   private readonly IOperatorLogRepository _iOperatorLogRepository;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


	   /// <summary>
	   /// 构造函数
	   /// </summary>
	   /// <param name="iOperatorLogRepository"></param>
	   /// <param name="operatorLogAppService"></param>
	   public OperatorLogAppService(IOperatorLogRepository iOperatorLogRepository,OperatorLogAppService operatorLogAppService): base(iOperatorLogRepository)
	   {
			_iOperatorLogRepository = iOperatorLogRepository;
			_operatorLogAppService = operatorLogAppService;
	   }
	
	   /// <summary>
       /// 查询列表实体：OperatorLog 
       /// </summary>
	   public  List<OperatorLogOutput> GetList(OperatorLogListInput input, out int count)
	   {
		  const string sql = "select  a.* ";
          var strSql = new StringBuilder(" from t_Base_OperatorLog  a  where a.IsDelete=0  ");
		  const string sqlCount = "select count(*) ";
          var dynamicParameters = new DynamicParameters();
          if (!string.IsNullOrWhiteSpace(input.Name))
          {
              strSql.Append(" and a.Name = @Name ");
              dynamicParameters.Add(":Name", input.Name, DbType.String);
          }
		  count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
          var list = Db.QueryList<OperatorLogOutput>(GetPageSql(sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord), dynamicParameters);
          return list;
	   }

	   /// <summary>
        /// 新增实体  OperatorLog
        /// </summary>
        public MessagesOutPut AddOrEdit(OperatorLogInput input)
        {
            OperatorLog model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iOperatorLogRepository.Get(input.Id);
				#region 修改逻辑
				model.Id = input.Id;


                model.UpdateUserId = UserObject.Id;
                model.UpdateTime = DateTime.Now;
                #endregion
                _iOperatorLogRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.OperatorLog,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改OperatorLog:"
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<OperatorLog>();
			model.Id = Guid.NewGuid().ToString();
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            var keyId = _iOperatorLogRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.OperatorLog,
                OperatorType =(int) OperatorType.Add,
                Remark = "新增OperatorLog:" 
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }
	   
    }
}

