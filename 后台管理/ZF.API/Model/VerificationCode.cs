using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZF.API.Model
{
    /// <summary>
    /// 验证码验证输入
    /// </summary>
    public class VerificationCode
    {
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string TelphoneNum { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
    }
}