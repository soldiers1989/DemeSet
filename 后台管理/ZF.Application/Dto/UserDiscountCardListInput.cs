using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    public class UserDiscountCardListInput : BasePageInput
    {
        public string Id { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 课程Id
        /// </summary>
        public string CourseId { get; set; }
        /// <summary>
        /// 领用时间
        /// </summary>
        public DateTime? AddTime { get; set; }
        /// <summary>
        /// 是否使用
        /// </summary>
        public int IfUse { get; set; }

        /// <summary>
        /// 使用开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 使用结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 学习卡卡号
        /// </summary>
        public object CardId { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string TelphoneNum { get; set; }

        /// <summary>
        /// 卡券类型
        /// </summary>
        public int Type { get; set; }
    }
}
