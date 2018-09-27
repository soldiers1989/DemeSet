using System;
using ZF.Application.BaseDto;
using ZF.Infrastructure;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：OrderSheet 
    /// </summary>
    public class OrderSheetListInput : BasePageInput
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 订单编码
        /// </summary>     
        public string OrderNo { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>     
        public string RegisterUserName { get; set; }
        /// <summary>
        /// 下单时间起
        /// </summary>     
        public DateTime? AddTimeBegin { get; set; }

        /// <summary>
        /// 下单时间止
        /// </summary>     
        public DateTime? AddTimeEnd { get; set; }

        /// <summary>
        /// 订单状态  0已付款  1代付款  2付款失败  3已取消  4已退款  5作废
        /// </summary>     
        public int? State { get; set; }

        /// <summary>
        /// 支付方式    微信  支付宝  线下
        /// </summary>     
        public string PayType { get; set; }

        /// <summary>
        /// 下单终端(方式)  0 客户端  1 app 2  微信  3 人工
        /// </summary>     
        public int? OrderType { get; set; }

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
        public string CourseId { get; set; }

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
        /// 机构编号
        /// </summary>
        public  string InstitutionsId { get; set; }

    }
}
