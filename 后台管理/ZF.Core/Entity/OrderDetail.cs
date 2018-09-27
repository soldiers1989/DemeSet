using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：订单明细表 
    /// </summary>
    public class OrderDetail:BaseEntity<Guid>
    {
       /// <summary>
       /// 订单编码
       /// </summary>     
       public string OrderNo{ get; set; }

       /// <summary>
       /// 课程编码
       /// </summary>     
       public string CourseId{ get; set; }

       /// <summary>
       /// 课程原价
       /// </summary>     
       public decimal? Price{ get; set; }

       /// <summary>
       /// 课程优惠价
       /// </summary>     
       public decimal? FavourablePrice{ get; set; }

       /// <summary>
       /// 课程数量
       /// </summary>     
       public int? Num{ get; set; }

       /// <summary>
       /// 合计
       /// </summary>     
       public decimal? Amount{ get; set; }

       /// <summary>
       /// 课程类型(0:课程 1：套餐)
       /// </summary>     
       public int? CourseType{ get; set; }

    }
}

