using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：CourseChapter 
    /// </summary>
    public class CourseChapter : FullAuditEntity<Guid>
    {
        /// <summary>
        /// 章节名称
        /// </summary>     
        public string CapterName { get; set; }

        /// <summary>
        /// 父节点编码
        /// </summary>     
        public string ParentId { get; set; }

        /// <summary>
        /// 章节代码
        /// </summary>     
        public string CapterCode { get; set; }

        /// <summary>
        /// 所属课程
        /// </summary>     
        public string CourseId { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>     
        public int OrderNo { get; set; }


    }
}

