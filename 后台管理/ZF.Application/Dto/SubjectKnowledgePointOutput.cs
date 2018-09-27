using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 数据表SubjectKnowledgePoint 输出Dto
    /// </summary>
    public class SubjectKnowledgePointOutput
    {
        /// <summary>
        /// 知识点编码
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 知识点名称
        /// </summary>     
        public string KnowledgePointName { get; set; }
        /// <summary>
        /// 知识点代码
        /// </summary>     
        public string KnowledgePointCode { get; set; }
        /// <summary>
        /// 所属科目名称
        /// </summary>     
        public string SubjectName { get; set; }

        /// <summary>
        /// 所属科目编号
        /// </summary>     
        public string SubjectId { get; set; }

        /// <summary>
        /// 上级知识点编码
        /// </summary>     
        public string ParentId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>     
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 上级知识点名称
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// 电子书页面
        /// </summary>
        public string DigitalBookPage { get; set; }
    }
}

