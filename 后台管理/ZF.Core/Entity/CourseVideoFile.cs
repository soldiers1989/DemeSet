using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：视频文件管理 
    /// </summary>
    public class CourseVideoFile : BaseEntity<Guid>
    {
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

