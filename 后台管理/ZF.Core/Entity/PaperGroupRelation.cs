using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：试卷组试卷关系表 
    /// </summary>
    public class PaperGroupRelation:BaseEntity<Guid>
    {
       /// <summary>
       /// 试卷组编号
       /// </summary>     
       public string PaperGroupId{ get; set; }

       /// <summary>
       /// 试卷编号
       /// </summary>     
       public string PaperId{ get; set; }

    }
}

