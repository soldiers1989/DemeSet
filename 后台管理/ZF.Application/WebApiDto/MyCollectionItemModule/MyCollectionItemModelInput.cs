using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.WebApiDto.MyCollectionItemModule
{
    /// <summary>
    /// 
    /// </summary>
    [AutoMap( typeof( MyCollectionItem ) )]
    public class MyCollectionItemModelInput
    {
        /// <summary>
        /// 编码
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 用户编码
        /// </summary>     
        public string UserId { get; set; }
        /// <summary>
        /// 试题编码
        /// </summary>     
        public string QuestionId { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>     
        public DateTime? AddTime { get; set; }
    }
}
