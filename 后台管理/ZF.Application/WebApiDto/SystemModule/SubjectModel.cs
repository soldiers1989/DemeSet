using System;
using ZF.Core;

namespace ZF.Application.WebApiDto.SystemModule
{
    /// <summary>
    /// 科目模型
    /// </summary>
    public class SubjectModel : BaseEntity<Guid>
    {
        /// <summary>
        /// 科目名称
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// 所属项目
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// 项目分类编号
        /// </summary>
        public string ProjectClassId { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 科目说明
        /// </summary>
        public string Remark { get; set; }
    }
}