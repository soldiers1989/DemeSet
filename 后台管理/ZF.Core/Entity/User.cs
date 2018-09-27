using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：User 
    /// </summary>
    public class User:FullAuditEntity<Guid>
    {
       /// <summary>
       /// 手机号
       /// </summary>     
       public string Phone{ get; set; }

       /// <summary>
       /// 用户昵称
       /// </summary>     
       public string UserName{ get; set; }

       /// <summary>
       /// 密码
       /// </summary>     
       public string PassWord{ get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 是否管理员
        /// </summary>
        public int IsAdmin { get; set; }

    }
}

