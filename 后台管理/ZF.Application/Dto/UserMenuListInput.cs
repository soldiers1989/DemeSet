using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：用户菜单关系表 
    /// </summary>
    public class UserMenuListInput: BasePageInput
    {
       /// <summary>
       /// 主键编号
       /// </summary>     
       public string Id{ get; set; }
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
