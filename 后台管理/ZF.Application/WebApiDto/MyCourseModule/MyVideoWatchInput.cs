using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.WebApiDto.MyCourseModule
{
    /// <summary>
    /// 新增修改输入Input：视频观看历史记录 
    /// </summary>
    [AutoMap(typeof(MyVideoWatch))]
    public class MyVideoWatchInput
    {
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
        /// 用户编号
        /// </summary>     
        public string UserId { get; set; }
    }
}

