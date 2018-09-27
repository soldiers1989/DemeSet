using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：Module 
    /// </summary>
    public class ModuleListInput: BasePageInput
    {
       /// <summary>
       /// 编号
       /// </summary>     
       public string Id{ get; set; }
       /// <summary>
       /// 模块名称
       /// </summary>     
       public string ModuleName{ get; set; }
       /// <summary>
       /// 样式
       /// </summary>     
       public string Class{ get; set; }
       /// <summary>
       /// 排序号
       /// </summary>     
       public int? Sort{ get; set; }
    }
}
