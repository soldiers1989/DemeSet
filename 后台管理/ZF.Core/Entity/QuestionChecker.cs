using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Core.Entity
{
   public class QuestionChecker:BaseEntity<Guid>
    {
        public string Content { get; set; }

        public DateTime? AddTime { get; set; }

        public string UserId { get; set; }
    }
}
