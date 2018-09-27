namespace ZF.Application.WebApiDto.CourseModule
{
    /// <summary>
    /// 课程模块
    /// </summary>
    public class CourseInfoModel
    {
        /// <summary>
        /// 课程编码
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 课程所属科目
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// 课程所属科目编号
        /// </summary>
        public string SubjectId { get; set; }

        /// <summary>
        /// 课程封面
        /// </summary>
        public string CourseIamge { get; set; }

        /// <summary>
        /// 主键教师
        /// </summary>
        public string TeachersName { get; set; }

        /// <summary>
        /// 主键教师编号
        /// </summary>
        public string TeacherId { get; set; }

        /// <summary>
        /// 原价
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// 优惠价
        /// </summary>
        public string FavourablePrice { get; set; }

        /// <summary>
        /// 课程时长
        /// </summary>
        public string CourseLongTime { get; set; }

        /// <summary>
        /// 课件数
        /// </summary>
        public string CourseWareCount { get; set; }

        /// <summary>
        /// 项目编号
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// 销售数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 课程类型(0:课程 1：套餐)
        /// </summary>
        public int? CourseType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 是否邮寄讲义
        /// </summary>
        public int EmailNotes { get; set; }
    }
}