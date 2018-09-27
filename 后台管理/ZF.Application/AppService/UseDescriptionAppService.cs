
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
    /// 数据表实体应用服务现实：使用指南 
    /// </summary>
    public class UseDescriptionAppService : BaseAppService<UseDescription>
    {
	   private readonly IUseDescriptionRepository _iUseDescriptionRepository;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


	   /// <summary>
	   /// 构造函数
	   /// </summary>
	   /// <param name="iUseDescriptionRepository"></param>
	   /// <param name="operatorLogAppService"></param>
	   public UseDescriptionAppService(IUseDescriptionRepository iUseDescriptionRepository,OperatorLogAppService operatorLogAppService): base(iUseDescriptionRepository)
	   {
			_iUseDescriptionRepository = iUseDescriptionRepository;
			_operatorLogAppService = operatorLogAppService;
	   }
	
	   /// <summary>
       /// 查询列表实体：使用指南 
       /// </summary>
	   public  List<UseDescriptionOutput> GetList(UseDescriptionListInput input, out int count)
	   {
		  const string sql = "select  a.* ,b.Name as ClassName,c.DataTypeName as BigClassName ";
          var strSql = new StringBuilder(" from T_Base_UseDescription  a  left join t_Base_Basedata b on a.ClassId=b.Id  left join  t_Base_DataType c on a.BigClassId=c.Id  where a.IsDelete=0  ");
		  const string sqlCount = "select count(*) ";
          var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.BigClassId))
            {
                strSql.Append(" and a.BigClassId = @BigClassId ");
                dynamicParameters.Add(":BigClassId", input.BigClassId, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.ClassId))
            {
                strSql.Append(" and a.ClassId = @ClassId ");
                dynamicParameters.Add(":ClassId", input.ClassId, DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
          var list = Db.QueryList<UseDescriptionOutput>(GetPageSql(sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord), dynamicParameters);
          return list;
	   }

	   /// <summary>
        /// 新增实体  使用指南
        /// </summary>
        public MessagesOutPut AddOrEdit(UseDescriptionInput input)
        {
            UseDescription model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iUseDescriptionRepository.Get(input.Id);
				#region 修改逻辑
				model.Id = input.Id;

                model.BigClassId = input.BigClassId;
                model.ClassId = input.ClassId;
                model.Content = input.Content;
                model.UpdateUserId = UserObject.Id;
                model.UpdateTime = DateTime.Now;
                #endregion
                _iUseDescriptionRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.UseDescription,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改使用指南:"
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<UseDescription>();
			model.Id = Guid.NewGuid().ToString();
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            var keyId = _iUseDescriptionRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.UseDescription,
                OperatorType =(int) OperatorType.Add,
                Remark = "新增使用指南:" 
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }
	   
    }
}

