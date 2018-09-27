using System;
using ZF.Core;

namespace ZF.Application.WebApiDto.MyCourseModule
{
    /// <summary>
    /// 查询输出Output:视频观看历史记录 
    /// </summary>
    public class MyVideoWatchOutput
    {

        /// <summary>
        /// 章节编号
        /// </summary>
        public string chapterId { get; set; }

        /// <summary>
        /// 章节名称
        /// </summary>
        public string chapterName { get; set; }

        /// <summary>
        /// 课程封面
        /// </summary>
        public string CourseIamge { get; set; }

        /// <summary>
        /// 课程编号
        /// </summary>
        public string courseId { get; set; }

        /// <summary>
        /// 编码
        /// </summary>     
        public string Id { get; set; }

        /// <summary>
        /// 视频名称
        /// </summary>     
        public string VideoId { get; set; }

        /// <summary>
        /// 观看时间
        /// </summary>     
        public DateTime WatchTime { get; set; }

        /// <summary>
        /// 视频名称
        /// </summary>
        public string VideoName { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }
    }
}

