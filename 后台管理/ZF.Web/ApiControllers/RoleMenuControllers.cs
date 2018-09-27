
using System.Collections.Generic;
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：角色菜单关系表 
    /// </summary>
    public class RoleMenuController : BaseController
    {
        private readonly RoleMenuAppService _roleMenuAppService;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        public RoleMenuController(RoleMenuAppService roleMenuAppService, OperatorLogAppService operatorLogAppService)
        {
            _roleMenuAppService = roleMenuAppService;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询列表实体：角色菜单关系表 
        /// </summary>
        [HttpPost]
        public List<RoleMenuOutput> GetList(RoleMenuListInput input)
        {
            var list = _roleMenuAppService.GetList(input);
            return list;
        }


        /// <summary>
        /// 根据id 删除实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut Delete(IdInputIds input)
        {
            var array = input.Ids.TrimEnd(',').Split(',');
            foreach (var item in array)
            {
                var model = _roleMenuAppService.Get(item);
                _roleMenuAppService.Delete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
        /// <summary>
        /// 新增或修改实体：角色菜单关系表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(RoleMenuInput input)
        {
            var data = _roleMenuAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

        /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public RoleMenu GetOne(IdInput input)
        {
            var model = _roleMenuAppService.Get(input.Id);
            return model;
        }
    }
}

