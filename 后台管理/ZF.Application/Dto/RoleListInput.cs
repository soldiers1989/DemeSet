using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：角色表 
    /// </summary>
    public class RoleListInput: BasePageInput
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
