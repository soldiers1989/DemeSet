using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 数据表CoursePackcourseInfo 输出Dto
    /// </summary>
    public class CoursePackcourseInfoOutput
    {
       /// <summary>
       /// 课程编码
       /// </summary>     
      public string Id{ get; set; }
       /// <summary>
       /// 课程名称
       /// </summary>     
      public string CourseName{ get; set; }
       /// <summary>
       /// 课程封面
       /// </summary>     
      public string CourseIamge{ get; set; }
       /// <summary>
       /// 课程简介
       /// </summary>     
      public string CourseContent{ get; set; }
       /// <summary>
       /// 原价
       /// </summary>     
      public decimal Price{ get; set; }
       /// <summary>
       /// 优惠价
       /// </summary>     
      public decimal FavourablePrice{ get; set; }
       /// <summary>
       /// 有效期天数
       /// </summary>     
      public int? ValidityPeriod{ get; set; }
       /// <summary>
       /// 上下架状态
       /// </summary>     
      public int? State{ get; set; }
       /// <summary>
       /// 是否置顶
       /// </summary>     
      public int? IsTop{ get; set; }
       /// <summary>
       /// 是否推荐
       /// </summary>     
      public int? IsRecommend{ get; set; }
       /// <summary>
       /// 课程标签
       /// </summary>     
      public string CourseTag{ get; set; }
       /// <summary>
       /// 课程时长
       /// </summary>     
      public int? CourseLongTime{ get; set; }
       /// <summary>
       /// 课件数
       /// </summary>     
      public int? CourseWareCount{ get; set; }
       /// <summary>
       /// 创建时间
       /// </summary>     
      public DateTime AddTime{ get; set; }

        /// <summary>
        /// 报班（购买）人数
        /// </summary>
        public int LearnCount { get; set; }

        /// <summary>
        /// 子课程数
        /// </summary>
        public int CourseCount { get; set; }

        /// <summary>
        /// 浏览量
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// 所属类目
        /// </summary>
        public string ClassId { get; set; }

        /// <summary>
        /// SEO 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// SEO 关键字
        /// </summary>
        public string KeyWord { get; set; }
        /// <summary>
        /// SEO 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否邮寄讲义
        /// </summary>
        public int EmailNotes { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int OrderNo { get; set; }

        /// <summary>
        /// 有效截止日期
        /// </summary>
        public DateTime? ValidityEndDate { get; set; }

    }
}

