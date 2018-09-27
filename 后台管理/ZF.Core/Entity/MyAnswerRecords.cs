using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 试卷作答记录明细表（试题）
    /// </summary>
    public class MyAnswerRecords:BaseEntity<Guid>
    {
        /// <summary>
        /// 试卷作答编号
        /// </summary>
        public string PaperRecordsId { get; set; }

       /// <summary>
       /// 用户编码
       /// </summary>     
        public string UserId{ get; set; }

       /// <summary>
       /// 试题编码
       /// </summary>     
       public string BigQuestionId{ get; set; }

       /// <summary>
       /// 小题编码
       /// </summary>     
       public string SmallQuestionId{ get; set; }

       /// <summary>
       /// 试卷编码
       /// </summary>     
       public string PaperId{ get; set; }

       /// <summary>
       /// 考生答案
       /// </summary>     
       public string StuAnswer{ get; set; }

       /// <summary>
       /// 得分
       /// </summary>     
       public decimal? Score{ get; set; }

       /// <summary>
       /// 时间
       /// </summary>     
       public DateTime AddTime{ get; set; }

    }
}

