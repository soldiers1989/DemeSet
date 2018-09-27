using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Core.Entity
{
   public class CourseLearnProgress: BaseEntity<Guid>
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
    }
}
