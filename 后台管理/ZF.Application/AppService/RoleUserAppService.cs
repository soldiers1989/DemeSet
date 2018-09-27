
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
using System.Linq;

namespace ZF.Application.AppService
{
    /// <summary>
    /// 数据表实体应用服务现实：角色人员关系表 
    /// </summary>
    public class RoleUserAppService : BaseAppService<RoleUser>
    {
        private readonly IRoleUserRepository _iRoleUserRepository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iRoleUserRepository"></param>
        /// <param name="operatorLogAppService"></param>
        public RoleUserAppService(IRoleUserRepository iRoleUserRepository, OperatorLogAppService operatorLogAppService) : base(iRoleUserRepository)
        {
            _iRoleUserRepository = iRoleUserRepository;
            _operatorLogAppService = operatorLogAppService;
        }


        /// <summary>
        /// 查询列表实体：角色人员关系表 
        /// </summary>
        public List<RoleUserOutput> GetList1(RoleUserListInput input, out int count)
        {
            const string sql = @"SELECT b.Id, b.Id AS UserId ,
        b.UserName ,
        b.LoginName ,
        b.Phone ";
            var strSql = new StringBuilder(@" FROM    t_Base_User b
WHERE   1 = 1
        AND b.IsAdmin = 1
        AND b.IsDelete = 0
        AND b.Id NOT IN (
        SELECT  a.UserId
        FROM    t_base_RoleUser a
        WHERE   a.RoleId = @RoleId ) ");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add(":RoleId", input.RoleId, DbType.String);
            if (!string.IsNullOrEmpty(input.UserName))
            {
                strSql.Append(" and b.UserName like @UserName");
                dynamicParameters.Add(":UserName", '%' + input.UserName + '%', DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<RoleUserOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 查询列表实体：角色人员关系表 
        /// </summary>
        public List<RoleUserOutput> GetList(RoleUserListInput input, out int count)
        {
            const string sql = "select  a.*,b.UserName,b.LoginName,b.Phone ";
            var strSql = new StringBuilder(" from t_Base_RoleUser  a left join t_Base_User b on a.UserId=b.Id   where 1=1  and b.IsAdmin=1 and b.IsDelete=0 ");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrEmpty(input.RoleId))
            {
                strSql.Append(" and a.RoleId=@RoleId");
                dynamicParameters.Add(":RoleId", input.RoleId, DbType.String);
            }
            if (!string.IsNullOrEmpty(input.UserName))
            {
                strSql.Append(" and b.UserName like @UserName");
                dynamicParameters.Add(":UserName", '%' + input.UserName + '%', DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<RoleUserOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 新增实体  角色人员关系表
        /// </summary>
        public MessagesOutPut AddOrEdit(RoleUserInput input)
        {
            if (!string.IsNullOrEmpty(input.UserIds))
            {
                var ids = input.UserIds;
                var list = ids.Split(',');
                foreach (var item in list.Where(item => !string.IsNullOrEmpty(item)))
                {
                    _iRoleUserRepository.Insert(new RoleUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = item,
                        RoleId = input.RoleId,
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

