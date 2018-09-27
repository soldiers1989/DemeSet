using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：Basedata 
    /// </summary>
    public class Basedata:FullAuditEntity<Guid>
    {
       /// <summary>
       /// 数据名称
       /// </summary>     
       public string Name{ get; set; }

       /// <summary>
       /// 数据类型代码
       /// </summary>     
       public string Code{ get; set; }

        /// <summary>
        /// 命名空间编号
        /// </summary>
       public string DataTypeId { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int Sort { get; set; }

    }
}

