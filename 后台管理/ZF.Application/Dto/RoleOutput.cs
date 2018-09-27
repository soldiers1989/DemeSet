using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输出Output:角色表 
    /// </summary>
    public class RoleOutput
    {
       /// <summary>
       /// 主键编号
       /// </summary>     
      public string Id{ get; set; }
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

