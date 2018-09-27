using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：CourseChapter 
    /// </summary>
    [AutoMap(typeof(CourseChapter))]
    public class CourseChapterInput
    {
        /// <summary>
        /// 编码
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 章节名称
        /// </summary>     
        public string CapterName { get; set; }
        /// <summary>
        /// 父节点编码
        /// </summary>     
        public string ParentId { get; set; }
        /// <summary>
        /// 章节代码
        /// </summary>     
        public string CapterCode { get; set; }
        /// <summary>
        /// 所属课程
        /// </summary>     
        public string CourseId { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int OrderNo { get; set; }
    }
}

