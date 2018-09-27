using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Application.WebApiDto.OrderCardModule
{
   public class OrderCardOutput
    {
        public string Id { get; set; }
        public string OrderNo { get; set; }

        public string CardCode { get; set; }

        /// <summary>
        /// 卡号编号
        /// </summary>
        public string CardId { get; set; }
    }
}
