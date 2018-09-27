using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：下单机构表 
    /// </summary>
    public class OrderInstitutions : FullAuditEntity<Guid>
    {
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
        /// 机构二维码地址
        /// </summary>     
        public string Url { get; set; }

    }
}

