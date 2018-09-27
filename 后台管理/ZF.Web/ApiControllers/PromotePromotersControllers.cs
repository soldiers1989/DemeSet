
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：推广员表 
    /// </summary>
    public class PromotePromotersController : BaseController
    {
	   private readonly PromotePromotersAppService _promotePromotersAppService;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

	   public PromotePromotersController(PromotePromotersAppService  promotePromotersAppService ,OperatorLogAppService operatorLogAppService)
	   {
			_promotePromotersAppService =promotePromotersAppService;
			_operatorLogAppService = operatorLogAppService;
	   }

	   /// <summary>
       /// 查询列表实体：推广员表 
       /// </summary>
	   [HttpPost]
        public JqGridOutPut<PromotePromotersOutput> GetList(PromotePromotersListInput input)
        {
            var count = 0;
            var list = _promotePromotersAppService.GetList(input, out count);
            return new JqGridOutPut<PromotePromotersOutput>()
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
                var model = _promotePromotersAppService.Get(item);
                if (model != null)
				{
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId =  (int)Model.PromotePromoters,
                        OperatorType = (int) OperatorType.Delete,
                        Remark = "删除推广员表:"+model.Name
                    });
				}
                _promotePromotersAppService.Delete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
         /// <summary>
        /// 新增或修改实体：推广员表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(PromotePromotersInput input)
        {
            var data = _promotePromotersAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

		 /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
		[System.Web.Http.HttpPost]
        public PromotePromoters GetOne(IdInput input)
        {
            var model = _promotePromotersAppService.Get(input.Id);
            return model;
        }
    }
}

