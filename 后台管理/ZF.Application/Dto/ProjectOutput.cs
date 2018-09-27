using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 数据表Project 输出Dto
    /// </summary>
    public class ProjectOutput
    {
        /// <summary>
        /// 项目编码
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>     
        public string ProjectName { get; set; }
        /// <summary>
        /// 所属项目分类
        /// </summary>     
        public string ProjectClassName { get; set; }
        /// <summary>
        /// 项目说明
        /// </summary>     
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>     
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int OrderNo { get; set; }
    }
}

