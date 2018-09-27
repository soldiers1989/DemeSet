using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 数据表SmsendLog 输出Dto
    /// </summary>
    public class SmsendLogOutput
    {
       /// <summary>
       /// 记录编码
       /// </summary>     
      public string Id{ get; set; }
       /// <summary>
       /// 手机号
       /// </summary>     
      public string TelhopheNum{ get; set; }
       /// <summary>
       /// 发送时间
       /// </summary>     
      public DateTime SendTime{ get; set; }
       /// <summary>
       /// 验证码
       /// </summary>     
      public string Code{ get; set; }
    }
}

