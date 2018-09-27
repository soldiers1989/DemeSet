
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：角色人员关系表 
    /// </summary>
    public class RoleUserController : BaseController
    {
        private readonly RoleUserAppService _roleUserAppService;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        public RoleUserController(RoleUserAppService roleUserAppService, OperatorLogAppService operatorLogAppService)
        {
            _roleUserAppService = roleUserAppService;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询列表实体：角色人员关系表 
        /// </summary>
        [HttpPost]
        public JqGridOutPut<RoleUserOutput> GetList1(RoleUserListInput input)
        {
            var count = 0;
            var list = _roleUserAppService.GetList1(input, out count);
            return new JqGridOutPut<RoleUserOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }

        /// <summary>
        /// 查询列表实体：角色人员关系表 
        /// </summary>
        [HttpPost]
        public JqGridOutPut<RoleUserOutput> GetList(RoleUserListInput input)
        {
            var count = 0;
            var list = _roleUserAppService.GetList(input, out count);
            return new JqGridOutPut<RoleUserOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
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
                var model = _roleUserAppService.Get(item);
                _roleUserAppService.Delete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
        /// <summary>
        /// 新增或修改实体：角色人员关系表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(RoleUserInput input)
        {
            var data = _roleUserAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

        /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public RoleUser GetOne(IdInput input)
        {
            var model = _roleUserAppService.Get(input.Id);
            return model;
        }
    }
}

