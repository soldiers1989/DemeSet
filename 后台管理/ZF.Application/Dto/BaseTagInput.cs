using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 标签管理
    /// </summary>
    [AutoMap( typeof( Base_Tag ) )]
    public class BaseTagInput
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
