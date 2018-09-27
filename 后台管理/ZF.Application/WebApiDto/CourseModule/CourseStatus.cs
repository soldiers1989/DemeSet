namespace ZF.Application.WebApiDto.CourseModule
{
    /// <summary>
    /// 课程子菜单状态
    /// </summary>
    public class CourseStatus
    {
        /// <summary>
        /// 课程编号
        /// </summary>
        public string CourseId { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }
        /// <summary>
        /// 课程封面
        /// </summary>
        public string CourseIamge { get; set; }

        /// <summary>
        /// 章节数量
        /// </summary>
        public int zjCount { get; set; }

        /// <summary>
        /// 模拟试卷数量
        /// </summary>
        public int mnsjCount { get; set; }

        /// <summary>
        /// 历年真题数量
        /// </summary>
        public int lnztCount { get; set; }

        /// <summary>
        /// 讲义数量
        /// </summary>
        public int jyCount { get; set; }


        /// <summary>
        /// 0  关闭评价功能  1  开放评价功能
        /// </summary>
        public int ArguValue { get; set; }

        /// <summary>
        /// 是否增值服务
        /// </summary>
        public int IsValueAdded { get; set; }

    }
}