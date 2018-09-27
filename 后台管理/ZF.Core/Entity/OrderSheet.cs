using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：订单表 
    /// </summary>
    public class OrderSheet : BaseEntity<Guid>
    {
        /// <summary>
        /// 订单编码
        /// </summary>     
        public string OrderNo { get; set; }

        /// <summary>
        /// 用户编码
        /// </summary>     
        public string RegisterUserId { get; set; }

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
        public int State { get; set; }

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
        public string PayType { get; set; }

        /// <summary>
        /// 实际支付时间
        /// </summary>     
        public DateTime? PayTime { get; set; }

        /// <summary>
        /// 下单终端(方式)
        /// </summary>     
        public int OrderType { get; set; }

        /// <summary>
        /// 交易编号（接口成功后返回）
        /// </summary>     
        public string TradeNo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>     
        public string Remark { get; set; }

        /// <summary>
        /// 机构编号
        /// </summary>
        public string InstitutionsId { get; set; }

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
        /// 发票人手机号
        /// </summary>
        public string InvoicePhone { get; set; }

        /// <summary>
        /// 发票地址写入时间
        /// </summary>
        public DateTime? InvoiceTime { get; set; }
        /// <summary>
        /// 订单价格修改人
        /// </summary>
        public string SetOrderUser { get; set; }
        /// <summary>
        /// 订单价格修改时间
        /// </summary>
        public string SetOrderTime { get; set; }

        /// <summary>
        /// 推广码
        /// </summary>
        public string PromotionCode { get; set; }

    }
}

