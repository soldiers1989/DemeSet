using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：视频观看历史记录 
    /// </summary>
    public class MyVideoWatch:BaseEntity<Guid>
    {
       /// <summary>
       /// 视频名称
       /// </summary>     
       public string VideoId{ get; set; }

       /// <summary>
       /// 观看时间
       /// </summary>     
       public DateTime WatchTime{ get; set; }

       /// <summary>
       /// 用户编号
       /// </summary>     
       public string UserId{ get; set; }

       /// <summary>
       /// 是否删除
       /// </summary>     
       public int IsDelete{ get; set; }

    }
}

