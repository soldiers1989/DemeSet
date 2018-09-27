using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：SubjectKnowledgePoint 
    /// </summary>
    public class SubjectKnowledgePointListInput : BasePageInput
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
        /// 项目编号
        /// </summary>     
        public string ProjectId { get; set; }

        /// <summary>
        /// 所属项目分类
        /// </summary>     
        public string ProjectClassId { get; set; }


    }
}
