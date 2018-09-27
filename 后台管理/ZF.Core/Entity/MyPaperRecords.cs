using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：试卷作答记录表 
    /// </summary>
    public class MyPaperRecords : BaseEntity<Guid>
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


    }

}

