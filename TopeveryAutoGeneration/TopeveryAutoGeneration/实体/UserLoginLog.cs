using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：UserLoginLog 
    /// </summary>
    public class UserLoginLog:BaseEntity<Guid>
    {
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

