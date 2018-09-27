using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输出Output:网站相关配置表 
    /// </summary>
    public class SysSetOutput
    {
       /// <summary>
       /// 主键编号
       /// </summary>     
      public string Id{ get; set; }
       /// <summary>
       /// 名称
       /// </summary>     
      public string Name{ get; set; }
       /// <summary>
       /// 参数名称
       /// </summary>     
      public string ArguName{ get; set; }
       /// <summary>
       /// 参数值
       /// </summary>     
      public string ArguValue{ get; set; }
       /// <summary>
       /// 备注
       /// </summary>     
      public string Remark{ get; set; }
       /// <summary>
       /// 创建时间
       /// </summary>     
      public DateTime AddTime{ get; set; }
       /// <summary>
       /// 创建人
       /// </summary>     
      public string AddUserId{ get; set; }
       /// <summary>
       /// 修改时间
       /// </summary>     
      public DateTime? UpdateTime{ get; set; }
       /// <summary>
       /// 修改人
       /// </summary>     
      public string UpdateUserId{ get; set; }
       /// <summary>
       /// 是否删除
       /// </summary>     
      public bool IsDelete{ get; set; }
    }
}

