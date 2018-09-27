using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Application.WebApiDto.CoursediscussionModule
{
    /// <summary>
    /// 
    /// </summary>
   public class CourseDiscussionModelOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        ///课程Id
        /// </summary>
        public string CourseId { get; set; }
        /// <summary>
        /// 发起讨论人
        /// </summary>
        public string CreateUserId { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string NickNamw { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 讨论内容
        /// </summary>
        public string DiscussContent { get; set; }
        /// <summary>
        /// 上一级讨论ID
        /// </summary>
        public string ParentId { get; set; }
        /// <summary>
        /// 讨论类型 1：老师问答 0：综合讨论
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 子评论
        /// </summary>
        public List<CourseDiscussionModelOutput> SubDiscussion { get; set; }
    }
}
