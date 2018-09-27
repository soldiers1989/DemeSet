
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
    /// 数据表实体应用服务现实：SubjectSmallquestion 
    /// </summary>
    public class SubjectSmallquestionAppService : BaseAppService<SubjectSmallquestion>
    {
	   private readonly ISubjectSmallquestionRepository _iSubjectSmallquestionRepository;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


	   /// <summary>
	   /// 构造函数
	   /// </summary>
	   /// <param name="iSubjectSmallquestionRepository"></param>
	   /// <param name="operatorLogAppService"></param>
	   public SubjectSmallquestionAppService(ISubjectSmallquestionRepository iSubjectSmallquestionRepository,OperatorLogAppService operatorLogAppService): base(iSubjectSmallquestionRepository)
	   {
			_iSubjectSmallquestionRepository = iSubjectSmallquestionRepository;
			_operatorLogAppService = operatorLogAppService;
	   }
	
	   /// <summary>
       /// 查询列表实体：SubjectSmallquestion 
       /// </summary>
	   public  List<SubjectSmallquestionOutput> GetList(SubjectSmallquestionListInput input, out int count)
	   {
		  const string sql = "select  a.* ";
          var strSql = new StringBuilder(" from t_Subject_Smallquestion  a  where a.IsDelete=0  ");
		  const string sqlCount = "select count(*) ";
          var dynamicParameters = new DynamicParameters();
          if (!string.IsNullOrWhiteSpace(input.Name))
          {
              strSql.Append(" and a.Name = @Name ");
              dynamicParameters.Add(":Name", input.Name, DbType.String);
          }
		  count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
          var list = Db.QueryList<SubjectSmallquestionOutput>(GetPageSql(sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord), dynamicParameters);
          return list;
	   }

	   /// <summary>
        /// 新增实体  SubjectSmallquestion
        /// </summary>
        public MessagesOutPut AddOrEdit(SubjectSmallquestionInput input)
        {
            SubjectSmallquestion model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iSubjectSmallquestionRepository.Get(input.Id);
				#region 修改逻辑
				model.Id = input.Id;


                model.UpdateUserId = UserObject.Id;
                model.UpdateTime = DateTime.Now;
                #endregion
                _iSubjectSmallquestionRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.SubjectSmallquestion,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改SubjectSmallquestion:"
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<SubjectSmallquestion>();
			model.Id = Guid.NewGuid().ToString();
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            var keyId = _iSubjectSmallquestionRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.SubjectSmallquestion,
                OperatorType =(int) OperatorType.Add,
                Remark = "新增SubjectSmallquestion:" 
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }
	   
    }
}

