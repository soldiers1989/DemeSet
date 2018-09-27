using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 数据表Subject 输出Dto
    /// </summary>
    public class SubjectOutput
    {
        /// <summary>
        /// 科目编码
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 科目名称
        /// </summary>     
        public string SubjectName { get; set; }
        /// <summary>
        /// 所属项目
        /// </summary>     
        public string ProjectId { get; set; }
        /// <summary>
        /// 所属项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>     
        public int OrderNo { get; set; }

        /// <summary>
        /// 项目分类名称
        /// </summary>
        public string ProjectClassName { get; set; }

        /// <summary>
        /// 是否经济基础
        /// </summary>
        public int IsEconomicBase { get; set; }
    }
}

