using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Core.Entity
{
   public class SubjectPracticeQuestions:BaseEntity<Guid>
    {
        /// <summary>
        /// 练习编号
        /// </summary>     
        public string PracticeNo { get; set; }

        /// <summary>
        /// 用户编码
        /// </summary>     
        public string UserId { get; set; }

        /// <summary>
        /// 试题编码
        /// </summary>     
        public string BigQuestionId { get; set; }

        /// <summary>
        /// 小题编码
        /// </summary>     
        public string SmallQuestionId { get; set; }

        /// <summary>
        /// 考生答案
        /// </summary>     
        public string StuAnswer { get; set; }

        /// <summary>
        /// 时间
        /// </summary>     
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 是否正确
        /// </summary>
        public int IsCorrect { get; set; }
    }
}
