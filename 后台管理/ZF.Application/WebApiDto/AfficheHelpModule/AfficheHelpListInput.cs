using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;

namespace ZF.Application.WebApiDto.AfficheHelpModule
{
   public class AfficheHelpListInput:BasePageInput
    {
        /// <summary>
        /// 类别  0资讯  1帮助
        /// </summary>     
        public int? Type { get; set; }
        /// <summary>
        /// 标题
        /// </summary>     
        public string Title { get; set; }
        /// <summary>
        /// 大类编号
        /// </summary>     
        public string BigClassId { get; set; }
        /// <summary>
        /// 小类编号
        /// </summary>     
        public string ClassId { get; set; }
        /// <summary>
        /// 是否置顶
        /// </summary>     
        public int? IsTop { get; set; }
        /// <summary>
        /// 是否显示在首页
        /// </summary>     
        public int? IsIndex { get; set; }
    }
}
