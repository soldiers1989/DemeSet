using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Application.WebApiDto.CourseLearnProgressModule
{
    public class CourseLearnProgressModelOutput
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 课程ID
        /// </summary>
        public string CourseId { get; set; }

        /// <summary>
        /// 章节ID
        /// </summary>
        public string ChapterId { get; set; }

        /// <summary>
        /// 状态 0：正在学习 1：已完成学习
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 视频Id
        /// </summary>
        public string VideoId { get; set; }
        /// <summary>
        /// 视频名称
        /// </summary>
        public string VideoName { get; set; }

        /// <summary>
        /// 上次观看
        /// </summary>
        public string LastWatch { get; set; }

        /// <summary>
        /// 总课时数
        /// </summary>
        public int TotalLearnCount { get; set; }

        /// <summary>
        /// 已学习课时数
        /// </summary>
        public int LearnedCount { get; set; }
    }
}
