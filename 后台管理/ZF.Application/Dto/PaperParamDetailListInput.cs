using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：PaperParamDetail 
    /// </summary>
    public class PaperParamDetailListInput : BasePageInput
    {
        /// <summary>
        /// 编码
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 试卷参数编码
        /// </summary>     
        public string PaperParamId { get; set; }
        /// <summary>
        /// 试卷结构明细
        /// </summary>     
        public string PaperStuctureDetailId { get; set; }
        /// <summary>
        /// 知识点编码
        /// </summary>     
        public string KnowledgePointId { get; set; }
        /// <summary>
        /// 试题数量
        /// </summary>     
        public int? QuestionCount { get; set; }
        /// <summary>
        /// 难度等级
        /// </summary>     
        public int? DifficultLevel { get; set; }
        /// <summary>
        /// 试题分数
        /// </summary>     
        public int? QuestionScoreSum { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>     
        public DateTime AddTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>     
        public string AddUserId { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>     
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>     
        public string UpdateUserId { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>     
        public int IsDelete { get; set; }

        /// <summary>
        /// 试卷结构编码
        /// </summary>
        public string StuctureId { get; set; }

        /// <summary>
        /// 组卷类型
        /// </summary>
        public int GroupType { get; set; }

        /// <summary>
        /// 试卷ID
        /// </summary>
        public string PaperId { get; set; }
    }
}
