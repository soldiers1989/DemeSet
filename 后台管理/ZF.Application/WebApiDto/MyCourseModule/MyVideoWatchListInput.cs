using System;
using ZF.Application.BaseDto;

namespace ZF.Application.WebApiDto.MyCourseModule
{
    /// <summary>
    /// 查询输入Input：视频观看历史记录 
    /// </summary>
    public class MyVideoWatchListInput : BasePageInput
    {
        /// <summary>
        /// 用户编号
        /// </summary>     
        public string UserId { get; set; }

        /// <summary>
        /// 查询条件
        /// </summary>
        public  string query { get; set; }

    }
}
