
using System.Collections.Generic;
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Application.Dto.SalesReports;
using ZF.Application.WebApiDto.SheetDtoModule;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：订单表 
    /// </summary>
    public class OrderSheetController : BaseController
    {
        private readonly OrderSheetAppService _orderSheetAppService;

        private readonly OrderDetailAppService _orderDetailAppService;

        private readonly RegisterUserAppService _registerUserAppService;
        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        public OrderSheetController(OrderSheetAppService orderSheetAppService, OperatorLogAppService operatorLogAppService, OrderDetailAppService orderDetailAppService, RegisterUserAppService registerUserAppService)
        {
            _orderSheetAppService = orderSheetAppService;
            _operatorLogAppService = operatorLogAppService;
            _orderDetailAppService = orderDetailAppService;
            _registerUserAppService = registerUserAppService;
        }

        /// <summary>
        /// 查询列表实体：订单表 
        /// </summary>
        [HttpPost]
        public JqGridOutPut<OrderSheetOutput> GetList(OrderSheetListInput input)
        {
            var count = 0;
            var list = _orderSheetAppService.GetList(input, out count);
            return new JqGridOutPut<OrderSheetOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }



        /// <summary>
        /// 按日获取统计报表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<SalesReportsOutPut> GetSalesReportsDayList(SalesReportsInput input)
        {
            var count = 0;
            var list = _orderSheetAppService.GetSalesReportsDayList(input);
            return new JqGridOutPut<SalesReportsOutPut>()
            {
                Page = 1,
                Total = 1,
                Records = count,
                Rows = list
            };
        }

        /// <summary>
        /// 按月获取统计报表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<SalesReportsOutPut> GetSalesReportsMonthList(SalesReportsInput input)
        {
            var count = 0;
            var list = _orderSheetAppService.GetSalesReportsMonthList(input);
            return new JqGridOutPut<SalesReportsOutPut>()
            {
                Page = 1,
                Total = 1,
                Records = count,
                Rows = list
            };
        }

        /// <summary>
        /// 按年获取统计报表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<SalesReportsOutPut> GetSalesReportsYearList(SalesReportsInput input)
        {
            var count = 0;
            var list = _orderSheetAppService.GetSalesReportsYearList(input);
            return new JqGridOutPut<SalesReportsOutPut>()
            {
                Page = 1,
                Total = 1,
                Records = count,
                Rows = list
            };
        }

        /// <summary>
        /// 通过订单编号获取订单课程明细
        /// </summary>
        [HttpPost]
        public JqGridOutPut<OrderDetailOutput> GetOrderCourseList(IdInput input)
        {
            var list = _orderDetailAppService.GetOrderCourseList(input);
            return new JqGridOutPut<OrderDetailOutput>()
            {
                Page = 1,
                Total = 1,
                Records = list.Count,
                Rows = list
            };
        }


        /// <summary>
        /// 根据id 删除实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut Revocation(IdInputIds input)
        {
            var array = input.Ids.TrimEnd(',').Split(',');
            foreach (var item in array)
            {
                var model = _orderSheetAppService.Get(item);
                if (model != null)
                {
                    model.State = (int)OrderState.Invalid;
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = (int)Model.OrderSheet,
                        OperatorType = (int)OperatorType.Edit,
                        Remark = "更新订单号:" + model.OrderNo + "为撤销状态"
                    });
                    _orderSheetAppService.UpdateModel(model);
                }
            }
            return new MessagesOutPut { Id = 1, Message = "撤销成功!", Success = true };
        }



        /// <summary>
        /// 新增或修改实体：订单表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(OrderSheetInput input)
        {
            if (input.RegiestInput.Count > 0)
            {
                foreach (var item in input.RegiestInput)
                {
                    var userId = _registerUserAppService.AddRegiest(item.手机号码);
                    input.RegisterUserId = userId;
                    _orderSheetAppService.AddOrEdit(input);
                }
                return new MessagesOutPut { Id = -1, Message = "新增成功!", Success = true };
            }
            else
            {
                var data = _orderSheetAppService.AddOrEdit(input);
                return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
            }
        }

        /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public OrderSheet GetOne(IdInput input)
        {
            var model = _orderSheetAppService.Get(input.Id);
            return model;
        }

        /// <summary>
        /// 修改订单快递公司
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut UpdateSheetExpressCompany(OrderSheetListInput input)
        {
            return _orderSheetAppService.UpdateSheetExpressCompany(input);
        }

        /// <summary>
        /// 查询已支付订单中的电子发票信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<OrderSheetOutput> GetElectronicInvoiceInfoByOrderSheet(OrderSheetListInput input)
        {
            return _orderSheetAppService.GetElectronicInvoiceInfoByOrderSheet(input);
        }

        /// <summary>
        /// 处理需要电子发票的订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut EnditiElectronicInvoice(IdInput input)
        {
            return _orderSheetAppService.EnditiElectronicInvoice(input);
        }


        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public SheetModelPageOutput OrderSheetDetail(SheetModelInput input)
        {
            return _orderSheetAppService.OrderSheetDetail(input);
        }

        /// <summary>
        /// 修改订单价格
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut SetOrderPrice(OrderSheet input)
        {
            return _orderSheetAppService.SetOrderPrice(input);
        }

        /// <summary>
        /// 统计课程销量
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<CourseSaleReport> GetSaleReportByCourse( OrderSaleInput input )
        {
            var count = 0;
            var list= _orderSheetAppService.GetSaleReportByCourse( input );
            return new JqGridOutPut<CourseSaleReport>( )
            {
                Page = 1,
                Total = 1,
                Records = count,
                Rows = list
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<CourseSaleReport> GetSaleReportByCourseWithPage( OrderSaleInput input ) {
            var count = 0;
            var list = _orderSheetAppService.GetSaleReportByCourseWithPage( input,out count );
            return new JqGridOutPut<CourseSaleReport>( )
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records =count,
                Rows = list
            };
        }
    }
}

