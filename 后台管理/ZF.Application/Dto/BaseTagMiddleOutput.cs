using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 
    /// </summary>
   public class BaseTagMiddleOutput
    {

        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 实体编码
        /// </summary>
        public string ModelId { get; set; }

        /// <summary>
        /// 标签编码
        /// </summary>
        public string TagId { get; set; }
    }
}
