using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：角色表 
    /// </summary>
   [AutoMap(typeof(Role ))]
    public class RoleInput
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

