using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 数据表CourseChapter 输出Dto
    /// </summary>
    public class CourseChapterOutput
    {
       /// <summary>
       /// 编码
       /// </summary>     
      public string Id{ get; set; }
       /// <summary>
       /// 章节名称
       /// </summary>     
      public string CapterName{ get; set; }
       /// <summary>
       /// 父节点编码
       /// </summary>     
      public string ParentId{ get; set; }
       /// <summary>
       /// 章节代码
       /// </summary>     
      public string CapterCode{ get; set; }
       /// <summary>
       /// 所属课程
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

