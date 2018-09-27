using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：快递公司 
    /// </summary>
    public class ExpressCompany : BaseEntity<Guid>
    {
        /// <summary>
        /// 快递公司名称
        /// </summary>     
        public string Name { get; set; }

        /// <summary>
        /// 快递公司网址
        /// </summary>     
        public string Companyurl { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>     
        public DateTime? AddTime { get; set; }

        /// <summary>
        /// 添加人
        /// </summary>     
        public string AddUser { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>     
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>     
        public string UpdateUser { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>     
        public int? IsDelete { get; set; }

        /// <summary>
        /// 是否默认快递公司
        /// </summary>
        public int IsDefault { get; set; }

    }
}

