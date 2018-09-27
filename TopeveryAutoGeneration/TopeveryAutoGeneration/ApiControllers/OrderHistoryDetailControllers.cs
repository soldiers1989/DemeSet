
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：订单明细表(历史表) 
    /// </summary>
    public class OrderHistoryDetailController : BaseController
    {
	   private readonly OrderHistoryDetailAppService _orderHistoryDetailAppService;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

	   public OrderHistoryDetailController(OrderHistoryDetailAppService  orderHistoryDetailAppService ,OperatorLogAppService operatorLogAppService)
	   {
			_orderHistoryDetailAppService =orderHistoryDetailAppService;
			_operatorLogAppService = operatorLogAppService;
	   }

	   /// <summary>
       /// 查询列表实体：订单明细表(历史表) 
       /// </summary>
	   [HttpPost]
        public JqGridOutPut<OrderHistoryDetailOutput> GetList(OrderHistoryDetailListInput input)
        {
            var count = 0;
            var list = _orderHistoryDetailAppService.GetList(input, out count);
            return new JqGridOutPut<OrderHistoryDetailOutput>()
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
                var model = _orderHistoryDetailAppService.Get(item);
                if (model != null)
				{
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId =  (int)Model.OrderHistoryDetail,
                        OperatorType = (int) OperatorType.Delete,
                        Remark = "删除订单明细表(历史表):"
                    });
				}
                _orderHistoryDetailAppService.Delete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
         /// <summary>
        /// 新增或修改实体：订单明细表(历史表)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(OrderHistoryDetailInput input)
        {
            var data = _orderHistoryDetailAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

		 /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
		[System.Web.Http.HttpPost]
        public OrderHistoryDetail GetOne(IdInput input)
        {
            var model = _orderHistoryDetailAppService.Get(input.Id);
            return model;
        }
    }
}

