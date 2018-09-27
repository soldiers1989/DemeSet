using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Application.Dto
{
   public class UserDiscountCardOutput
    {
        public string Id { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 抵用券Id
        /// </summary>
        public string CardId { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set;}

        /// <summary>
        /// 有效开始时间
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// 有效结束时间
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 领用时间
        /// </summary>
        public DateTime? AddTime { get; set; }
        /// <summary>
        /// 是否使用
        /// </summary>
        public int IfUse { get; set; }

        /// <summary>
        /// 使用时间
        /// </summary>
        public DateTime UseTime { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string TelphoneNum { get; set; }
    }
}
