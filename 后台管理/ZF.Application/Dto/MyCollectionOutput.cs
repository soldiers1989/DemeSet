using System;
using ZF.Core;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 数据表MyCollection 输出Dto
    /// </summary>
    public class MyCollectionOutput: CourseInfo
    {
       /// <summary>
       /// 编码
       /// </summary>     
      public string Id{ get; set; }
       /// <summary>
       /// 课程编码
       /// </summary>     
      public string CourseId{ get; set; }
       /// <summary>
       /// 用户编码
       /// </summary>     
      public string UserId{ get; set; }
       /// <summary>
       /// 收藏时间
       /// </summary>     
      public DateTime? AddTime{ get; set; }
    }

    /// <summary>
    /// 我的收藏具体信息
    /// </summary>
    public class MyCollectionAndCourseOutput
    {
        /// <summary>
        /// 编码
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 课程编码
        /// </summary>     
        public string CourseId { get; set; }
        /// <summary>
        /// 用户编码
        /// </summary>     
        public string UserId { get; set; }
        /// <summary>
        /// 收藏时间
        /// </summary>     
        public DateTime? AddTime { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>     
        public string CourseName { get; set; }
        /// <summary>
        /// 课程封面
        /// </summary>     
        public string CourseIamge { get; set; }

        /// <summary>
        /// 课程简介
        /// </summary>     
        public string CourseContent { get; set; }

        /// <summary>
        /// 原价
        /// </summary>     
        public decimal Price { get; set; }

        /// <summary>
        /// 优惠价
        /// </summary>     
        public decimal FavourablePrice { get; set; }
    }
}

