using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.WebApiDto.CoursediscussionModule
{
    /// <summary>
    /// 
    /// </summary>
    [AutoMap( typeof( CourseDiscussion ) )]
    public class CourseDiscussionModelInput : BasePageInput
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
    }
}
