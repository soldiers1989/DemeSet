using System;
using Topevery.Application.BaseDto;

namespace Topevery.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：CourseOnTeachers 
    /// </summary>
   [AutoMap(typeof(CourseOnTeachers ))]
    public class CourseOnTeachersInput
    {
       /// <summary>
       /// 编码
       /// </summary>     
       public string Id{ get; set; }
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
       /// <summary>
       /// 创建时间
       /// </summary>     
       public DateTime AddTime{ get; set; }
       /// <summary>
       /// 创建人
       /// </summary>     
       public string AddUserId{ get; set; }
       /// <summary>
       /// 修改时间
       /// </summary>     
       public DateTime? UpdateTime{ get; set; }
       /// <summary>
       /// 修改人
       /// </summary>     
       public string UpdateUserId{ get; set; }
       /// <summary>
       /// 是否删除
       /// </summary>     
       public int IsDelete{ get; set; }
    }
}

