using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：SubjectClass 
    /// </summary>
    [AutoMap(typeof(SubjectClass))]
    public class SubjectClassInput
    {
        /// <summary>
        /// 分类编码
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>     
        public string ClassName { get; set; }
        /// <summary>
        /// 分类所属项目
        /// </summary>     
        public string ProjectId { get; set; }
        /// <summary>
        /// 分类描述
        /// </summary>     
        public string Remark { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>     
        public int? OrderNo { get; set; }
        /// <summary>
        /// 评分规则？
        /// </summary>     
        public string Column_6 { get; set; }


        /// <summary>
        /// 试题表现形式(大题)
        /// </summary>     
        public int? BigType { get; set; }

    }
}

