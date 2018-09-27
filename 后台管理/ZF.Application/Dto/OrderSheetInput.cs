using System;
using System.Collections.Generic;
using ZF.Application.BaseDto;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：OrderSheet 
    /// </summary>
    [AutoMap(typeof(OrderSheet))]
    public class OrderSheetInput
    {
        /// <summary>
        /// 用户编码
        /// </summary>     
        public string RegisterUserId { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>     
        public decimal OrderAmount { get; set; }

        /// <summary>
        /// 实际支付金额
        /// </summary>     
        public decimal FactPayAmount { get; set; }

        /// <summary>
        /// 实际支付时间
        /// </summary>     
        public string PayTime1 { get; set; }

        /// <summary>
        /// 交易编号（接口成功后返回）
        /// </summary>     
        public string TradeNo { get; set; }

        /// <summary>
        /// 机构编号
        /// </summary>
        public string InstitutionsId { get; set; }


        /// <summary>
        /// 备注
        /// </summary>     
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<OrderDetailInputs> Data { get; set; }

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
        /// 修改订单人
        /// </summary>
        public string SetOrderUser { get; set; }
        /// <summary>
        /// 修改订单时间
        /// </summary>
        public string SetOrderTime { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public List<RegiestInput> RegiestInput { get; set; }
    }



    public class RegiestInput
    {
        /// <summary>
        /// 
        /// </summary>
        public string 手机号码 { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class OrderDetailInputs
    {
        /// <summary>
        /// 课程编码
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 订单编码
        /// </summary>     
        public string OrderNo { get; set; }

        /// <summary>
        /// 课程原价
        /// </summary>     
        public decimal? Price { get; set; }

        /// <summary>
        /// 课程优惠价
        /// </summary>     
        public decimal? FavourablePrice { get; set; }

        /// <summary>
        /// 课程数量
        /// </summary>     
        public int Number { get; set; }

        /// <summary>
        /// 合计
        /// </summary>     
        public decimal? Total { get; set; }

        /// <summary>
        /// 课程类型(0:课程 1：套餐)
        /// </summary>     
        public int? CourseType { get; set; }

        /// <summary>
        /// 有效期天数
        /// </summary>
        public DateTime? ValidityEndDate { get; set; }
    }

    /// <summary>
    /// 销售统计入参
    /// </summary>
    public class OrderSaleInput:BasePageInput {
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? BeginDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 课程类型
        /// </summary>
        public int CourseType { get; set; }

        /// <summary>
        /// 专业类别
        /// </summary>
        public string SubjectClassId { get; set;}

    }
}

