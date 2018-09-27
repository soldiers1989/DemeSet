using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：PaperPaperParam 
    /// </summary>
    public class PaperPaperParamListInput : BasePageInput
    {
        /// <summary>
        /// 编码
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 试卷结构编码
        /// </summary>     
        public string StuctureId { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>     
        public string ParamName { get; set; }
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
        /// 参数发布状态
        /// </summary>
        public int State { get; set; }
    }
}
