using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：Menu 
    /// </summary>
    public class MenuListInput: BasePageInput
    {
       /// <summary>
       /// 主键编号
       /// </summary>     
       public string Id{ get; set; }
       /// <summary>
       /// 模块编号
       /// </summary>     
       public string ModuleId{ get; set; }
       /// <summary>
       /// 菜单名称
       /// </summary>     
       public string MenuName{ get; set; }
       /// <summary>
       /// 路径
       /// </summary>     
       public string Url{ get; set; }
       /// <summary>
       /// 排序号
       /// </summary>     
       public int? Sort{ get; set; }
       /// <summary>
       /// 样式
       /// </summary>     
       public string Class{ get; set; }
       /// <summary>
       /// 描述
       /// </summary>     
       public string Description{ get; set; }
    }
}
