using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输出Output:使用指南 
    /// </summary>
    public class UseDescriptionOutput
    {
        /// <summary>
        /// 主键编号
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 大类编号
        /// </summary>     
        public string BigClassName { get; set; }
        /// <summary>
        /// 小类名称
        /// </summary>     
        public string ClassName { get; set; }
        /// <summary>
        /// 内容
        /// </summary>     
        public string Content { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>     
        public DateTime AddTime { get; set; }

    }
}

