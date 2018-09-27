using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：CoursePackcourseInfo 
    /// </summary>
    public class CoursePackcourseInfoListInput: BasePageInput
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
        /// 讲师ID
        /// </summary>
        public string TeacherId { get; set; }

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
       /// 创建人
       /// </summary>     
       public string AddUserId{ get; set; }
       /// <summary>
       /// 修改时间
       /// </summary>     
       public DateTime? UpdateTime{ get; set; }
       /// <summary>
       /// 修改人
       /// </summary>     
       public string UpdateUserId{ get; set; }
       /// <summary>
       /// 是否删除
       /// </summary>     
       public int IsDelete{ get; set; }

        /// <summary>
        /// 是否邮寄讲义
        /// </summary>
        public int EmailNotes { get; set; }

    }
}
