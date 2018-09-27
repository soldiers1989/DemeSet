using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：PaperParam 
    /// </summary>
    public class PaperParam:FullAuditEntity<Guid>
    {
       /// <summary>
       /// 试卷结构明细编码
       /// </summary>     
       public string PaperStructureDetailId{ get; set; }

       /// <summary>
       /// 知识点编码
       /// </summary>     
       public string KnowledgePointId{ get; set; }

       /// <summary>
       /// 试题数量
       /// </summary>     
       public int? QuestionCount{ get; set; }

       /// <summary>
       /// 难度级别
       /// </summary>     
       public int? DifficultLevel{ get; set; }

       /// <summary>
       /// 试题分数
       /// </summary>     
       public int? QuestionScoreSum{ get; set; }

    }
}

