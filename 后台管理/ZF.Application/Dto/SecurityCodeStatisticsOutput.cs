namespace ZF.Application.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class SecurityCodeStatisticsOutput
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
        /// 已领取数量
        /// </summary>
        public int? Count1 { get; set; }

        /// <summary>
        /// 总数量
        /// </summary>
        public int? Count2 { get; set; }
    }
}