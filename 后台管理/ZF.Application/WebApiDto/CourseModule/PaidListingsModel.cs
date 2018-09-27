namespace ZF.Application.WebApiDto.CourseModule
{
    /// <summary>
    /// 课程排名输出模型
    /// </summary>
    public class PaidListingsModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 课程类型(0:课程 1：套餐)
        /// </summary>
        public int? CourseType { get; set; }
    }
}