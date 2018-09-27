using System;

namespace ZF.Core
{
    /// <summary>
    /// 默认基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseEntity<T> where T : struct
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public string Id { set; get; }
    }

    /// <summary>
    /// 审核基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AuditEntity<T> : BaseEntity<T> where T : struct
    {
        /// <summary>
        /// 更新者Id
        /// </summary>
        public string UpdateUserId { set; get; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { set; get; }

        /// <summary>
        /// 创建者Id
        /// </summary>
        public string AddUserId { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime AddTime { set; get; }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DeleteEntity<T> : BaseEntity<T> where T : struct
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDelete { set; get; } = 0;
    }

    /// <summary>
    /// 删除基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FullAuditEntity<T> : AuditEntity<T> where T : struct
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDelete { set; get; }
    }
    
}