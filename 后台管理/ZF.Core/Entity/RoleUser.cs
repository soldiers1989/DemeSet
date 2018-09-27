using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：角色人员关系表 
    /// </summary>
    public class RoleUser:BaseEntity<Guid>
    {
       /// <summary>
       /// 角色编号
       /// </summary>     
       public string RoleId{ get; set; }

       /// <summary>
       /// 用户编号
       /// </summary>     
       public string UserId{ get; set; }

    }
}

