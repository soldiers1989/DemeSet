using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输出Output:试卷组表 
    /// </summary>
    public class PaperGroupOutput
    {
        /// <summary>
        /// 编码
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 试卷名称
        /// </summary>     
        public string PaperGroupName { get; set; }
        /// <summary>
        /// 所属科目编码
        /// </summary>     
        public string SubjectName { get; set; }
        /// <summary>
        /// 试卷状态
        /// </summary>     
        public string StateName { get; set; }

        /// <summary>
        /// 试卷状态
        /// </summary>     
        public int State { get; set; }

        /// <summary>
        /// 试卷属性  0历年真题 1模拟试卷
        /// </summary>     
        public string TypeName { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public DateTime? AddTime { get; set; }

    }
}

