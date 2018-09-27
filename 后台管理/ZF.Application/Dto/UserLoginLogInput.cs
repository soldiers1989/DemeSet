using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.Dto
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
       public int? LoginType{ get; set; }
    }
}

