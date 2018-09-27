using System;
using ZF.Application.BaseDto;
using ZF.Infrastructure;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 操作人日志
    /// </summary>

    public class GetListOperatorLogInput : BasePageInput
    {
        /// <summary>
        /// 操作内容
        /// </summary>
        public string Content { set; get; }

        /// <summary>
        /// 模块类型
        /// </summary>
        public int? ModuleType { set; get; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginDateTime { set; get; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDateTime { set; get; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public OperatorType? OperatorType { set; get; }

        /// <summary>
        /// 操作人名称
        /// </summary>
        public string OperatorName { set; get; }

    }
}