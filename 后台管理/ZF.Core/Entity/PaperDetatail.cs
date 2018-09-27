using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：试卷明细表 
    /// </summary>
    public class PaperDetatail:FullAuditEntity<Guid>
    {
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
       public int QuestionTypeId{ get; set; }

       /// <summary>
       /// 试题分数
       /// </summary>     
       public decimal QuestionScore{ get; set; }

    }
}

