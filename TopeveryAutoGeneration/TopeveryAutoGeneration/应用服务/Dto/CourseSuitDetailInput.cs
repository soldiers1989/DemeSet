using System;
using Topevery.Application.BaseDto;

namespace Topevery.Application.Dto
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
    }
}

