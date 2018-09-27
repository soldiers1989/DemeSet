using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：我的足迹 
    /// </summary>
    public sealed class MyFootprintMap : BaseClassMapper<MyFootprint, Guid>
    {
		public MyFootprintMap ()
		{
			Table("t_My_Footprint");
				
			Map(x => x.CourseId).Column("CourseId");
			Map(x => x.CourseType).Column("CourseType");
			Map(x => x.BrowsingTime).Column("BrowsingTime");
			Map(x => x.UserId).Column("UserId");
			Map(x => x.IsDelete).Column("IsDelete");
			
			this.AutoMap();
		}
    }
}

