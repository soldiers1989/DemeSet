using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输出Output:下单机构表 
    /// </summary>
    public class OrderInstitutionsOutput
    {
        /// <summary>
        /// 编码
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>     
        public string Name { get; set; }
        /// <summary>
        /// 机构联系人
        /// </summary>     
        public string Contact { get; set; }
        /// <summary>
        /// 机构联系方式
        /// </summary>     
        public string ContactPhone { get; set; }
        /// <summary>
        /// 机构联系地址
        /// </summary>     
        public string ContactAddress { get; set; }
        /// <summary>
        /// 机构备注
        /// </summary>     
        public string Remark { get; set; }

        /// <summary>
        /// url
        /// </summary>     
        public string Url { get; set; }

    }
}

