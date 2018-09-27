using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：CourseVideo 
    /// </summary>
    [AutoMap(typeof(CourseVideo))]
    public class CourseVideoInput
    {
        /// <summary>
        /// 编码
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 视频名称
        /// </summary>     
        public string VideoName { get; set; }
        /// <summary>
        /// 视频地址
        /// </summary>     
        public string VideoUrl { get; set; }
        /// <summary>
        /// 所属章节
        /// </summary>     
        public string ChapterId { get; set; }

        /// <summary>
        /// 视频时长(秒)
        /// </summary>
        public int? VideoLong { get; set; }

        /// <summary>
        /// 视频时长 小时:分钟:秒
        /// </summary>     
        public string VideoLongTime { get; set; }
        /// <summary>
        /// 课程目标
        /// </summary>     
        public string VideoContent { get; set; }
        /// <summary>
        /// 是否可试听
        /// </summary>     
        public int? IsTaste { get; set; }
        /// <summary>
        /// 可试听的时长(分钟)
        /// </summary>     
        public int? TasteLongTime { get; set; }
        /// <summary>
        /// 学习次数
        /// </summary>     
        public int? StudyCount { get; set; }

        /// <summary>
        /// 文件集合
        /// </summary>
        public string IdFilehiddenFile { get; set; }

        /// <summary>
        /// 视频编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 二维码标题
        /// </summary>
        public string QcodeTitle { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int OrderNo { get; set; }

        /// <summary>
        /// 未登录状态可试听时长
        /// </summary>
        public int TasteLongTime2 { get; set; }
    }
}

