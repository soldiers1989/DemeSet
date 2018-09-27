using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：角色菜单关系表 
    /// </summary>
    public class RoleMenu:BaseEntity<Guid>
    {
       /// <summary>
       /// 角色编号
       /// </summary>     
       public string RoleId{ get; set; }

       /// <summary>
       /// 菜单编号
       /// </summary>     
       public string MenuId{ get; set; }

       /// <summary>
       /// 类型  0 模块  1菜单
       /// </summary>     
       public int? Type{ get; set; }

    }
}

