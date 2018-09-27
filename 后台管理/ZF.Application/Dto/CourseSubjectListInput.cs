using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：CourseSubject 
    /// </summary>
    public class CourseSubjectListInput: BasePageInput
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
