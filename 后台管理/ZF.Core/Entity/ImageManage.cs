using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Core.Entity
{
   public class ImageManage:BaseEntity<Guid>
    {
        public string ImageName { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public string ImageClass { get; set; }

        public string ImageUrl { get; set; }
    }


    public class ImageClassManage : BaseEntity<Guid> {
        public string ImageClassName { get; set; }
    }
}
