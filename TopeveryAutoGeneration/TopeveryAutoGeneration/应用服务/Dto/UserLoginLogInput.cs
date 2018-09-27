using System;
using Topevery.Application.BaseDto;

namespace Topevery.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：UserLoginLog 
    /// </summary>
   [AutoMap(typeof(UserLoginLog ))]
    public class UserLoginLogInput
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

