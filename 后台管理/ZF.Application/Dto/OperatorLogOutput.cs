using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 操作者
    /// </summary>
    [AutoMap(typeof(OperatorLog))]
    public class OperatorLogOutput
    {
        /// <summary>
        /// 主键Id
        /// </summary>     
        public string Id { get; set; }

        /// <summary>
        /// 模块Id
        /// </summary>     
        public int ModuleId { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>     
        public string ModuleName { get; set; }

        /// <summary>
        /// 标识Id
        /// </summary>     
        public string KeyId { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>     
        public string OperatorName { get; set; }

        /// <summary>
        /// 操作人Id
        /// </summary>     
        public string OperatorId { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>     
        public DateTime OperatorDate { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>     
        public OperatorType OperatorType { get; set; }

        /// <summary>
        /// 操作类型名称
        /// </summary>     
        public string OperatorTypeName { get; set; }


        /// <summary>
        /// 操作者Id
        /// </summary>     
        public string OperatorIp { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>     
        public string Remark { get; set; }
    }
}