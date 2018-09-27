
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
    /// 数据表实体应用服务现实：CourseAppraise 
    /// </summary>
    public class CourseAppraiseAppService : BaseAppService<CourseAppraise>
    {
	   private readonly ICourseAppraiseRepository _iCourseAppraiseRepository;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


	   /// <summary>
	   /// 构造函数
	   /// </summary>
	   /// <param name="iCourseAppraiseRepository"></param>
	   /// <param name="operatorLogAppService"></param>
	   public CourseAppraiseAppService(ICourseAppraiseRepository iCourseAppraiseRepository,OperatorLogAppService operatorLogAppService): base(iCourseAppraiseRepository)
	   {
			_iCourseAppraiseRepository = iCourseAppraiseRepository;
			_operatorLogAppService = operatorLogAppService;
	   }
	
	   /// <summary>
       /// 查询列表实体：CourseAppraise 
       /// </summary>
	   public  List<CourseAppraiseOutput> GetList(CourseAppraiseListInput input, out int count)
	   {
		  const string sql = "select  a.* ";
          var strSql = new StringBuilder(" from t_Course_Appraise  a  where a.IsDelete=0  ");
		  const string sqlCount = "select count(*) ";
          var dynamicParameters = new DynamicParameters();
          if (!string.IsNullOrWhiteSpace(input.Name))
          {
              strSql.Append(" and a.Name = @Name ");
              dynamicParameters.Add(":Name", input.Name, DbType.String);
          }
		  count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
          var list = Db.QueryList<CourseAppraiseOutput>(GetPageSql(sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord), dynamicParameters);
          return list;
	   }

	   /// <summary>
        /// 新增实体  CourseAppraise
        /// </summary>
        public MessagesOutPut AddOrEdit(CourseAppraiseInput input)
        {
            CourseAppraise model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iCourseAppraiseRepository.Get(input.Id);
				#region 修改逻辑
				model.Id = input.Id;


                model.UpdateUserId = UserObject.Id;
                model.UpdateTime = DateTime.Now;
                #endregion
                _iCourseAppraiseRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.CourseAppraise,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改CourseAppraise:"
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<CourseAppraise>();
			model.Id = Guid.NewGuid().ToString();
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            var keyId = _iCourseAppraiseRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.CourseAppraise,
                OperatorType =(int) OperatorType.Add,
                Remark = "新增CourseAppraise:" 
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }
	   
    }
}

