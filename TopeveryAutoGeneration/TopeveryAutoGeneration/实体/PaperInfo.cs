using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：PaperInfo 
    /// </summary>
    public class PaperInfo:FullAuditEntity<Guid>
    {
       /// <summary>
       /// 试卷名称
       /// </summary>     
       public string PaperName{ get; set; }

       /// <summary>
       /// 试卷结构编码
       /// </summary>     
       public string PaperStructureId{ get; set; }

       /// <summary>
       /// 所属科目编码
       /// </summary>     
       public string SubjectId{ get; set; }

       /// <summary>
       /// 考试时长
       /// </summary>     
       public int? TestTime{ get; set; }

       /// <summary>
       /// 试卷状态
       /// </summary>     
       public int? State{ get; set; }

    }
}

