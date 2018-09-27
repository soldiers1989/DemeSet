using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输出Output:课程防伪码管理 
    /// </summary>
    public class CourseSecurityCodeOutput
    {
        /// <summary>
        /// 编码
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 课程编码
        /// </summary>     
        public string CourseId { get; set; }
        /// <summary>
        /// 防伪码
        /// </summary>     
        public string Code { get; set; }
        /// <summary>
        /// 是否使用
        /// </summary>     
        public int? IsUse { get; set; }

        /// <summary>
        /// 使用用户昵称
        /// </summary>     
        public string NickNamw { get; set; }

        /// <summary>
        /// 使用时间
        /// </summary>
        public DateTime? GetDateTime { get; set; }

        /// <summary>
        /// 是否增值服务  1是 0否
        /// </summary>
        public int IsValueAdded { get; set; }

    }
}

