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
        public string Id { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>     
        public string TelphoneNum { get; set; }
        /// <summary>
        /// 登录时间
        /// </summary>     
        public DateTime? LoginTime { get; set; }
        /// <summary>
        /// 登录IP
        /// </summary>     
        public string LoginIp { get; set; }
        /// <summary>
        /// 登录终端(方式)
        /// </summary>     
        public string LoginType { get; set; }

        /// <summary>
        /// 登录终端(方式)
        /// </summary>     
        public string LoginTypeName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public  string NickNamw { get; set; }
    }
}

