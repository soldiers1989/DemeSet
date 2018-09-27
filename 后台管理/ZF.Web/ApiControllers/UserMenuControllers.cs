
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：用户菜单关系表 
    /// </summary>
    public class UserMenuController : BaseController
    {
	   private readonly UserMenuAppService _userMenuAppService;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

	   public UserMenuController(UserMenuAppService  userMenuAppService ,OperatorLogAppService operatorLogAppService)
	   {
			_userMenuAppService =userMenuAppService;
			_operatorLogAppService = operatorLogAppService;
	   }

	   /// <summary>
       /// 查询列表实体：用户菜单关系表 
       /// </summary>
	   [HttpPost]
        public JqGridOutPut<UserMenuOutput> GetList(UserMenuListInput input)
        {
            var count = 0;
            var list = _userMenuAppService.GetList(input, out count);
            return new JqGridOutPut<UserMenuOutput>()
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
                var model = _userMenuAppService.Get(item);
                _userMenuAppService.Delete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
         /// <summary>
        /// 新增或修改实体：用户菜单关系表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(UserMenuInput input)
        {
            var data = _userMenuAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

		 /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
		[System.Web.Http.HttpPost]
        public UserMenu GetOne(IdInput input)
        {
            var model = _userMenuAppService.Get(input.Id);
            return model;
        }
    }
}

