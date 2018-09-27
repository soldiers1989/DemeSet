using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：MyCourse 
    /// </summary>
    public class MyCourse : BaseEntity<Guid>
    {
        /// <summary>
        /// 用户编码
        /// </summary>     
        public string UserId { get; set; }

        /// <summary>
        /// 课程编码
        /// </summary>     
        public string CourseId { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>     
        public DateTime? AddTime { get; set; }

        /// <summary>
        /// 课程有效开始时间
        /// </summary>     
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// 课程有效结束时间
        /// </summary>     
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 课程类型(0:课程 1：套餐)
        /// </summary>     
        public int? CourseType { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderId { get; set; }

    }
}

