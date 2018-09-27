using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZF.Application.BaseDto;
using ZF.Application.WebApiAppService.CartModule;
using ZF.Application.WebApiAppService.SheetModule;
using ZF.Application.WebApiDto.SheetDtoModule;
using ZF.Core.Entity;

namespace ZF.API.Controllers
{
    /// <summary>
    /// 订单api
    /// </summary>
    public class SheetController : BaseApiController
    {
        private readonly SheetApiService sheetApiService;

        private readonly CartModuleAppService cartModuleAppService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_sheetApiService"></param>
        public SheetController(SheetApiService _sheetApiService, CartModuleAppService _cartModuleAppService)
        {
            sheetApiService = _sheetApiService;
            cartModuleAppService = _cartModuleAppService;
        }

        /// <summary>
        /// 订单写入后返回结果集
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public List<SheetModelOutput> SheetAddToList(SheetModelInput input)
        {
            input.RegisterUserId = UserObject.Id;
            input.InstitutionsId = UserObject.InstitutionsId;
            return sheetApiService.SheetAddToList(input);
        }

        /// <summary>
        /// 订单写入后返回结果集
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public List<SheetModelOutput> SheetAddToListByWiki(SheetModelInput input)
        {
            input.RegisterUserId = UserObject.Id;
            input.InstitutionsId = UserObject.InstitutionsId;
            return sheetApiService.SheetAddToListByWiki(input);
        }

        /// <summary>
        /// 订单写入后返回结果集
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cartId"></param>
        /// <returns></returns>
        public List<SheetModelOutput> SheetAddToListNoLogin(SheetModelInput input)
        {
            input.RegisterUserId = UserObject.Id;
              input.InstitutionsId = UserObject.InstitutionsId;
            return sheetApiService.SheetAddToListNoLogin(input);
        }
        /// <summary>
        /// 卡卷使用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public MessagesOutPut EnditDiscountCard(SheetModelInput input)
        {
            return sheetApiService.EnditDiscountCard(input);
        }

        /// <summary>
        /// 我的订单
        /// </summary>
        /// <param name="input"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<SheetModelPageOutput> PageSheetList(SheetModelInput input)
        {
            var count = 0;
            input.RegisterUserId = UserObject.Id;
            var list = sheetApiService.PageSheetList(input, out count);
            return new JqGridOutPut<SheetModelPageOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }

        /// <summary>
        /// 订单-wechat
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public List<SheetModelPageOutput> WcChatSheetList(SheetModelInput input)
        {
            input.RegisterUserId = UserObject.Id;
            return sheetApiService.WcChatSheetList(input);
        }

        /// <summary>
        /// 写入电子发票信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut EnditiSheetElectronicInvoice(SheetModelInput input)
        {
            return sheetApiService.EnditiSheetElectronicInvoice(input);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>



        /// <summary>
        /// 平台用户撤销订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut EnditSheetState(SheetModelInput input)
        {
            return sheetApiService.EnditSheetState(input);
        }




        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut DelSheet(IdInput input)
        {
            return sheetApiService.DelSheet(input);
        }
        /// <summary>
        /// 用户重新下单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut ReOrder(SheetModelInput input)
        {
            return sheetApiService.ReOrder(input);
        }

        /// <summary>
        /// 判断是否已购买指定课程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public bool IfAreadyPay(SheetDetailModelInput input)
        {
            input.UserId = UserObject.Id;
            return sheetApiService.IfAreadyPay(input);
        }
        /// <summary>
        /// 修改讲义收货地址
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut EnditiSheetDeliveryAddRess(SheetModelInput input)
        {
            return sheetApiService.EnditiSheetDeliveryAddRess(input);
        }

        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public SheetModelPageOutput OrderSheetDetail(SheetModelInput input)
        {
            return sheetApiService.OrderSheetDetail(input);
        }

        /// <summary>
        /// 获得指定课程信息以及购物车数量
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public CourseInfo GetCourseInfoByOneAndUserCartCount(IdInput input)
        {
            return cartModuleAppService.GetCourseInfoByOneAndUserCartCount(input.Id, UserObject.Id);
        }

        /// <summary>
        /// 获得指定课程信息以及购物车数量
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public CoursePackcourseInfo GetPackInfoByOneAndUserCartCount(IdInput input)
        {
            return cartModuleAppService.GetPackInfoByOneAndUserCartCount(input.Id, UserObject.Id);
        }

        /// <summary>
        /// 查询订单是否已经被支付或支付失败
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public MessagesOutPut OrderSheetIsPay(SheetModelInput input)
        {
            input.RegisterUserId = UserObject.Id;
            return sheetApiService.OrderSheetIsPay(input);
        }
    }
}