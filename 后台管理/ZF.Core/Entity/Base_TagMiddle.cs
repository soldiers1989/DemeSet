using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 标签管理中间表
    /// </summary>
   public class Base_TagMiddle:BaseEntity<Guid>
    {
        /// <summary>
        /// 实体编码
        /// </summary>
        public string ModelId { get; set;}

        /// <summary>
        /// 标签编码
        /// </summary>
        public string TagId { get; set; }
    }
}
