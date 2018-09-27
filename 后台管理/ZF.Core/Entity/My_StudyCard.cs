using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Core.Entity
{
   public class My_StudyCard : BaseEntity<Guid>
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public float Amount { get; set;}

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? AddTime { get; set; }
    }
}
