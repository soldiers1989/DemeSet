using System;
using ZF.Application.WebApiDto.CourseModule;
using ZF.AutoMapper.AutoMapper;
using ZF.Core;
using ZF.Core.Entity;

namespace ZF.Application.WebApiDto.SystemModule
{
    /// <summary>
    /// 项目模型
    /// </summary>

    [AutoMap(typeof(ProjectModel))]
    public class ProjectMode : BaseEntity<Guid>
    {
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
    }
}
