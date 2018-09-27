using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
using ZF.Infrastructure.zTree;

namespace ZF.Application.AppService
{
    /// <summary>
    /// 
    /// </summary>
    public class MenuAppService : BaseAppService<Menu>
    {
        /// <summary>
        /// 文件关系表仓储服务
        /// </summary>
        private readonly IMenuRepository _repository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="operatorLogAppService"></param>
        public MenuAppService(IMenuRepository repository, OperatorLogAppService operatorLogAppService) : base(repository)
        {
            _repository = repository;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 新增实体：Operatorlog 
        /// </summary>
        public MessagesOutPut AddOrEdit(MenuInput input)
        {
            Menu model;
            RedisCacheHelper.Remove("MenuModuleTree");
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _repository.Get(input.Id);
                model.Class = input.Class;
                model.Description = input.Description;
                model.MenuName = input.MenuName;
                model.ModuleId = input.ModuleId;
                model.Url = input.Url;
                model.Sort = input.Sort;
                _repository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.Menu,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改菜单:" + model.MenuName
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<Menu>();
            model.Id = Guid.NewGuid().ToString();
            var keyId = _repository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.Menu,
                OperatorType = (int)OperatorType.Add,
                Remark = "新增菜单:" + model.MenuName
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        public List<MenuOutput> GetList(ListMenuInput input, out int count)
        {
            const string sql = "select  * ";
            var strSql = new StringBuilder(" from t_Base_Menu  where 1 = 1 ");
            var dynamicParameters = new DynamicParameters();
            const string sqlCount = "select count(*) ";
            if (!string.IsNullOrEmpty(input.ModuleId))
            {
                strSql.Append(" and ModuleId = @ModuleId ");
                dynamicParameters.Add(":ModuleId", input.ModuleId, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.MenuName))
            {
                strSql.Append(" and MenuName like  @MenuName ");
                dynamicParameters.Add(":MenuName", '%' + input.MenuName + '%', DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<MenuOutput>(GetPageSql(sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord), dynamicParameters);

            return list;
        }


        /// <summary>
        /// 获取菜单列表
        /// </summary>
        public List<MenuModule> GetMenuModuleList()
        {
            string strSqlModule = "";
            string strSqlMenu = "";
            if (UserObject.IsAdmin == 0)
            {
                strSqlModule = " select * from  t_Base_Module order by Sort asc   ";
                strSqlMenu = " select * from t_Base_Menu order by Sort asc  ";
            }
            else
            {
                strSqlModule = $@" SELECT  *
FROM    t_Base_Module
WHERE   Id IN ( SELECT  b.MenuId
                FROM    t_base_RoleUser c
                        LEFT JOIN t_base_Role a ON c.RoleId = a.Id
                        LEFT JOIN t_base_RoleMenu b ON a.Id = b.RoleId
                WHERE   b.Type = 0
                        AND c.UserId = '{UserObject.Id}' )
ORDER BY Sort ASC  ";
                strSqlMenu = $@" SELECT *
 FROM   t_Base_Menu
 WHERE  Id IN ( SELECT  b.MenuId
                FROM    t_base_RoleUser c
                        LEFT JOIN t_base_Role a ON c.RoleId = a.Id
                        LEFT JOIN t_base_RoleMenu b ON a.Id = b.RoleId
                WHERE   b.Type = 1
                        AND c.UserId = '{UserObject.Id}' )
 ORDER BY Sort ASC ";
            }
            var moduleList = Db.QueryList<Module>(strSqlModule);
            var menuList = Db.QueryList<Menu>(strSqlMenu);
            var row = new List<MenuModule>();
            foreach (var item in moduleList)
            {
                if (menuList == null) continue;
                var child = (from item1 in menuList where item.Id == item1.ModuleId select new Child { Class = item1.Class, Name = item1.MenuName, Url = item1.Url }).ToList();
                var menuModule = new MenuModule { Class = item.Class, Name = item.ModuleName, Child = child };
                row.Add(menuModule);
            }
            return row;
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        public List<Tree.zTree> GetMenuModuleLists()
        {
            const string strSqlModule = @" SELECT *
 FROM   ( SELECT    a.Id ,
                    a.ModuleName AS Name ,
                    '' AS pId ,
                    a.Sort
          FROM      t_Base_Module a
          UNION
          SELECT    b.Id ,
                    b.MenuName AS Name ,
                    b.ModuleId AS pId ,
                    b.Sort
          FROM      t_Base_Menu b
        ) c
 ORDER BY c.Sort ASC ";
            var menuList = Db.QueryList<Tree.zTree>(strSqlModule);
            var row = new List<Tree.zTree>();
            foreach (var item in menuList)
            {
                if (String.IsNullOrEmpty(item.pId))
                {
                    row.Add(item);
                    Tree.Ztree(item, menuList);
                }
            }
            return row;
        }


        /// <summary>
        /// 菜单module
        /// </summary>
        public class MenuModule
        {
            public string Class { get; set; }

            public string Name { get; set; }

            public List<Child> Child { get; set; }
        }

        /// <summary>
        /// 子菜单
        /// </summary>
        public class Child
        {
            public string Url { get; set; }

            public string Name { get; set; }

            public string Class { get; set; }
        }
    }
}