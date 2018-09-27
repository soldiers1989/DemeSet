using System;
using System.Collections.Generic;
using System.Text;

namespace ZF.WebSite.Models.Entity
{
    [Serializable]
    public class WeiXinGroup
    {
        public WeiXinGroupData group { get; set; }
       
    }

    [Serializable]
    public class WeiXinGroupData
    {
         public int id { get; set; }

        public string name { get; set; }
    }
}
