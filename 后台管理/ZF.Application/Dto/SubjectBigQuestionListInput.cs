using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：SubjectBigQuestion 
    /// </summary>
    public class SubjectBigQuestionListInput : BasePageInput
    {
        /// <summary>
        /// 试题标题
        /// </summary>     
        public string QuestionTitle { get; set; }
        /// <summary>
        /// 试题内容
        /// </summary>     
        public string SubjectType { get; set; }
        /// <summary>
        /// 所属科目
        /// </summary>     
        public string SubjectId { get; set; }

        /// <summary>
        /// 试题所属知识点编码
        /// </summary>     
        public string KnowledgePointId { get; set; }

        /// <summary>
        /// 试题难度
        /// </summary>
        public int? DifficultLevel { get; set; }

        /// <summary>
        /// 项目分类编号
        /// </summary>
        public string ProjectClassId { get; set; }


        /// <summary>
        /// 项目编号
        /// </summary>
        public string ProjectId { get; set; }


        /// <summary>
        /// 题型
        /// </summary>
        public string SubjectClassId { get; set; }
        
    }
}
