using System;
using Topevery.Application.BaseDto;

namespace Topevery.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：MyCollection 
    /// </summary>
   [AutoMap(typeof(MyCollection ))]
    public class MyCollectionInput
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
       /// 用户编码
       /// </summary>     
       public string UserId{ get; set; }
       /// <summary>
       /// 收藏时间
       /// </summary>     
       public DateTime? AddTime{ get; set; }
    }
}

