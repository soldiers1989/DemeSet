using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Application.WebApiDto.AfficheHelpModule
{
    public class AfficheHelpOutput
    {
        /// <summary>
        /// 主键编号
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 类别  0资讯  1帮助
        /// </summary>     
        public int Type { get; set; }
        /// <summary>
        /// 标题
        /// </summary>     
        public string Title { get; set; }
        /// <summary>
        /// 资讯内容
        /// </summary>     
        public string Content { get; set; }
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
        public int IsTop { get; set; }
        /// <summary>
        /// 是否显示在首页
        /// </summary>     
        public int IsIndex { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>     
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 大类名称
        /// </summary>
        public string DataTypeName { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        public string AfficheIamge { get; set; }
    }
}
