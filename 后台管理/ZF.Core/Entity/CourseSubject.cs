using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：CourseSubject 
    /// </summary>
    public class CourseSubject:BaseEntity<Guid>
    {
       /// <summary>
       /// 课程编码
       /// </summary>     
       public string CourseId{ get; set; }

       /// <summary>
       /// 章节编码
       /// </summary>     
       public string ChapterId{ get; set; }

       /// <summary>
       /// 试题编码
       /// </summary>     
       public string SubjectId{ get; set; }

    }
}

