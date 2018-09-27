using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输出Output:试卷组试卷关系表 
    /// </summary>
    public class PaperGroupRelationOutput
    {
        /// <summary>
        /// 编码
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 试卷组编号
        /// </summary>     
        public string PaperGroupId { get; set; }
        /// <summary>
        /// 试卷编号
        /// </summary>     
        public string PaperId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PaperName { get; set; }

        /// <summary>
        /// /
        /// </summary>
        public string TestTime { get; set; }
    }
}

