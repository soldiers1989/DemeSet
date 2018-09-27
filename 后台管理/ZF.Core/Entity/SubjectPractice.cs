using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Core.Entity
{
   public class SubjectPractice:BaseEntity<Guid>
    {
        /// <summary>
        /// 用户编码
        /// </summary>     
        public string UserId { get; set; }
      
        /// <summary>
        /// 时间
        /// </summary>     
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 练习类型 0 收藏试题 1 错题
        /// </summary>
        public int Type { get; set; }
    }
}
