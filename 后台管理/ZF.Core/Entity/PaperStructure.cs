using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：PaperStructure 
    /// </summary>
    public class PaperStructure:FullAuditEntity<Guid>
    {
       /// <summary>
       /// 试卷结构名称
       /// </summary>     
       public string StuctureName{ get; set; }

       /// <summary>
       /// 所属科目
       /// </summary>     
       public string SubjectId{ get; set; }

    }
}

