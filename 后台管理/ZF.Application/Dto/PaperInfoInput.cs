using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace Topevery.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：PaperInfo 
    /// </summary>
    [AutoMap(typeof(PaperInfo))]
    public class PaperInfoInput
    {
        /// <summary>
        /// 编码
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 试卷名称
        /// </summary>     
        public string PaperName { get; set; }
        /// <summary>
        /// 试卷参数编码
        /// </summary>     
        public string PaperParamId { get; set; }
        /// <summary>
        /// 所属科目编码
        /// </summary>     
        public string SubjectId { get; set; }
        /// <summary>
        /// 考试时长
        /// </summary>     
        public int? TestTime { get; set; }
        /// <summary>
        /// 试卷状态
        /// </summary>     
        public int? State { get; set; }
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
        /// 试卷数量
        /// </summary>
        public int PaperInfoCount { get; set; }

        public int Type { get; set; }
    }
}

