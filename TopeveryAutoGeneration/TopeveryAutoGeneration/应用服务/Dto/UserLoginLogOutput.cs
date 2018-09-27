using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 数据表UserLoginLog 输出Dto
    /// </summary>
    public class UserLoginLogOutput
    {
       /// <summary>
       /// 日志编码
       /// </summary>     
      public string Id{ get; set; }
       /// <summary>
       /// 登录用户编码
       /// </summary>     
      public string UserId{ get; set; }
       /// <summary>
       /// 登录时间
       /// </summary>     
      public DateTime? LoginTime{ get; set; }
       /// <summary>
       /// 登录IP
       /// </summary>     
      public string LoginIp{ get; set; }
       /// <summary>
       /// 登录终端(方式)
       /// </summary>     
      public string LoginType{ get; set; }
    }
}

