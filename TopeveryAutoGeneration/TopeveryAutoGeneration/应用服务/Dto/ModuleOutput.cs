using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 数据表Module 输出Dto
    /// </summary>
    public class ModuleOutput
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

