using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输出Output:文本配置表(联系我们,注册协议) 
    /// </summary>
    public class DanyeOutput
    {
       /// <summary>
       /// 主键编号
       /// </summary>     
      public string Id{ get; set; }
       /// <summary>
       /// 编码
       /// </summary>     
      public string Code{ get; set; }
       /// <summary>
       /// 名称
       /// </summary>     
      public string Name{ get; set; }
       /// <summary>
       /// 内容
       /// </summary>     
      public string Content{ get; set; }
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

