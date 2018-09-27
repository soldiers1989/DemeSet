using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输出Output:面试班 
    /// </summary>
    public class CourseFaceToFaceOutput
    {
        /// <summary>
        /// 课程编码
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 班级名称
        /// </summary>     
        public string ClassName { get; set; }

        /// <summary>
        /// 上课地点
        /// </summary>     
        public string Address { get; set; }

        /// <summary>
        /// 优惠价
        /// </summary>     
        public decimal FavourablePrice { get; set; }

        /// <summary>
        /// 上课人数
        /// </summary>     
        public int? Number { get; set; }

        /// <summary>
        /// 上下架状态
        /// </summary>     
        public int? State { get; set; }

        /// <summary>
        /// 线上报名人数
        /// </summary>
        public  string Count { get; set; }

    }
}

