using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：编码 
    /// </summary>
    public class DeliveryAddressListInput: BasePageInput
    {
       /// <summary>
       /// 编码
       /// </summary>     
       public string Id{ get; set; }
       /// <summary>
       /// 用户编码
       /// </summary>     
       public string UserId{ get; set; }
       /// <summary>
       /// 联系人
       /// </summary>     
       public string Contact{ get; set; }
       /// <summary>
       /// 联系电话
       /// </summary>     
       public string ContactPhone{ get; set; }
       /// <summary>
       /// 邮编
       /// </summary>     
       public string Zip{ get; set; }
       /// <summary>
       /// 详细地址
       /// </summary>     
       public string DetailedAddress{ get; set; }
       /// <summary>
       /// 默认地址
       /// </summary>     
       public int? DefaultAddress{ get; set; }
       /// <summary>
       /// 省
       /// </summary>     
       public string Province{ get; set; }
       /// <summary>
       /// 城市
       /// </summary>     
       public string City{ get; set; }
       /// <summary>
       /// 街道,镇
       /// </summary>     
       public string Town{ get; set; }
       /// <summary>
       /// 添加时间
       /// </summary>     
       public DateTime? AddTime{ get; set; }

        /// <summary>
        /// 查询方式
        /// </summary>
        public int SelectType { get; set; }
    }
}
