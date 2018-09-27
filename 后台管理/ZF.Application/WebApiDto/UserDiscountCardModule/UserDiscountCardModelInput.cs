using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.WebApiDto.UserDiscountCardModule
{
    /// <summary>
    /// 
    /// </summary>
    [AutoMap( typeof( User_Discount_Card ) )]
    public class UserDiscountCardModelInput
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 学习卡Id
        /// </summary>
        public string CardId { get; set; }
        /// <summary>
        /// 领用时间
        /// </summary>
        public DateTime? AddTime { get; set; }
        /// <summary>
        /// 是否使用
        /// </summary>
        public int IfUse { get; set; }

        /// <summary>
        /// 是否过期
        /// </summary>
        public int IfExpair { get; set; }

        public int Type { get; set; }

    }
}
