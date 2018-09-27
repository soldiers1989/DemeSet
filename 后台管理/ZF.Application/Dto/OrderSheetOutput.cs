using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 数据表OrderSheet 输出Dto
    /// </summary>
    public class OrderSheetOutput
    {
        /// <summary>
        /// 编码
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 订单编码
        /// </summary>     
        public string OrderNo { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>     
        public string RegisterUserName { get; set; }
        /// <summary>
        /// 下单时间
        /// </summary>     
        public DateTime AddTime { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>     
        public decimal OrderAmount { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>     
        public string State { get; set; }
        /// <summary>
        /// 实际支付金额
        /// </summary>     
        public decimal FactPayAmount { get; set; }
        /// <summary>
        /// 下单IP
        /// </summary>     
        public string OrderIp { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>     
        public string PayTypeName { get; set; }

        /// <summary>
        /// 实际支付时间
        /// </summary>     
        public DateTime? PayTime { get; set; }

        /// <summary>
        /// 下单终端(方式)
        /// </summary>     
        public string OrderType { get; set; }

        /// <summary>
        /// 交易编号（接口成功后返回）
        /// </summary>     
        public string TradeNo { get; set; }
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
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

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
        /// 电子发票处理状态
        /// </summary>
        public int InvoiceState { get; set; }
        /// <summary>
        /// 发票手机号
        /// </summary>
        public string InvoicePhone { get; set; }

        /// <summary>
        /// 机构名称
        /// </summary>
        public string InstitutionsName { get; set; }

        /// <summary>
        /// 课程金额
        /// </summary>
        public decimal FavourablePrice { get; set; }

    }
}

