using System;
using System.Collections.Generic;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：PaperInfo 
    /// </summary>
    public class PaperInfo : FullAuditEntity<Guid>
    {
        /// <summary>
        /// 试卷名称
        /// </summary>     
        public string PaperName { get; set; }

        /// <summary>
        /// 试卷参数编码
        /// </summary>     
        public string PaperParamId { get; set; }

        /// <summary>
        /// 所属科目编码
        /// </summary>     
        public string SubjectId { get; set; }

        /// <summary>
        /// 考试时长
        /// </summary>     
        public int? TestTime { get; set; }

        /// <summary>
        /// 试卷状态
        /// </summary>     
        public int? State { get; set; }
        /// <summary>
        /// 类别 0：历年真题 1：模拟试卷
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 试题
        /// </summary>
        
        public List<PaperDetatail> PaperDetatailList { get; set; }

    }
}

