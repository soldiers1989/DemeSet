using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：CourseSuitDetail 
    /// </summary>
   [AutoMap(typeof(CourseSuitDetail ))]
    public class CourseSuitDetailInput
    {
       /// <summary>
       /// 编码
       /// </summary>     
       public string Id{ get; set; }
       /// <summary>
       /// 套餐课程编码
       /// </summary>     
       public string PackCourseId{ get; set; }
       /// <summary>
       /// 课程编码
       /// </summary>     
       public string CourseId{ get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int OrderNo { get; set; }
    }
}

