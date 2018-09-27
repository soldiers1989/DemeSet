
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
    /// 数据表实体应用服务现实：用户菜单关系表 
    /// </summary>
    public class UserMenuAppService : BaseAppService<UserMenu>
    {
	   private readonly IUserMenuRepository _iUserMenuRepository;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


	   /// <summary>
	   /// 构造函数
	   /// </summary>
	   /// <param name="iUserMenuRepository"></param>
	   /// <param name="operatorLogAppService"></param>
	   public UserMenuAppService(IUserMenuRepository iUserMenuRepository,OperatorLogAppService operatorLogAppService): base(iUserMenuRepository)
	   {
			_iUserMenuRepository = iUserMenuRepository;
			_operatorLogAppService = operatorLogAppService;
	   }
	
	   /// <summary>
       /// 查询列表实体：用户菜单关系表 
       /// </summary>
	   public  List<UserMenuOutput> GetList(UserMenuListInput input, out int count)
	   {
		  const string sql = "select  a.* ";
          var strSql = new StringBuilder(" from t_Base_UserMenu  a  where 1=1  ");
		  const string sqlCount = "select count(*) ";
          var dynamicParameters = new DynamicParameters();
		  count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
          var list = Db.QueryList<UserMenuOutput>(GetPageSql(sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord), dynamicParameters);
          return list;
	   }

	   /// <summary>
        /// 新增实体  用户菜单关系表
        /// </summary>
        public MessagesOutPut AddOrEdit(UserMenuInput input)
        {
            UserMenu model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iUserMenuRepository.Get(input.Id);
				#region 修改逻辑
				model.Id = input.Id;


                #endregion
                _iUserMenuRepository.Update(model);
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<UserMenu>();
			model.Id = Guid.NewGuid().ToString();
            var keyId = _iUserMenuRepository.InsertGetId(model);
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }
	   
    }
}

