using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 数据表PaperInfo 输出Dto
    /// </summary>
    public class PaperInfoOutput
    {
        /// <summary>
        /// 编码
        /// </summary>     
        public string Id { get; set; }
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
        /// 所属科目编码
        /// </summary>     
        public string SubjectName { get; set; }
        /// <summary>
        /// 考试时长
        /// </summary>     
        public int? TestTime { get; set; }
        /// <summary>
        /// 试卷状态
        /// </summary>     
        public int? State { get; set; }
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
        /// 总分
        /// </summary>
        public string QuestionScore { get; set; }

        /// <summary>
        /// 试卷类别
        /// </summary>
        public int Type { get; set; }
    }
}

