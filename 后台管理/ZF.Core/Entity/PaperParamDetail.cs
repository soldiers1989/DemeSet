using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：PaperParamDetail 
    /// </summary>
    public class PaperParamDetail:FullAuditEntity<Guid>
    {
       /// <summary>
       /// 试卷参数编码
       /// </summary>     
       public string PaperParamId{ get; set; }

       /// <summary>
       /// 试卷结构明细
       /// </summary>     
       public string PaperStuctureDetailId{ get; set; }

       /// <summary>
       /// 知识点编码
       /// </summary>     
       public string KnowledgePointId{ get; set; }

       /// <summary>
       /// 试题数量
       /// </summary>     
       public int? QuestionCount{ get; set; }

       /// <summary>
       /// 难度等级
       /// </summary>     
       public int? DifficultLevel{ get; set; }

       /// <summary>
       /// 试题分数
       /// </summary>     
       public int? QuestionScoreSum{ get; set; }

    }
}

