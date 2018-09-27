
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：RegisterUser 
    /// </summary>
    public class RegisterUserController : BaseController
    {
	   private readonly RegisterUserAppService _registerUserAppService;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

	   public RegisterUserController(RegisterUserAppService  registerUserAppService ,OperatorLogAppService operatorLogAppService)
	   {
			_registerUserAppService =registerUserAppService;
			_operatorLogAppService = operatorLogAppService;
	   }

	   /// <summary>
       /// 查询列表实体：RegisterUser 
       /// </summary>
	   [HttpPost]
        public JqGridOutPut<RegisterUserOutput> GetList(RegisterUserListInput input)
        {
            var count = 0;
            var list = _registerUserAppService.GetList(input, out count);
            return new JqGridOutPut<RegisterUserOutput>()
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
                var model = _registerUserAppService.Get(item);
                if (model != null)
				{
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId =  (int)Model.RegisterUser,
                        OperatorType = (int) OperatorType.Delete,
                        Remark = "删除RegisterUser:"
                    });
				}
                _registerUserAppService.Delete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
         /// <summary>
        /// 新增或修改实体：RegisterUser
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(RegisterUserInput input)
        {
            var data = _registerUserAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

		 /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
		[System.Web.Http.HttpPost]
        public RegisterUser GetOne(IdInput input)
        {
            var model = _registerUserAppService.Get(input.Id);
            return model;
        }
    }
}

