using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：购物车表 
    /// </summary>
    [AutoMap(typeof(OrderCart))]
    public class OrderCartInput
    {
        /// <summary>
        /// 编码
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>     
        public string OrderNo { get; set; }
        /// <summary>
        /// 用户编码
        /// </summary>     
        public string RegisterUserId { get; set; }

        /// <summary>
        /// 购物车明细编码
        /// </summary>
        public string CartDetailId { get; set; }

        /// <summary>
        /// 订单明细编码
        /// </summary>
        public string DetailOrderNo { get; set; }

        /// <summary>
        /// 课程编码
        /// </summary>
        public string CourseId { get; set; }

        /// <summary>
        /// 课程原价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 课程优惠价
        /// </summary>
        public decimal FavourablePrice { get; set; }

        /// <summary>
        /// 课程数量
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 合计
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 课程类型
        /// </summary>
        public int CourseType { get; set; }

    }
}

