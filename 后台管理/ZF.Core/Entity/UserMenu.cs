using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：用户菜单关系表 
    /// </summary>
    public class UserMenu:BaseEntity<Guid>
    {
       /// <summary>
       /// 用户编号
       /// </summary>     
       public string UserId{ get; set; }

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

