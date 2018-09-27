namespace ZF.Application.WebApiDto.CourseModule
{
    public class ProjectSubjectOutput
    {
        /// <summary>
        /// 课程编号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }


        /// <summary>
        /// 科目编号
        /// </summary>
        public string SubjectId { get; set; }


        /// <summary>
        /// 科目名称
        /// </summary>
        public string SubjectName { get; set; }


        /// <summary>
        /// 项目编号
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// 项目分类
        /// </summary>
        public string ProjectName { get; set; }


        /// <summary>
        /// 项目分类编号
        /// </summary>
        public string ProjectClassId { get; set; }

        /// <summary>
        /// 项目分类名称
        /// </summary>
        public string ProjectClassName { get; set; }
    }
}