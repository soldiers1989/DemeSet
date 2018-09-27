using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 数据表PaperStructure 输出Dto
    /// </summary>
    public class PaperStructureOutput
    {
       /// <summary>
       /// 编码
       /// </summary>     
      public string Id{ get; set; }
       /// <summary>
       /// 试卷结构名称
       /// </summary>     
      public string StuctureName{ get; set; }
       /// <summary>
       /// 所属科目
       /// </summary>     
      public string SubjectId{ get; set; }
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

