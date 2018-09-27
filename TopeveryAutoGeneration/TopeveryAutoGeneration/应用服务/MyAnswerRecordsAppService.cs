
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
    /// 数据表实体应用服务现实：MyAnswerRecords 
    /// </summary>
    public class MyAnswerRecordsAppService : BaseAppService<MyAnswerRecords>
    {
	   private readonly IMyAnswerRecordsRepository _iMyAnswerRecordsRepository;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


	   /// <summary>
	   /// 构造函数
	   /// </summary>
	   /// <param name="iMyAnswerRecordsRepository"></param>
	   /// <param name="operatorLogAppService"></param>
	   public MyAnswerRecordsAppService(IMyAnswerRecordsRepository iMyAnswerRecordsRepository,OperatorLogAppService operatorLogAppService): base(iMyAnswerRecordsRepository)
	   {
			_iMyAnswerRecordsRepository = iMyAnswerRecordsRepository;
			_operatorLogAppService = operatorLogAppService;
	   }
	
	   /// <summary>
       /// 查询列表实体：MyAnswerRecords 
       /// </summary>
	   public  List<MyAnswerRecordsOutput> GetList(MyAnswerRecordsListInput input, out int count)
	   {
		  const string sql = "select  a.* ";
          var strSql = new StringBuilder(" from t_My_AnswerRecords  a  where a.IsDelete=0  ");
		  const string sqlCount = "select count(*) ";
          var dynamicParameters = new DynamicParameters();
          if (!string.IsNullOrWhiteSpace(input.Name))
          {
              strSql.Append(" and a.Name = @Name ");
              dynamicParameters.Add(":Name", input.Name, DbType.String);
          }
		  count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
          var list = Db.QueryList<MyAnswerRecordsOutput>(GetPageSql(sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord), dynamicParameters);
          return list;
	   }

	   /// <summary>
        /// 新增实体  MyAnswerRecords
        /// </summary>
        public MessagesOutPut AddOrEdit(MyAnswerRecordsInput input)
        {
            MyAnswerRecords model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iMyAnswerRecordsRepository.Get(input.Id);
				#region 修改逻辑
				model.Id = input.Id;


                model.UpdateUserId = UserObject.Id;
                model.UpdateTime = DateTime.Now;
                #endregion
                _iMyAnswerRecordsRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.MyAnswerRecords,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改MyAnswerRecords:"
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<MyAnswerRecords>();
			model.Id = Guid.NewGuid().ToString();
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            var keyId = _iMyAnswerRecordsRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.MyAnswerRecords,
                OperatorType =(int) OperatorType.Add,
                Remark = "新增MyAnswerRecords:" 
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }
	   
    }
}

