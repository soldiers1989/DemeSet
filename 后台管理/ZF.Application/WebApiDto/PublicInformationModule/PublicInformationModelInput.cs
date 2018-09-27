using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Application.WebApiDto.PublicInformationModule
{
    /// <summary>
    /// 
    /// </summary>
   public class PublicInformationModelInput
    {
        /// <summary>
        /// 编码
        /// </summary>     
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>     
        public string Name { get; set; }
        /// <summary>
        /// 内容
        /// </summary>     
        public string Content { get; set; }
    }
}
