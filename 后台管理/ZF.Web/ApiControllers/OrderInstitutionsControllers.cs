
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：下单机构表 
    /// </summary>
    public class OrderInstitutionsController : BaseController
    {
        private readonly OrderInstitutionsAppService _orderInstitutionsAppService;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        public OrderInstitutionsController(OrderInstitutionsAppService orderInstitutionsAppService, OperatorLogAppService operatorLogAppService)
        {
            _orderInstitutionsAppService = orderInstitutionsAppService;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询列表实体：下单机构表 
        /// </summary>
        [HttpPost]
        public JqGridOutPut<OrderInstitutionsOutput> GetList(OrderInstitutionsListInput input)
        {
            var count = 0;
            var list = _orderInstitutionsAppService.GetList(input, out count);
            return new JqGridOutPut<OrderInstitutionsOutput>()
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
                var model = _orderInstitutionsAppService.Get(item);
                if (model != null)
                {
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = (int)Model.OrderInstitutions,
                        OperatorType = (int)OperatorType.Delete,
                        Remark = "删除下单机构表:" + model.Name
                    });
                }
                _orderInstitutionsAppService.Delete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
        /// <summary>
        /// 新增或修改实体：下单机构表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(OrderInstitutionsInput input)
        {
            var data = _orderInstitutionsAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

        /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public OrderInstitutions GetOne(IdInput input)
        {
            var model = _orderInstitutionsAppService.Get(input.Id);
            return model;
        }
    }
}

