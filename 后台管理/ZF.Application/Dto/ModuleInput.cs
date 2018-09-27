using System;
using ZF.AutoMapper.AutoMapper;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：Module 
    /// </summary>
   [AutoMap(typeof(ZF.Core.Entity.Module))]
    public class ModuleInput
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

