using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：Subject 
    /// </summary>
    public class Subject : FullAuditEntity<Guid>
    {
        /// <summary>
        /// 科目名称
        /// </summary>     
        public string SubjectName { get; set; }

        /// <summary>
        /// 所属项目
        /// </summary>     
        public string ProjectId { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>     
        public int OrderNo { get; set; }

        /// <summary>
        /// 科目说明
        /// </summary>     
        public string Remark { get; set; }

        /// <summary>
        /// 是否经济基础
        /// </summary>
        public int IsEconomicBase { get; set; }

    }
}

