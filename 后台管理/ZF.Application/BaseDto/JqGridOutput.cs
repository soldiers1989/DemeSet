using System.Collections.Generic;

namespace ZF.Application.BaseDto
{
    /// <summary>
    /// 用于JqGrid插件返回数据格式
    /// </summary>
    public class JqGridOutPut<T> where T:class 
    {
        private List<T> list;
        private int? rows;
        private int count;

        public JqGridOutPut(List<T> list, int? page, int? rows, int count)
        {
            this.list = list;
            Page = page;
            this.rows = rows;
            this.count = count;
        }

        public JqGridOutPut()
        {
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int? Total { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// 查询出的记录数
        /// </summary>
        public int? Records { get; set; }

        /// <summary>
        /// 包含实际数据的数组
        /// </summary>
        public List<T> Rows { get; set; }

    }
}