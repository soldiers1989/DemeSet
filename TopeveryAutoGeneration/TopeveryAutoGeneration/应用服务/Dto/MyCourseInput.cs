using System;
using Topevery.Application.BaseDto;

namespace Topevery.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：MyCourse 
    /// </summary>
   [AutoMap(typeof(MyCourse ))]
    public class MyCourseInput
    {
       /// <summary>
       /// 编码
       /// </summary>     
       public string Id{ get; set; }
       /// <summary>
       /// 用户编码
       /// </summary>     
       public string UserId{ get; set; }
       /// <summary>
       /// 课程编码
       /// </summary>     
       public string CourseId{ get; set; }
       /// <summary>
       /// 添加时间
       /// </summary>     
       public DateTime? AddTime{ get; set; }
       /// <summary>
       /// 课程有效开始时间
       /// </summary>     
       public DateTime? BeginTime{ get; set; }
       /// <summary>
       /// 课程有效结束时间
       /// </summary>     
       public DateTime? EndTime{ get; set; }
    }
}

