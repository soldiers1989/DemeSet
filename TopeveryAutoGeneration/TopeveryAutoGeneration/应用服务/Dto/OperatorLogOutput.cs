using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 数据表OperatorLog 输出Dto
    /// </summary>
    public class OperatorLogOutput
    {
       /// <summary>
       /// 主键Id
       /// </summary>     
      public string Id{ get; set; }
       /// <summary>
       /// 模块Id
       /// </summary>     
      public int ModuleId{ get; set; }
       /// <summary>
       /// 标识Id
       /// </summary>     
      public string KeyId{ get; set; }
       /// <summary>
       /// 操作人
       /// </summary>     
      public string OperatorName{ get; set; }
       /// <summary>
       /// 操作时间
       /// </summary>     
      public DateTime OperatorDate{ get; set; }
       /// <summary>
       /// 操作类型
       /// </summary>     
      public int? OperatorType{ get; set; }
       /// <summary>
       /// 操作人Id
       /// </summary>     
      public string OperatorId{ get; set; }
       /// <summary>
       /// 操作内容
       /// </summary>     
      public string Remark{ get; set; }
       /// <summary>
       /// 操作人Ip
       /// </summary>     
      public string OperatorIp{ get; set; }
    }
}

