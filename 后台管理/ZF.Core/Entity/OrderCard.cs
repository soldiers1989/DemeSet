using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Core.Entity
{
    public class OrderCard : BaseEntity<Guid>
    {
        public string OrderNo { get; set; }

        public string CardCode { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 卡号编号
        /// </summary>
        public string CardId { get; set; }
    }
}
