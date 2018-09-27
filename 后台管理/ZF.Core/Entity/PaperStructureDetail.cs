using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：PaperStructureDetail 
    /// </summary>
    public class PaperStructureDetail : FullAuditEntity<Guid>
    {
        /// <summary>
        /// 试卷结构编码
        /// </summary>     
        public string StuctureId { get; set; }

        /// <summary>
        /// 题型名称
        /// </summary>     
        public string QuesitonTypeName { get; set; }

        /// <summary>
        /// 题型类型
        /// </summary>     
        public string QuestionType { get; set; }

        /// <summary>
        /// 题型分类
        /// </summary>     
        public string QuestionClass { get; set; }

        /// <summary>
        /// 难度等级
        /// </summary>     
        public int? DifficultLevel { get; set; }

        /// <summary>
        /// 试题数量
        /// </summary>     
        public int QuestionCount { get; set; }

        /// <summary>
        /// 题型总分
        /// </summary>     
        public int QuestionTypeScoreSum { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>     
        public int OrderNo { get; set; }

    }
}

