using System;
using System.Collections.Generic;
using ZF.Application.WebApiDto.SystemModule;
using ZF.Core;

namespace ZF.Application.WebApiDto.CourseModule
{
    /// <summary>
    /// 首页科目 课程返回
    /// </summary>
    public class ProjectCourseModel : BaseEntity<Guid>
    {
        /// <summary>
        /// 科目集合
        /// </summary>
        public List<SubjectModel> SubjectModel { get; set; } = new List<SubjectModel>();


        /// <summary>
        /// 课程集合
        /// </summary>
        public List<CourseInfoModel> CourseInfoModel { get; set; } = new List<CourseInfoModel>();
    }
    /// <summary>
    /// 项目分类集合
    /// </summary>
    public class ProjectClassModel1 : BaseEntity<Guid>
    {

        /// <summary>
        /// 项目集合
        /// </summary>
        public List<ProjectModel> ProjectMode { get; set; } = new List<ProjectModel>();
    }

    /// <summary>
    /// 项目模型
    /// </summary>
    public class ProjectModel : BaseEntity<Guid>
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

        /// <summary>
        /// 科目集合
        /// </summary>
        public List<SubjectModel> SubjectModel { get; set; } = new List<SubjectModel>();
    }
}