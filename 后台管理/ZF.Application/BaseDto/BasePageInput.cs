namespace ZF.Application.BaseDto
{
    /// <summary>
    /// 分页排序基类
    /// </summary>
    public class BasePageInput
    {
        /// <summary>
        /// 用于指定从哪一条数据开始显示到表格中去
        /// </summary>
        public int? Rows { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string Sidx { get; set; } = "Id";

        /// <summary>
        /// 排序方式
        /// </summary>
        public string Sord { get; set; } = "Desc";


    }
}