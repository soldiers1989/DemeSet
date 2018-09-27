using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：SubjectClass 
    /// </summary>
    public class SubjectClass : FullAuditEntity<Guid>
    {
        /// <summary>
        /// 题型名称
        /// </summary>     
        public string ClassName { get; set; }

        /// <summary>
        /// 题型所属项目
        /// </summary>     
        public string ProjectId { get; set; }

        /// <summary>
        /// 题型描述
        /// </summary>     
        public string Remark { get; set; }

        /// <summary>
        /// 试题表现形式(大题)
        /// </summary>     
        public int? BigType { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>     
        public int? OrderNo { get; set; }

        /// <summary>
        /// 评分规则？
        /// </summary>     
        public string Column_6 { get; set; }

    }
}

