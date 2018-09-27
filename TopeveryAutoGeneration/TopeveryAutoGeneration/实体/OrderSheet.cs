using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：订单表 
    /// </summary>
    public class OrderSheet:BaseEntity<Guid>
    {
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
       public int OrderType{ get; set; }

       /// <summary>
       /// 交易编号（接口成功后返回）
       /// </summary>     
       public string TradeNo{ get; set; }

       /// <summary>
       /// 订单备注
       /// </summary>     
       public string Remark{ get; set; }

    }
}

