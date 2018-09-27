using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 数据表CourseAppraise 输出Dto
    /// </summary>
    public class CourseAppraiseOutput
    {
        /// <summary>
        /// 编码
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 评价内容
        /// </summary>     
        public string AppraiseCotent { get; set; }
        /// <summary>
        /// 评价用户
        /// </summary>     
        public string UserId { get; set; }
        /// <summary>
        /// 评价级别
        /// </summary>     
        public int? AppraiseLevel { get; set; }
        /// <summary>
        /// 评价时间
        /// </summary>     
        public DateTime? AppraiseTime { get; set; }
        /// <summary>
        /// 评价IP
        /// </summary>     
        public string AppraiseIp { get; set; }
        /// <summary>
        /// 回复时间
        /// </summary>     
        public DateTime? ReplyTime { get; set; }
        /// <summary>
        /// 回复内容
        /// </summary>     
        public string ReplyContent { get; set; }
        /// <summary>
        /// 回复人
        /// </summary>     
        public string ReplyAdminUserId { get; set; }
        /// <summary>
        /// 课程编码
        /// </summary>     
        public string CourseId { get; set; }
        /// <summary>
        /// 课程类型(0:课程 1：套餐)
        /// </summary>     
        public int? CourseType { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public int? AuditStatus { get; set; }
    }
}

