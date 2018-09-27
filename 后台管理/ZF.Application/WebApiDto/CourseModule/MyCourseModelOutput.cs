using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Application.WebApiDto.CourseModule
{
    public class MyCourseModelOutput
    {

        /// <summary>
        /// 课程编码
        /// </summary>     
        public string CourseId { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>     
        public string CourseName { get; set; }


        /// <summary>
        /// 课程封面
        /// </summary>     
        public string CourseIamge { get; set; }

        /// <summary>
        /// 课件数
        /// </summary>     
        public int? CourseWareCount { get; set; }

        /// <summary>
        /// 已学习课时数
        /// </summary>
        public int HasLearnCount { get; set; }
        /// <summary>
        /// 课程测评数
        /// </summary>
        public int PracticePaperNum { get; set; }

        /// <summary>
        /// 课程测评总数
        /// </summary>
        public int TotalPaperNum { get; set; }

        /// <summary>
        /// 章节练习总数
        /// </summary>
        public int TotalChapterPractice { get; set; }
        /// <summary>
        /// 个人章节练习数--同一个章节练习练习多次只计一次
        /// </summary>
        public int ChapterPracticeNum { get; set; }

        public string VideoId { get; set; }

        /// <summary>
        /// 最近观看视频名称
        /// </summary>
        public string VideoName { get; set; }
        /// <summary>
        /// 收藏时间
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 有效日期
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 课程类型  套餐   课程
        /// </summary>
        public int CourseType { get; set; }

        /// <summary>
        /// 课程类型    课程  题库
        /// </summary>
        public int? Type { get; set; }
    }
}
