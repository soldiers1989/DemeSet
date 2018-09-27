using System;
using System.Collections.Generic;
using ZF.Application.WebApiDto.CourseModule;

namespace ZF.Application.WebApiDto.CoursePackModule
{
    /// <summary>
    /// 
    /// </summary>
    public class CoursePackcourseInfoModelOutput
    {
        /// <summary>
        /// 课程编码
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>     
        public string CourseName { get; set; }
        /// <summary>
        /// 课程封面
        /// </summary>     
        public string CourseIamge { get; set; }
       
        /// <summary>
        /// 原价
        /// </summary>     
        public decimal Price { get; set; }
        /// <summary>
        /// 优惠价
        /// </summary>     
        public decimal FavourablePrice { get; set; }
       
        /// <summary>
        /// 课程数
        /// </summary>     
        public int? CourseWareCount { get; set; }

        /// <summary>
        /// 是否邮寄讲义
        /// </summary>
        public int EmailNotes { get; set; }
    }
}