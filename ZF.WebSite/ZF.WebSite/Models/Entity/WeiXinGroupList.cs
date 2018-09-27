using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZF.WebSite.Models.Entity
{
    [Serializable]
    public class WeiXinGroupList
    {
        /// <summary>
        /// 列表数据
        /// </summary>
        public WeiXinGroupListData[] groups { get; set; } 
    }

    [Serializable]
    public class WeiXinGroupListData
    {
        public int id { get; set; }

        public string name { get; set; }

        //组内成员数量
        public int count { get; set; }
    }
}
