using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Application.WebApiDto.ChapterExerciseModule
{
    public class CourseChapterExerciseOuput
    {

        public string Id { get; set; }
        /// <summary>
        /// 用户编码
        /// </summary>     
        public string UserId { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>     
        public string CourseName { get; set; }

        /// <summary>
        /// 章节名称
        /// </summary>     
        public string VideoName { get; set; }

        /// <summary>
        /// 时间
        /// </summary>     
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>     
        public int? Status { get; set; }

        /// <summary>
        /// 练习编号
        /// </summary>
        public string PracticeNo { get; set; }

        /// <summary>
        /// 完成率
        /// </summary>
        public string Completion { get; set; }

        /// <summary>
        /// 正确率
        /// </summary>
        public string Correct { get; set; }
    }
}
