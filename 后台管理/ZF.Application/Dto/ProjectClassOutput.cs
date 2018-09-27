using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 数据表ProjectClass 输出Dto
    /// </summary>
    public class ProjectClassOutput
    {
       /// <summary>
       /// 项目分类编码
       /// </summary>     
      public string Id{ get; set; }
       /// <summary>
       /// 项目分类名称
       /// </summary>     
      public string ProjectClassName{ get; set; }
       /// <summary>
       /// 分类说明
       /// </summary>     
      public string Remark{ get; set; }
       /// <summary>
       /// 排序号
       /// </summary>     
      public int? OrderNo{ get; set; }
       /// <summary>
       /// 创建时间
       /// </summary>     
      public DateTime AddTime{ get; set; }
    }
}

