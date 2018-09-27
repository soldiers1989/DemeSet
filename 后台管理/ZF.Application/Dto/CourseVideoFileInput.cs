using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：视频文件管理 
    /// </summary>
    [AutoMap(typeof(CourseVideoFile))]
    public class CourseVideoFileInput
    {
        /// <summary>
        /// 编码
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 视频封面
        /// </summary>     
        public string CoverURL { get; set; }
        /// <summary>
        /// 视频名称
        /// </summary>     
        public string Name { get; set; }
        /// <summary>
        /// 视频类型
        /// </summary>     
        public string Type { get; set; }
        /// <summary>
        /// 视频时长
        /// </summary>     
        public float? Duration { get; set; }

        /// <summary>
        /// MP4播放地址
        /// </summary>
        public string VideoUrl { get; set; }

        /// <summary>
        /// 视频别名
        /// </summary>
        public string VideoAlias { get; set; }
    }
}

