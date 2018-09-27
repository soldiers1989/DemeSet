using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Core.Entity
{
    public class User_Discount_Card:BaseEntity<Guid>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 抵用券Id
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
        /// 卡券类型 0：学习卡 1：满减优惠券
        /// </summary>
        public int Type { get; set; }
    }
}
