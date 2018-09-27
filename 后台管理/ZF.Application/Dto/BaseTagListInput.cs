using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseTagListInput: BasePageInput
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
        // <summary>
        /// 模块编码
        /// </summary>
        public string ModelCode { get; set; }

        /// <summary>
        /// 标签名称
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
