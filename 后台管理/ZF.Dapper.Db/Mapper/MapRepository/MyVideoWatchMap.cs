using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：视频观看历史记录 
    /// </summary>
    public sealed class MyVideoWatchMap : BaseClassMapper<MyVideoWatch, Guid>
    {
		public MyVideoWatchMap ()
		{
			Table("t_My_VideoWatch");
				
			Map(x => x.VideoId).Column("VideoId");
			Map(x => x.WatchTime).Column("WatchTime");
			Map(x => x.UserId).Column("UserId");
			Map(x => x.IsDelete).Column("IsDelete");
			
			this.AutoMap();
		}
    }
}

