
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
    /// 数据表实体应用服务现实：CourseSuitDetail 
    /// </summary>
    public class CourseSuitDetailAppService : BaseAppService<CourseSuitDetail>
    {
	   private readonly ICourseSuitDetailRepository _iCourseSuitDetailRepository;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


	   /// <summary>
	   /// 构造函数
	   /// </summary>
	   /// <param name="iCourseSuitDetailRepository"></param>
	   /// <param name="operatorLogAppService"></param>
	   public CourseSuitDetailAppService(ICourseSuitDetailRepository iCourseSuitDetailRepository,OperatorLogAppService operatorLogAppService): base(iCourseSuitDetailRepository)
	   {
			_iCourseSuitDetailRepository = iCourseSuitDetailRepository;
			_operatorLogAppService = operatorLogAppService;
	   }
	
	   /// <summary>
       /// 查询列表实体：CourseSuitDetail 
       /// </summary>
	   public  List<CourseSuitDetailOutput> GetList(CourseSuitDetailListInput input, out int count)
	   {
		  const string sql = "select  a.* ";
          var strSql = new StringBuilder(" from t_Course_SuitDetail  a  where a.IsDelete=0  ");
		  const string sqlCount = "select count(*) ";
          var dynamicParameters = new DynamicParameters();
          if (!string.IsNullOrWhiteSpace(input.Name))
          {
              strSql.Append(" and a.Name = @Name ");
              dynamicParameters.Add(":Name", input.Name, DbType.String);
          }
		  count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
          var list = Db.QueryList<CourseSuitDetailOutput>(GetPageSql(sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord), dynamicParameters);
          return list;
	   }

	   /// <summary>
        /// 新增实体  CourseSuitDetail
        /// </summary>
        public MessagesOutPut AddOrEdit(CourseSuitDetailInput input)
        {
            CourseSuitDetail model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iCourseSuitDetailRepository.Get(input.Id);
				#region 修改逻辑
				model.Id = input.Id;


                model.UpdateUserId = UserObject.Id;
                model.UpdateTime = DateTime.Now;
                #endregion
                _iCourseSuitDetailRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.CourseSuitDetail,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改CourseSuitDetail:"
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<CourseSuitDetail>();
			model.Id = Guid.NewGuid().ToString();
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            var keyId = _iCourseSuitDetailRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.CourseSuitDetail,
                OperatorType =(int) OperatorType.Add,
                Remark = "新增CourseSuitDetail:" 
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }
	   
    }
}

