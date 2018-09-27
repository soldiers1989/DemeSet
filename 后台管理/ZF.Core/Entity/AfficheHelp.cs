using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：资讯,帮助管理表 
    /// </summary>
    public class AfficheHelp:FullAuditEntity<Guid>
    {
       /// <summary>
       /// 类别  0资讯  1帮助
       /// </summary>     
       public int Type{ get; set; }

       /// <summary>
       /// 标题
       /// </summary>     
       public string Title{ get; set; }

       /// <summary>
       /// 资讯内容
       /// </summary>     
       public string Content{ get; set; }

       /// <summary>
       /// 大类编号
       /// </summary>     
       public string BigClassId{ get; set; }

       /// <summary>
       /// 小类编号
       /// </summary>     
       public string ClassId{ get; set; }

       /// <summary>
       /// 是否置顶
       /// </summary>     
       public int IsTop{ get; set; }

       /// <summary>
       /// 是否显示在首页
       /// </summary>     
       public int IsIndex{ get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        public string AfficheIamge { get; set; }

    }
}

