
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
    /// 数据表实体应用服务现实：MyErrorSubject 
    /// </summary>
    public class MyErrorSubjectAppService : BaseAppService<MyErrorSubject>
    {
	   private readonly IMyErrorSubjectRepository _iMyErrorSubjectRepository;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


	   /// <summary>
	   /// 构造函数
	   /// </summary>
	   /// <param name="iMyErrorSubjectRepository"></param>
	   /// <param name="operatorLogAppService"></param>
	   public MyErrorSubjectAppService(IMyErrorSubjectRepository iMyErrorSubjectRepository,OperatorLogAppService operatorLogAppService): base(iMyErrorSubjectRepository)
	   {
			_iMyErrorSubjectRepository = iMyErrorSubjectRepository;
			_operatorLogAppService = operatorLogAppService;
	   }
	
	   /// <summary>
       /// 查询列表实体：MyErrorSubject 
       /// </summary>
	   public  List<MyErrorSubjectOutput> GetList(MyErrorSubjectListInput input, out int count)
	   {
		  const string sql = "select  a.* ";
          var strSql = new StringBuilder(" from t_My_ErrorSubject  a  where a.IsDelete=0  ");
		  const string sqlCount = "select count(*) ";
          var dynamicParameters = new DynamicParameters();
          if (!string.IsNullOrWhiteSpace(input.Name))
          {
              strSql.Append(" and a.Name = @Name ");
              dynamicParameters.Add(":Name", input.Name, DbType.String);
          }
		  count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
          var list = Db.QueryList<MyErrorSubjectOutput>(GetPageSql(sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord), dynamicParameters);
          return list;
	   }

	   /// <summary>
        /// 新增实体  MyErrorSubject
        /// </summary>
        public MessagesOutPut AddOrEdit(MyErrorSubjectInput input)
        {
            MyErrorSubject model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iMyErrorSubjectRepository.Get(input.Id);
				#region 修改逻辑
				model.Id = input.Id;


                model.UpdateUserId = UserObject.Id;
                model.UpdateTime = DateTime.Now;
                #endregion
                _iMyErrorSubjectRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.MyErrorSubject,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改MyErrorSubject:"
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<MyErrorSubject>();
			model.Id = Guid.NewGuid().ToString();
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            var keyId = _iMyErrorSubjectRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.MyErrorSubject,
                OperatorType =(int) OperatorType.Add,
                Remark = "新增MyErrorSubject:" 
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }
	   
    }
}

