using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 数据表SubjectClass 输出Dto
    /// </summary>
    public class SubjectClassOutput
    {
        /// <summary>
        /// 分类编码
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>     
        public string ClassName { get; set; }
        /// <summary>
        /// 分类所属项目
        /// </summary>     
        public string ProjectName { get; set; }
        /// <summary>
        /// 分类描述
        /// </summary>     
        public string Remark { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>     
        public int? OrderNo { get; set; }
        /// <summary>
        /// 评分规则？
        /// </summary>     
        public string Column_6 { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>     
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 试题表现形式(大题)
        /// </summary>     
        public int? BigType { get; set; }

        /// <summary>
        /// 试题表现形式(大题)
        /// </summary>     
        public string BigTypeName { get; set; }
    }
}

