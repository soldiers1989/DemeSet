using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：SubjectKnowledgePoint 
    /// </summary>
    public class SubjectKnowledgePoint : FullAuditEntity<Guid>
    {
        /// <summary>
        /// 知识点名称
        /// </summary>     
        public string KnowledgePointName { get; set; }

        /// <summary>
        /// 知识点代码
        /// </summary>     
        public string KnowledgePointCode { get; set; }

        /// <summary>
        /// 所属科目
        /// </summary>     
        public string SubjectId { get; set; }

        /// <summary>
        /// 上级知识点编码
        /// </summary>     
        public string ParentId { get; set; }

        /// <summary>
        /// 电子书页面
        /// </summary>
        public string DigitalBookPage { get; set; }

    }
}

