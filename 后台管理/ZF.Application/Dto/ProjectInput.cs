using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：Project 
    /// </summary>
    [AutoMap(typeof(Project))]
    public class ProjectInput
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
        public string ProjectClassId { get; set; }
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

