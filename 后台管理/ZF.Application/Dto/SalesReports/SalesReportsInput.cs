namespace ZF.Application.Dto.SalesReports
{
    /// <summary>
    /// 销售报表统计输入实体类
    /// </summary>
    public class SalesReportsInput
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }
    }
}