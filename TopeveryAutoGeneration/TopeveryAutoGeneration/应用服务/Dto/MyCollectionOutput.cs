using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 数据表MyCollection 输出Dto
    /// </summary>
    public class MyCollectionOutput
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

