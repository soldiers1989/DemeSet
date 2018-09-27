using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.AutoMapper.AutoMapper;
using ZF.Core;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure;
using ZF.Infrastructure.RedisCache;

namespace ZF.Application.AppService
{
    /// <summary>
    /// 
    /// </summary>
    public class ModuleAppService : BaseAppService<Module>
    {
        private readonly IModuleRepository _repository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="operatorLogAppService"></param>
        public ModuleAppService(IModuleRepository repository, OperatorLogAppService operatorLogAppService) : base(repository)
        {
            _repository = repository;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 新增模块  sys_Module
        /// </summary>
        public MessagesOutPut AddOrEdit(Module input)
        {
            Module model;
            RedisCacheHelper.Remove("MenuModuleTree");
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _repository.Get(input.Id);
                #region 修改逻辑
                model.Id = input.Id;
                model.ModuleName = input.ModuleName;
                model.Class = input.Class;
                model.Sort = input.Sort;
                #endregion
                _repository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.Module,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改模块:" + model.ModuleName
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<Module>();
            model.Id = Guid.NewGuid().ToString();
            var keyId = _repository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.Module,
                OperatorType = (int)OperatorType.Add,
                Remark = "新增模块:" + model.ModuleName
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

        /// <summary>
        /// 获取模块集合
        /// </summary>
        public List<Module> GetList(ListModuleInput input, out int count)
        {
            const string sql = "select  * ";
            var strSql = new StringBuilder(" from t_Base_Module  where 1 = 1 ");
            var dynamicParameters = new DynamicParameters();
            const string sqlCount = "select count(*) ";
            if (!string.IsNullOrWhiteSpace(input.ModuleName))
            {
                strSql.Append(" and ModuleName like  @ModuleName ");
                dynamicParameters.Add(":ModuleName", '%' + input.ModuleName + '%', DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<Module>(GetPageSql(sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 删除模块集合
        /// </summary>
        public void DeleteAndMenu(string id)
        {
            RedisCacheHelper.Remove("MenuModuleTree");
            if (string.IsNullOrEmpty(id)) return;
            var strSql = new StringBuilder("delete  t_Base_Module  where 1 = 1 ");
            var dynamicParameters = new DynamicParameters();
            strSql.Append(" and Id =  @id ");
            dynamicParameters.Add(":id", id, DbType.String);
            Db.ExecuteNonQuery(strSql.ToString(), dynamicParameters);
        }
    }
}