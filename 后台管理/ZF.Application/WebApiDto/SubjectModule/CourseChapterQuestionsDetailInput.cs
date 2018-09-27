using System;
using System.Collections.Generic;
using ZF.Core.Entity;

namespace ZF.Application.WebApiDto.SubjectModule
{
    /// <summary>
    /// 
    /// </summary>
    public class CourseChapterQuestionsDetailInput
    {
        /// <summary>
        /// 
        /// </summary>
        public  List<CourseChapterQuestionsDetailModel> CourseChapterQuestionsDetail { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public  string UserId { get; set; }

        /// <summary>
        /// 练习编号
        /// </summary>
        public  string ChapterQuestionsId { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public class CourseChapterQuestionsDetailModel
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
        public string QuestionId { get; set; }

        /// <summary>
        /// 试题类型
        /// </summary>     
        public int Type { get; set; }

        /// <summary>
        /// 考生答案
        /// </summary>     
        public string StuAnswer { get; set; }

        /// <summary>
        /// 得分
        /// </summary>     
        public decimal? Score { get; set; }

        /// <summary>
        /// 时间
        /// </summary>     
        public DateTime AddTime { get; set; }
    }
}