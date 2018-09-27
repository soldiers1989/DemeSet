using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：CoursePackcourseInfo 
    /// </summary>
    public class CoursePackcourseInfo:FullAuditEntity<Guid>
    {
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

    }
}

