using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：购物车表 
    /// </summary>
    public class OrderCart:BaseEntity<Guid>
    {
       /// <summary>
       /// 订单号
       /// </summary>     
       public string OrderNo{ get; set; }

       /// <summary>
       /// 用户编码
       /// </summary>     
       public string RegisterUserId{ get; set; }

    }
}

