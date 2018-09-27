using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：SmsendLog 
    /// </summary>
    public class SmsendLog : BaseEntity<Guid>
    {
        /// <summary>
        /// 手机号
        /// </summary>     
        public string TelphoneNum { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>     
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 验证码过期时间
        /// </summary>     
        public DateTime FailureTime { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>     
        public string Code { get; set; }



    }
}

