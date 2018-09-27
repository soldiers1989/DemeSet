using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：使用指南 
    /// </summary>
    public class UseDescription:FullAuditEntity<Guid>
    {
       /// <summary>
       /// 大类编号
       /// </summary>     
       public string BigClassId { get; set; }

       /// <summary>
       /// 小类编号
       /// </summary>     
       public string ClassId{ get; set; }

       /// <summary>
       /// 内容
       /// </summary>     
       public string Content{ get; set; }

    }
}

