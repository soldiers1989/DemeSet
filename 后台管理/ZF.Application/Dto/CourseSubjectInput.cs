using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：CourseSubject 
    /// </summary>
   [AutoMap(typeof(CourseSubject ))]
    public class CourseSubjectInput
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
       /// 章节编码
       /// </summary>     
       public string ChapterId{ get; set; }
       /// <summary>
       /// 试题编码
       /// </summary>     
       public string SubjectId{ get; set; }
    }
}

