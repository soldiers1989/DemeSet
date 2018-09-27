using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
   public class DiscountCardListInput:BasePageInput
    {

        public string Id { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardCode { get; set; }

        /// <summary>
        /// 学习卡名称
        /// </summary>
        public string CardName { get; set; }

        public string TargetCourse { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }
        /// <summary>
        /// 抵用券金额
        /// </summary>
        public float Amount { get; set; }

        /// <summary>
        /// 有效期开始时间
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 有效期结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
