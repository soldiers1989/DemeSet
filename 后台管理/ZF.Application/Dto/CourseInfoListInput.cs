using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：CourseInfo 
    /// </summary>
    public class CourseInfoListInput: BasePageInput
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
        /// 课程视频简介
        /// </summary>
        public string CourseVideo { get; set; }
       /// <summary>
       /// 课程所属科目
       /// </summary>     
       public string SubjectId{ get; set; }
        /// <summary>
        /// 课程所属项目
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// 课程所属类目
        /// </summary>
        public string ClassId { get; set; }

        /// <summary>
        /// 课程所属项目分类
        /// </summary>
        public string ProjectClassId { get; set; }
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
       /// 主讲教师
       /// </summary>     
       public string TeacherId{ get; set; }
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
        /// 浏览量
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// 是否邮寄讲义
        /// </summary>
        public int EmailNotes { get; set; }
        /// <summary>
        /// 类型 0：课程 1：题库
        /// </summary>
        public int? Type { get; set; }

    }

    /// <summary>
    /// 不包括指定套餐课程中的课程
    /// </summary>
    public class CourseInfoListExceptInPackCourse : CourseInfoListInput {

        /// <summary>
        /// 套餐课程ID
        /// </summary>
        public string PackCourseId { get; set; }
        /// <summary>
        /// 项目分类ID
        /// </summary>
        public string ProjectClassId { get; set; }
    }
}
