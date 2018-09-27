
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
    /// 数据表实体应用服务现实：MyCourse 
    /// </summary>
    public class MyCourseApiService : BaseAppService<MyCourse>
    {
	   private readonly IMyCourseRepository _iMyCourseRepository;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


	   /// <summary>
	   /// 构造函数
	   /// </summary>
	   /// <param name="iMyCourseRepository"></param>
	   /// <param name="operatorLogAppService"></param>
	   public MyCourseApiService(IMyCourseRepository iMyCourseRepository,OperatorLogAppService operatorLogAppService): base(iMyCourseRepository)
	   {
			_iMyCourseRepository = iMyCourseRepository;
			_operatorLogAppService = operatorLogAppService;
	   }
	
	   /// <summary>
       /// 查询列表实体：MyCourse 
       /// </summary>
	   public  List<MyCourseOutput> GetList(MyCourseListInput input, out int count)
	   {
		  const string sql = "select  a.* ";
          var strSql = new StringBuilder(" from t_My_Course  a  where a.IsDelete=0  ");
		  const string sqlCount = "select count(*) ";
          var dynamicParameters = new DynamicParameters();
          if (!string.IsNullOrWhiteSpace(input.UserId))
          {
              strSql.Append(" and a.UserId = @UserId ");
              dynamicParameters.Add(":UserId", input.UserId, DbType.String);
          }
		  count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
          var list = Db.QueryList<MyCourseOutput>(GetPageSql(sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord), dynamicParameters);
          return list;
	   }

	   /// <summary>
        /// 新增实体  MyCourse
        /// </summary>
        public MessagesOutPut AddOrEdit(MyCourseInput input)
        {
            MyCourse model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iMyCourseRepository.Get(input.Id);
				#region 修改逻辑
				model.Id = input.Id;


                //model.UpdateUserId = UserObject.Id;
                //model.UpdateTime = DateTime.Now;
                #endregion
                _iMyCourseRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.MyCourse,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改MyCourse:"
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<MyCourse>();
			model.Id = Guid.NewGuid().ToString();
            model.AddTime = DateTime.Now;
            var keyId = _iMyCourseRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.MyCourse,
                OperatorType =(int) OperatorType.Add,
                Remark = "新增MyCourse:" 
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }
	   
    }
}

