using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：CourseSuitDetail 
    /// </summary>
    public class CourseSuitDetail:BaseEntity<Guid>
    {
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

