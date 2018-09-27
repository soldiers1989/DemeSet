using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：MyCollection 
    /// </summary>
    public sealed class MyCollectionMap : BaseClassMapper<MyCollection, Guid>
    {
		public MyCollectionMap ()
		{
			Table("t_My_Collection");
				
			Map(x => x.CourseId).Column("CourseId");
			Map(x => x.UserId).Column("UserId");
			Map(x => x.AddTime).Column("AddTime");
			
			this.AutoMap();
		}
    }
}

