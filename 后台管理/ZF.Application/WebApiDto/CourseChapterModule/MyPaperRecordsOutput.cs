using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Core;

namespace ZF.Application.WebApiDto.CourseChapterModule
{
   public class MyPaperRecordsOutput : BaseEntity<Guid>
    {
        /// <summary>
        /// 用户编码
        /// </summary>     
        public string UserId { get; set; }

        /// <summary>
        /// 试卷编码
        /// </summary>     
        public string PaperId { get; set; }

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
        public int RowNumber
        {
            get; set;
        }

    }

    public class MyPaperRecordsJgOutput
    {
        /// <summary>
        /// 作答数量
        /// </summary>
        public string c1 { get; set; }

        /// <summary>
        /// 答对数量
        /// </summary>
        public string c2 { get; set; }


        /// <summary>
        /// 试卷名称
        /// </summary>
        public string PaperName { get; set; }

        /// <summary>
        /// 得分
        /// </summary>
        public  decimal? Score { get; set; }

        /// <summary>
        /// 练习时间
        /// </summary>
        public DateTime? AddTime { get; set; }

    }
}
