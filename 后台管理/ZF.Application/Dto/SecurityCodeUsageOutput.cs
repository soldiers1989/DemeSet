using System;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 防伪码领取情况输出Dto
    /// </summary>
    public class SecurityCodeUsageOutput
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 领取时间
        /// </summary>
        public DateTime? GetDateTime { get; set; }

        /// <summary>
        /// 是否增值服务
        /// </summary>
        public int? IsValueAdded { get; set; }

        /// <summary>
        /// 防伪码编码
        /// </summary>
        public string Code { get; set; }
    }
}