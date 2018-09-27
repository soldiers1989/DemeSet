using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
   public class ArticleListInput:BasePageInput
    {
        public string Id { get; set; }
        public string ArticleTitle { get; set; }

        public string ArticleContent { get; set; }

        public string ArticleImage { get; set; }

        public DateTime AddTime { get; set; }

        public int Type { get; set; }

        public int IsDelete { get; set; }
    }
}
