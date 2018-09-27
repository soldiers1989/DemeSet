
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
    /// 数据表实体应用服务现实：角色菜单关系表 
    /// </summary>
    public class RoleMenuAppService : BaseAppService<RoleMenu>
    {
        private readonly IRoleMenuRepository _iRoleMenuRepository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iRoleMenuRepository"></param>
        /// <param name="operatorLogAppService"></param>
        public RoleMenuAppService(IRoleMenuRepository iRoleMenuRepository, OperatorLogAppService operatorLogAppService) : base(iRoleMenuRepository)
        {
            _iRoleMenuRepository = iRoleMenuRepository;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询列表实体：角色菜单关系表 
        /// </summary>
        public List<RoleMenuOutput> GetList(RoleMenuListInput input)
        {
            const string sql = "select  a.* ";
            var strSql = new StringBuilder(" from t_Base_RoleMenu  a  where 1=1   ");
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrEmpty(input.RoleId))
            {
                strSql.Append(" and a.RoleId=@RoleId");
                dynamicParameters.Add(":RoleId", input.RoleId, DbType.String);
            }
            var list = Db.QueryList<RoleMenuOutput>(sql + strSql, dynamicParameters);
            return list;
        }

        /// <summary>
        /// 新增实体  角色菜单关系表
        /// </summary>
        public MessagesOutPut AddOrEdit(RoleMenuInput input)
        {
            var str = $"DELETE t_base_RoleMenu WHERE RoleId='{input.Id}'";
            Db.ExecuteNonQuery(str, null);
            if (!string.IsNullOrEmpty(input.Ids))
            {
                var ids = input.Ids;
                var list = ids.Split(';');
                foreach (var item in list.Where(item => !string.IsNullOrEmpty(item)))
                {
                    _iRoleMenuRepository.Insert(new RoleMenu
                    {
                        Id = Guid.NewGuid().ToString(),
                        MenuId = item.Split(',')[0],
                        RoleId = input.Id,
                        Type = int.Parse(item.Split(',')[1])
                    });
                }
            }
            return new MessagesOutPut
            {
                Id = -1,
                Message = "保存成功!",
                Success = true
            };
        }

    }
}

