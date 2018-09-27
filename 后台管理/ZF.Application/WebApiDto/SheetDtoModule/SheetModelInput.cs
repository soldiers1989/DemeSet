using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;

namespace ZF.Application.WebApiDto.SheetDtoModule
{
    /// <summary>
    /// 入参
    /// </summary>
    public class SheetModelInput : BasePageInput
    {

        /// <summary>
        /// 明细id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal OrderAmount { get; set; }

        /// <summary>
        /// 下单ip
        /// </summary>
        public string OrderIp { get; set; }

        /// <summary>
        /// 下单终端
        /// </summary>
        public int OrderType { get; set; }

        /// <summary>
        /// 订单编码
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string RegisterUserId { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 提交订单明细
        /// </summary>
        public string CarDetailIdList { get; set; }
        /// <summary>
        /// 协议收货ID
        /// </summary>     
        public string HandOutId { get; set; }

        /// <summary>
        /// 快递公司Id
        /// </summary>     
        public string ExpressCompanyId { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>     
        public string ExpressNumber { get; set; }

        /// <summary>
        /// 学习卡
        /// </summary>
        public string DiscountCard { get; set; }
        /// <summary>
        /// 当前用户
        /// </summary>
        public string CurrentUser { get; set; }

        /// <summary>
        /// 发票抬头
        /// </summary>
        public string InvoiceHeader { get; set; }

        /// <summary>
        /// 发票邮箱
        /// </summary>
        public string InvoiceMailbox { get; set; }

        /// <summary>
        /// 纳税人标识号
        /// </summary>
        public string TaxpayerIdentificationNumber { get; set; }
        /// <summary>
        /// 发票人手机号
        /// </summary>
        public string InvoicePhone { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayType { get; set; }

        /// <summary>
        /// 推广码
        /// </summary>
        public string PromotionCode { get; set; }

        /// <summary>
        /// 优惠卡号
        /// </summary>
        public  string CardNo { get; set; }

        /// <summary>
        /// 机构编号
        /// </summary>
        public  string InstitutionsId { get; set; }
    }
}
