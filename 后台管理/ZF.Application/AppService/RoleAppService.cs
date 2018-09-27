
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
    /// 数据表实体应用服务现实：角色表 
    /// </summary>
    public class RoleAppService : BaseAppService<Role>
    {
        private readonly IRoleRepository _iRoleRepository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iRoleRepository"></param>
        /// <param name="operatorLogAppService"></param>
        public RoleAppService(IRoleRepository iRoleRepository, OperatorLogAppService operatorLogAppService) : base(iRoleRepository)
        {
            _iRoleRepository = iRoleRepository;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询列表实体：角色表 
        /// </summary>
        public List<RoleOutput> GetList(RoleListInput input, out int count)
        {
            const string sql = "select  a.* ";
            var strSql = new StringBuilder(" from t_Base_Role  a  where a.IsDelete=0  ");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();
           
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<RoleOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 新增实体  角色表
        /// </summary>
        public MessagesOutPut AddOrEdit(RoleInput input)
        {
            Role model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iRoleRepository.Get(input.Id);
                #region 修改逻辑
                model.Id = input.Id;
                model.Description = input.Description;
                model.RoleName = model.RoleName;
                #endregion
                _iRoleRepository.Update(model);
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<Role>();
            model.Id = Guid.NewGuid().ToString();
            model.AddTime = DateTime.Now;
            model.AddUserId = UserObject.Id;
            var keyId = _iRoleRepository.InsertGetId(model);
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

    }
}

