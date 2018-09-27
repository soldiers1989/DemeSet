namespace ZF.Application.Dto.SalesReports
{
    /// <summary>
    /// 销售订单统计输出Dto
    /// </summary>
    public class SalesReportsOutPut
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public  string Date { get; set; }

        /// <summary>
        /// 金额总计
        /// </summary>
        public string TotalMoney { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public  int? TradeCount { get; set; }
    }

    /// <summary>
    /// 课程销售统计
    /// </summary>
    public class CourseSaleReport {
        /// <summary>
        /// 課程名稱
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 课程销售数量
        /// </summary>
        public int CourseCount { get; set; }
    }

   /// <summary>
   /// 专业课程销售统计
   /// </summary>
    public class SubjectSaleReport {
        /// <summary>
        /// 专业名称
        /// </summary>
        public string SubjectClassName { get; set; }
        /// <summary>
        /// 专业销售统计
        /// </summary>
        public int SubjectSaleCount { get; set; }
    }
}