using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 数据表CourseResource 输出Dto
    /// </summary>
    public class CourseResourceOutput
    {
       /// <summary>
       /// 编码
       /// </summary>     
      public string Id{ get; set; }
       /// <summary>
       /// 资源名称
       /// </summary>     
      public string ResourceName{ get; set; }
       /// <summary>
       /// 资源url
       /// </summary>     
      public string ResourceUrl{ get; set; }
       /// <summary>
       /// 资源大小
       /// </summary>     
      public string ResourceSize{ get; set; }
       /// <summary>
       /// 课程编码
       /// </summary>     
      public string CourseId{ get; set; }
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

