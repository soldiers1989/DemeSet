using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 数据表CourseSubject 输出Dto
    /// </summary>
    public class CourseSubjectOutput
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

