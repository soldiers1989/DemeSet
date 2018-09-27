using System;
using ZF.Application.BaseDto;

namespace ZF.Application.WebApiDto.MyCourseModule
{
    /// <summary>
    /// 查询输入Input：我的足迹 
    /// </summary>
    public class MyFootprintListInput:BasePageInput
    {
        /// <summary>
        /// 用户编号
        /// </summary>     
        public string UserId { get; set; }

        /// <summary>
        /// 查询时间类型  0全部  1最近一周  2最近一月  3最近一年
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 查询条件
        /// </summary>
        public string query { get; set; }
    }
}
