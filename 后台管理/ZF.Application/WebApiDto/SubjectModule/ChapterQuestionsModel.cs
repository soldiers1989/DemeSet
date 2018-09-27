using System;

namespace ZF.Application.WebApiDto.SubjectModule
{
    /// <summary>
    /// 
    /// </summary>
    public class ChapterQuestionsModel
    {

        /// <summary>
        /// 练习编号
        /// </summary>
        public string PracticeNo { get; set; }

        /// <summary>
        /// 练习标题
        /// </summary>
        public  string ChapterTiele { get; set; }
        /// <summary>
        ///章节名称
        /// </summary>
        public  string ChapterName { get; set; }

        /// <summary>
        ///章节编号
        /// </summary>
        public string ChapterId { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 考试时间
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// 章节试题统计
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 练习编号
        /// </summary>
        public string ChapterQuestionsId { get; set; }
    }
}