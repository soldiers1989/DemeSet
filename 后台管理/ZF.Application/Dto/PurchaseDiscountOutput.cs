using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Application.Dto
{
   public class PurchaseDiscountOutput
    {
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float TopNum { get; set; }

        public float MinusNum { get; set; }

        /// <summary>
        /// 有效期开始时间
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 有效期结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 可用于购买课程
        /// </summary>
        public string TargetCourse { get; set; }
    }
}
