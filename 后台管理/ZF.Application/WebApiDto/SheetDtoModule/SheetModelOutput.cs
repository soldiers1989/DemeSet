using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;

namespace ZF.Application.WebApiDto.SheetDtoModule
{
    /// <summary>
    /// 订单出参
    /// </summary>
    public class SheetModelOutput
    {

        /// <summary>
        /// 明细ID
        /// </summary>
        public string Id { get; set; }


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
        /// 明细订单编码
        /// </summary>     
        public string DetailOrderNo { get; set; }

        /// <summary>
        /// 明细课程编码
        /// </summary>     
        public string CourseId { get; set; }

        /// <summary>
        /// 明细课程原价
        /// </summary>     
        public decimal? Price { get; set; }

        /// <summary>
        /// 明细课程优惠价
        /// </summary>     
        public decimal? FavourablePrice { get; set; }

        /// <summary>
        /// 明细课程数量
        /// </summary>     
        public int? Num { get; set; }

        /// <summary>
        /// 明细合计
        /// </summary>     
        public decimal? Amount { get; set; }

        /// <summary>
        /// 明细课程类型(0:课程 1：套餐)
        /// </summary>     
        public int? CourseType { get; set; }

        /// <summary>
        /// 明细课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 有效天数
        /// </summary>
        public int ValidityPeriod { get; set; }

        /// <summary>
        /// 有效日期
        /// </summary>
        public DateTime? ValidityEndDate { get; set; }

        /// <summary>
        /// 商品单价
        /// </summary>
        public double CoursePrice { get; set; }

        /// <summary>
        /// 商品总金额
        /// </summary>
        public double CourseAmout { get; set; }

        /// <summary>
        /// 是否邮寄讲义
        /// </summary>
        public int EmailNotes { get; set; }

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
        /// 快递公司名称
        /// </summary>
        public string ExpressName { get; set; }
        /// <summary>
        /// 发票抬头
        /// </summary>
        public string InvoiceHeader { get; set; }

        /// <summary>
        /// 发票邮箱
        /// </summary>
        public string InvoiceMailbox { get; set; }
        /// <summary>
        /// 课程图片
        /// </summary>
        public string CourseIamge { get; set; }
        /// <summary>
        /// 纳税人标识号
        /// </summary>
        public string TaxpayerIdentificationNumber { get; set; }
        /// <summary>
        /// 发票人手机号
        /// </summary>
        public string InvoicePhone { get; set; }


    }

    /// <summary>
    /// 订单
    /// </summary>

    public class SheetModelPageOutput
    {

        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime PayTime { get; set; }

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
        public string AddTime { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>     
        public decimal OrderAmount { get; set; }

        /// <summary>
        /// 课程金额
        /// </summary>
        public decimal? FavourablePrice { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>     
        public int State { get; set; }

        /// <summary>
        /// 实际支付金额
        /// </summary>     
        public decimal FactPayAmount { get; set; }

        /// <summary>
        /// 状态值
        /// </summary>
        public string StateVal { get; set; }

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
        /// 支付方式
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 讲义收件人
        /// </summary>
        public string Contact { get; set; }
        /// <summary>
        /// 讲义邮寄邮编
        /// </summary>
        public string Zip { get; set; }
        /// <summary>
        /// 讲义邮寄详细地址
        /// </summary>
        public string DetailedAddress { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 居住城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 区
        /// </summary>
        public string Town { get; set; }
        /// <summary>
        /// 订单优惠金额
        /// </summary>
        public double CardAmount { get; set; }
        /// <summary>
        /// 讲义收件人电话
        /// </summary>
        public string ContactPhone { get; set; }
        /// <summary>
        /// 应付金额
        /// </summary>
        public double WithPrice { get; set; }
        /// <summary>
        /// 电子发票处理状态
        /// </summary>
        public int InvoiceState { get; set; }
        /// <summary>
        /// 明细
        /// </summary>
        public List<SheetModelPageDetailOutput> sheetmodelpagedetailoutput { get; set; }
        /// <summary>
        /// 讲义ID
        /// </summary>
        public string HandOutId { get; set; }

        /// <summary>
        /// 商品总额
        /// </summary>
        public double CommodityPrice { get; set; }

        /// <summary>
        /// 发票人手机号
        /// </summary>
        public string InvoicePhone { get; set; }

        /// <summary>
        /// 订单完成超过一月不允许开发票
        /// </summary>
        public int InvoiceHeaderTime { get; set; }

    }

    /// <summary>
    /// 明细
    /// </summary>
    public class SheetModelPageDetailOutput
    {

        /// <summary>
        /// 明细ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 明细订单编码
        /// </summary>     
        public string DetailOrderNo { get; set; }

        /// <summary>
        /// 明细课程编码
        /// </summary>     
        public string CourseId { get; set; }

        /// <summary>
        /// 明细课程原价
        /// </summary>     
        public decimal? Price { get; set; }

        /// <summary>
        /// 明细课程优惠价
        /// </summary>     
        public decimal? FavourablePrice { get; set; }

        /// <summary>
        /// 明细课程数量
        /// </summary>     
        public int? Num { get; set; }

        /// <summary>
        /// 明细合计
        /// </summary>     
        public decimal? Amount { get; set; }

        /// <summary>
        /// 明细课程类型(0:课程 1：套餐)
        /// </summary>     
        public int? CourseType { get; set; }

        /// <summary>
        /// 明细课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 有效天数
        /// </summary>
        public int ValidityPeriod { get; set; }
        /// <summary>
        /// 课程图片
        /// </summary>
        public string CourseIamge { get; set; }

        /// <summary>
        /// 有效日期
        /// </summary>
        public DateTime? ValidityEndDate { get; set; }
    }

}
