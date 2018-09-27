using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 防伪码使用情况输入Dto
    /// </summary>
    public class SecurityCodeUsageInput : BasePageInput
    {
        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 类型 0：课程 1：题库
        /// </summary>
        public int? Type { get; set; }

        /// <summary>
        /// 防伪码编码
        /// </summary>
        public string Code { get; set; }
    }
}