using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.WebApiDto.DeliveryAddressModule
{
    /// <summary>
    /// 输出类
    /// </summary>
    [AutoMap(typeof(DeliveryAddress))]
    public class DeliveryAddressModelInput: BasePageInput
    {
        /// <summary>
        /// 编码
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 用户编码
        /// </summary>     
        public string UserId { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>     
        public string Contact { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>     
        public string ContactPhone { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>     
        public string Zip { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>     
        public string DetailedAddress { get; set; }
        /// <summary>
        /// 默认地址
        /// </summary>     
        public int? DefaultAddress { get; set; } = 0;
        /// <summary>
        /// 省
        /// </summary>     
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>     
        public string City { get; set; }
        /// <summary>
        /// 街道,镇
        /// </summary>     
        public string Town { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>     
        public DateTime? AddTime { get; set; }
    }
}
