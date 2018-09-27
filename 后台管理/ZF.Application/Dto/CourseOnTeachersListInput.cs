using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：CourseOnTeachers 
    /// </summary>
    public class CourseOnTeachersListInput : BasePageInput
    {
        /// <summary>
        /// 编码
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 教师名称
        /// </summary>     
        public string TeachersName { get; set; }
        /// <summary>
        /// 讲师照片
        /// </summary>
        public string TeacherPhoto { get; set; }
        /// <summary>
        /// 擅长科目
        /// </summary>     
        public string TheLabel { get; set; }
        /// <summary>
        /// 简介
        /// </summary>     
        public string Synopsis { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>     
        public DateTime AddTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>     
        public string AddUserId { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>     
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>     
        public string UpdateUserId { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>     
        public int IsDelete { get; set; }

        /// <summary>
        /// 所属项目
        /// </summary>
        public string ProjectId { get; set; }
        /// <summary>
        /// 是否在名师栏
        /// </summary>
        public int IsFamous { get; set; }
        /// <summary>
        /// 教学风格
        /// </summary>
        public string TeachStyle { get; set; }
    }
}
