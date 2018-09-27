using System;
using System.Collections.Generic;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输出Output:编码 
    /// </summary>
    public class DeliveryAddressOutput
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
        public int? DefaultAddress { get; set; }
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

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 讲义地址
        /// </summary>
        public string HandOutId { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 快递公司
        /// </summary>
        public List<Express> ExpressList { get; set; }

        /// <summary>
        /// 查询方式
        /// </summary>
        public int SelectType { get; set; }

        /// <summary>
        /// 快递公司ID
        /// </summary>
        public string ExpressCompanyId { get; set; }

        /// <summary>
        /// 快递公司名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 快递编号
        /// </summary>
        public string ExpressNumber { get; set; }
    }

    /// <summary>
    /// 快递公司
    /// </summary>
    public class Express
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}

