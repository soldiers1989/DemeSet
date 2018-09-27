using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：角色表 
    /// </summary>
    public class Role: FullAuditEntity<Guid>
    {
       /// <summary>
       /// 角色名称
       /// </summary>     
       public string RoleName{ get; set; }

       /// <summary>
       /// 描述
       /// </summary>     
       public string Description{ get; set; }

    }
}

