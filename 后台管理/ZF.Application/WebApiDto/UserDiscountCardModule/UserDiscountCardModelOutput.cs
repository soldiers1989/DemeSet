using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Application.WebApiDto.UserDiscountCardModule
{
    /// <summary>
    /// 
    /// </summary>
    public class UserDiscountCardModelOutput
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
        /// <summary>
        /// 
        /// </summary>
        public string TargetCourse { get; set; }

        /// <summary>
        /// 使用状态
        /// </summary>
        public int IfUse { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 学习卡单价
        /// </summary>
        public int dj { get; set; }
    }
}
