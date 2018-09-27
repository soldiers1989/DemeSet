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
    /// 标签管理中间表
    /// </summary>
    [AutoMap( typeof( Base_TagMiddle ) )]
    public class BaseTagMiddleInput
    {

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
