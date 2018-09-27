using System;
using ZF.Core;

namespace ZF.Application.WebApiDto.MyCourseModule
{
    /// <summary>
    /// 查询输出Output:我的足迹 
    /// </summary>
    public class MyFootprintOutput
    {
        /// <summary>
        /// 课程编号
        /// </summary>     
        public string CourseId { get; set; }

        /// <summary>
        /// 课程类别
        /// </summary>     
        public int CourseType { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 封面图片
        /// </summary>
        public string CourseIamge { get; set; }

        /// <summary>
        /// 浏览时间
        /// </summary>
        public DateTime? BrowsingTime { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
    }
}

