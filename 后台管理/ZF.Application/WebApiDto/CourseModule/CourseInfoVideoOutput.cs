using System.Collections.Generic;
using ZF.Application.WebApiDto.CourseChapterModule;

namespace ZF.Application.WebApiDto.CourseModule
{
    /// <summary>
    ///     
    /// </summary>
    public class CourseInfoVideoOutput
    {
        /// <summary>
        /// 课程编码
        /// </summary>     
        public string Id { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>     
        public string CourseName { get; set; }

        /// <summary>
        /// 课程封面
        /// </summary>     
        public string CourseIamge { get; set; }

        /// <summary>
        /// 评价次数  wyf 2018-05-16 add
        /// </summary> 
        public int? AppraiseNum { get; set; }

        /// <summary>
        /// 评价分数
        /// </summary>
        public int EvaluationScore { get; set; }

        /// <summary>
        /// 章节视频列表
        /// </summary>
        public List<CourseChapterModelOutput> CourseChapterModelOutput { get; set; }

    }
}