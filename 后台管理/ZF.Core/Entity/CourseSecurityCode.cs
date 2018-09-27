using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：课程防伪码管理 
    /// </summary>
    public class CourseSecurityCode : BaseEntity<Guid>
    {
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
        /// 使用用户编号
        /// </summary>     
        public string UserId { get; set; }

        /// <summary>
        /// 领取课程时间
        /// </summary>
        public DateTime? GetDateTime { get; set; }

        /// <summary>
        /// 是否增值服务  1是 0否
        /// </summary>
        public  int IsValueAdded { get; set; }

    }
}

