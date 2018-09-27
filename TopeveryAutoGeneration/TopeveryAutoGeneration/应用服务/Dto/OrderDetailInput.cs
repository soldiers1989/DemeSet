using System;
using Topevery.Application.BaseDto;

namespace Topevery.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：OrderDetail 
    /// </summary>
   [AutoMap(typeof(OrderDetail ))]
    public class OrderDetailInput
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

