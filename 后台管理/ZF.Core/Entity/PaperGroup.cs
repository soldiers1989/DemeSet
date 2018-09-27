using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：试卷组表 
    /// </summary>
    public class PaperGroup:FullAuditEntity<Guid>
    {
       /// <summary>
       /// 试卷名称
       /// </summary>     
       public string PaperGroupName{ get; set; }

       /// <summary>
       /// 所属科目编码
       /// </summary>     
       public string SubjectId{ get; set; }

       /// <summary>
       /// 试卷状态
       /// </summary>     
       public int? State{ get; set; }

       /// <summary>
       /// 试卷属性  0历年真题 1模拟试卷
       /// </summary>     
       public int Type{ get; set; }

    }
}

