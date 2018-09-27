using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Infrastructure.PublicPaper
{
    /// <summary>
    /// 试卷表--页面参数传递
    /// </summary>
    public class PublicPaperInfo
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
        /// 考试时长
        /// </summary>
        public int TestTime { get; set; }
        /// <summary>
        /// 试题数量
        /// </summary>
        public int QuestionCount { get; set; }
        /// <summary>
        /// 试题分数
        /// </summary>
        public int QuestionScoreSum { get; set; }
        /// <summary>
        /// 试题编码
        /// </summary>
        public string QuestionId { get; set; }

        /// <summary>
        /// 试卷id
        /// </summary>
        public string PaperId { get; set; }

        public int Type { get; set; }
    }


}
