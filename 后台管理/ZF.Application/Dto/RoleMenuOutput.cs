using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输出Output:角色菜单关系表 
    /// </summary>
    public class RoleMenuOutput
    {
       /// <summary>
       /// 主键编号
       /// </summary>     
      public string Id{ get; set; }
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

