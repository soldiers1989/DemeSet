using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：课程章节试题练习(明细)表 
    /// </summary>
    public class CourseChapterQuestionsDetail : BaseEntity<Guid>
    {
        /// <summary>
        /// 课程章节练习编号
        /// </summary>     
        public string ChapterQuestionsId { get; set; }

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

