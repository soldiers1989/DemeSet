using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.WebApiDto.MyCourseModule
{
    /// <summary>
    /// 新增修改输入Input：我的足迹 
    /// </summary>
    [AutoMap(typeof(MyFootprint))]
    public class MyFootprintInput
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
        /// 浏览时间
        /// </summary>     
        public DateTime? BrowsingTime { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>     
        public string UserId { get; set; }
    }
}

