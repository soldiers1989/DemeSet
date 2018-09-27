
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
    /// 数据表实体Api接口：快递公司 
    /// </summary>
    public class ExpressCompanyController : BaseController
    {
        private readonly ExpressCompanyAppService _expressCompanyAppService;

        //邮寄讲义
        private readonly DeliveryAddressAppService _deliveryAddressAppService;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        public ExpressCompanyController(ExpressCompanyAppService expressCompanyAppService, OperatorLogAppService operatorLogAppService, DeliveryAddressAppService deliveryAddressAppService)
        {
            _expressCompanyAppService = expressCompanyAppService;
            _operatorLogAppService = operatorLogAppService;
            _deliveryAddressAppService = deliveryAddressAppService;
        }

        /// <summary>
        /// 查询列表实体：快递公司 
        /// </summary>
        [HttpPost]
        public JqGridOutPut<ExpressCompanyOutput> GetList(ExpressCompanyListInput input)
        {
            var count = 0;
            var list = _expressCompanyAppService.GetList(input, out count);
            return new JqGridOutPut<ExpressCompanyOutput>()
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
                var model = _expressCompanyAppService.Get(item);
                if (model != null)
                {
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = (int)Model.ExpressCompany,
                        OperatorType = (int)OperatorType.Delete,
                        Remark = "删除快递公司:"
                    });
                }
                _expressCompanyAppService.LogicDelete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
        /// <summary>
        /// 新增或修改实体：快递公司
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(ExpressCompanyInput input)
        {
            var data = _expressCompanyAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

        /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public ExpressCompany GetOne(IdInput input)
        {
            var model = _expressCompanyAppService.Get(input.Id);
            return model;
        }

        /// <summary>
        /// 查看需要邮寄的讲义信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public List<DeliveryAddressOutput> GetDeliveryAddRessList(DeliveryAddressListInput input)
        {
            var count = 0;
            var list = _deliveryAddressAppService.GetList(input);
            //return new JqGridOutPut<DeliveryAddressOutput>()
            //{
            //    Page = input.Page,
            //    Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
            //    Records = count,
            //    Rows = list
            //};
            return list;
        }



        /// <summary>
        /// 查询单个信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public ExpressCompany GetInfo(IdInput input)
        {
            return _expressCompanyAppService.GetOne(input);
        }


        /// <summary>
        /// 修改快递公司是否是默认
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut UpdateDefault(IdInput input)
        {
            return _expressCompanyAppService.UpdateDefault(input);
        }
    }
}

