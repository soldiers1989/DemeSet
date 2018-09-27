using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 数据表PaperDetatail 输出Dto
    /// </summary>
    public class PaperDetatailOutput
    {
       /// <summary>
       /// 编码
       /// </summary>     
      public string Id{ get; set; }
       /// <summary>
       /// 试卷编码
       /// </summary>     
      public string PaperId{ get; set; }
       /// <summary>
       /// 试题编码
       /// </summary>     
      public string QuestionId{ get; set; }
       /// <summary>
       /// 试题类型
       /// </summary>     
      public string QuestionTypeId{ get; set; }
       /// <summary>
       /// 试题分数
       /// </summary>     
      public decimal QuestionScore{ get; set; }
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

