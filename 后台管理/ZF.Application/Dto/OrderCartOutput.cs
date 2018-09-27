using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输出Output:购物车表 
    /// </summary>
    public class OrderCartOutput
    {
       /// <summary>
       /// 编码
       /// </summary>     
      public string Id{ get; set; }
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

