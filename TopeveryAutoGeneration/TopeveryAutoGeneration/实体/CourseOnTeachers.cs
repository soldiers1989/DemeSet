using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：CourseOnTeachers 
    /// </summary>
    public class CourseOnTeachers:FullAuditEntity<Guid>
    {
       /// <summary>
       /// 教师名称
       /// </summary>     
       public string TeachersName{ get; set; }

       /// <summary>
       /// 标签
       /// </summary>     
       public string TheLabel{ get; set; }

       /// <summary>
       /// 简介
       /// </summary>     
       public string Synopsis{ get; set; }

    }
}

