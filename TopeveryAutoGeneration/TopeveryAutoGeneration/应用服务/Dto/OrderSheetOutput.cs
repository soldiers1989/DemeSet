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
      public string Id{ get; set; }
       /// <summary>
       /// 订单编码
       /// </summary>     
      public string OrderNo{ get; set; }
       /// <summary>
       /// 用户编码
       /// </summary>     
      public string RegisterUserId{ get; set; }
       /// <summary>
       /// 下单时间
       /// </summary>     
      public DateTime AddTime{ get; set; }
       /// <summary>
       /// 订单金额
       /// </summary>     
      public decimal OrderAmount{ get; set; }
       /// <summary>
       /// 订单状态
       /// </summary>     
      public int State{ get; set; }
       /// <summary>
       /// 实际支付金额
       /// </summary>     
      public decimal FactPayAmount{ get; set; }
       /// <summary>
       /// 下单IP
       /// </summary>     
      public string OrderIp{ get; set; }
       /// <summary>
       /// 是否删除
       /// </summary>     
      public int IsDelete{ get; set; }
       /// <summary>
       /// 支付方式
       /// </summary>     
      public string PayType{ get; set; }
       /// <summary>
       /// 实际支付时间
       /// </summary>     
      public DateTime? PayTime{ get; set; }
       /// <summary>
       /// 下单终端(方式)
       /// </summary>     
      public string OrderType{ get; set; }
       /// <summary>
       /// 交易编号（接口成功后返回）
       /// </summary>     
      public string TradeNo{ get; set; }
    }
}

