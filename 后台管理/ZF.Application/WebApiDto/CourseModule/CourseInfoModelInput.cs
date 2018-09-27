using ZF.Application.BaseDto;

namespace ZF.Application.WebApiDto.CourseModule
{
    /// <summary>
    /// 课程查询输入模型
    /// </summary>
    public class CourseInfoModelInput : BasePageInput
    {
        /// <summary>
        /// 课程名称
        /// </summary>     
        public string CourseName { get; set; }
        /// <summary>
        /// 项目分类编号
        /// </summary>
        public string ProjectClassId { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// 科目编号
        /// </summary>
        public string SubjectId { get; set; }

        /// <summary>
        /// 优惠价起
        /// </summary>
        public decimal? FavourablePriceQ { get; set; }

        /// <summary>
        /// 优惠价止
        /// </summary>
        public decimal? FavourablePriceZ { get; set; }

        /// <summary>
        /// 是否推荐
        /// </summary>
        public int? IsRecommend { get; set; }

        /// <summary>
        /// 课程类型(0:课程 1：套餐  2  面授课)
        /// </summary>
        public int? CourseType { get; set; }

        /// <summary>
        /// 讲师ID
        /// </summary>
        public string TeacherId { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public int? IsFree { get; set; }

        /// <summary>
        /// 查询条件
        /// </summary>
        public string query { get; set; }

        /// <summary>
        /// 是否邮寄讲义
        /// </summary>
        public int EmailNotes { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }


        /// <summary>
        /// 课程编号
        /// </summary>
        public string CourseId { get; set; }

        /// <summary>
        /// 0：课程 1 题库
        /// </summary>
        public int? Type { get; set; }

        /// <summary>
        /// 是否增值服务
        /// </summary>
        public int? IsValueAdded { get; set; }
    }
}