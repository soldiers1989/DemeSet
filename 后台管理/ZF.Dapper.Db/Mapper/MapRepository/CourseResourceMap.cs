using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：CourseResource 
    /// </summary>
    public sealed class CourseResourceMap : FullAuditEntityClassMapper<CourseResource, Guid>
    {
		public CourseResourceMap ()
		{
			Table("t_Course_Resource");
				
			Map(x => x.ResourceName).Column("ResourceName");
			Map(x => x.ResourceUrl).Column("ResourceUrl");
			Map(x => x.ResourceSize).Column("ResourceSize");
			Map(x => x.CourseId).Column("CourseId");
			
			
			this.AutoMap();
		}
    }
}

