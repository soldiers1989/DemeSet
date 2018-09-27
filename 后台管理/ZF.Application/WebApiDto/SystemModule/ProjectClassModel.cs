using System;
using ZF.Core;

namespace ZF.Application.WebApiDto.SystemModule
{
    /// <summary>
    /// 项目分类模型
    /// </summary>
    public class ProjectClassModel : BaseEntity<Guid>
    {
        /// <summary>
        /// 项目分类名称
        /// </summary>     
        public string ProjectClassName { get; set; }

        /// <summary>
        /// 项目说明
        /// </summary>     
        public string Remark { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>     
        public int OrderNo { get; set; }
    }
}
