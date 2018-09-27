using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：Subject 
    /// </summary>
    [AutoMap(typeof(Subject))]
    public class SubjectInput
    {
        /// <summary>
        /// 科目编码
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 科目名称
        /// </summary>     
        public string SubjectName { get; set; }
        /// <summary>
        /// 所属项目
        /// </summary>     
        public string ProjectId { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>     
        public int OrderNo { get; set; }
        /// <summary>
        /// 科目说明
        /// </summary>     
        public string Remark { get; set; }

        /// <summary>
        /// 是否经济基础
        /// </summary>
        public int IsEconomicBase { get; set; }
    }
}

