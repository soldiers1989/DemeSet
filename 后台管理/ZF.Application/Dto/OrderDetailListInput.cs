using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：OrderDetail 
    /// </summary>
    public class OrderDetailListInput: BasePageInput
    {
       /// <summary>
       /// 订单编码
       /// </summary>     
       public string OrderNo{ get; set; }
    
    }
}
