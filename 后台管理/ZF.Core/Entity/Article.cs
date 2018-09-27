using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Core.Entity
{
   public class Article:BaseEntity<Guid>
    {

        [Required]
        [MaxLength(100,ErrorMessage ="超过指定长度")]
        public string ArticleTitle { get; set; }
        [Required]
        public string ArticleContent { get; set; }

        public string ArticleImage { get; set; }

        public DateTime AddTime { get; set; }

        public int Type { get; set; }

        public int IsDelete { get; set; }
    }
}
