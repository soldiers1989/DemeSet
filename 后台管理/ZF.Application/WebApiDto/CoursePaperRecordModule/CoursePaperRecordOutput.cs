using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Application.WebApiDto.CoursePaperRecordModule
{
    /// <summary>
    /// 
    /// </summary>
    public class CoursePaperRecordOutput
    {

        public string Id { get; set; }
        /// <summary>
        /// 用户编码
        /// </summary>     
        public string UserId { get; set; }
        /// <summary>
        /// 课程编号
        /// </summary>
        public string CourseId { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 试卷编码
        /// </summary>     
        public string PaperId { get; set; }

        /// <summary>
        /// 试卷名称
        /// </summary>
        public string PaperName { get; set; }

        /// <summary>
        /// 得分
        /// </summary>     
        public decimal? Score { get; set; }


        /// <summary>
        /// 试卷总分
        /// </summary>     
        public decimal? ScoreSum { get; set; }

        /// <summary>
        /// 试卷练习序号
        /// </summary>
        public string PracticeNo { get; set; }

        /// <summary>
        /// 时间
        /// </summary>     
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>     
        public int? Status { get; set; }

        /// <summary>
        /// 练习排名
        /// </summary>
        public int RowNumber1
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public class CoursePaperRecordMoreOutput : CoursePaperRecordOutput {
            /// <summary>
            /// 用时
            /// </summary>
            public int UseTime { get; set; }
            /// <summary>
            /// 总题数
            /// </summary>
            public int Total { get; set; }
            /// <summary>
            /// 答错题数
            /// </summary>
            public int ErrorCount { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class CoursePaperClassByPaper {
            /// <summary>
            /// 试卷名称
            /// </summary>
            public string PaperName { get; set; }
            /// <summary>
            /// 试卷练习记录
            /// </summary>
            public List<CoursePaperRecordOutput> PaperRecordList { get; set; }
        }
    }
}
