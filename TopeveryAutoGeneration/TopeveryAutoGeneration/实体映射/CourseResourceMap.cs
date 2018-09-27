using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：CourseResource 
    /// </summary>
    public sealed class CourseResourceMap : BaseClassMapper<CourseResource, Guid>
    {
		public CourseResourceMap ()
		{
			Table("t_Course_Resource");
				
			Map(x => x.ResourceName).Column("ResourceName");
			Map(x => x.ResourceUrl).Column("ResourceUrl");
			Map(x => x.ResourceSize).Column("ResourceSize");
			Map(x => x.CourseId).Column("CourseId");
			Map(x => x.AddTime).Column("AddTime");
			Map(x => x.AddUserId).Column("AddUserId");
			Map(x => x.UpdateTime).Column("UpdateTime");
			Map(x => x.UpdateUserId).Column("UpdateUserId");
			Map(x => x.IsDelete).Column("IsDelete");
			
			this.AutoMap();
		}
    }
}

