using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：CourseChapter 
    /// </summary>
    public sealed class CourseChapterMap : FullAuditEntityClassMapper<CourseChapter, Guid>
    {
		public CourseChapterMap ()
		{
			Table("t_Course_Chapter");
				
			Map(x => x.CapterName).Column("CapterName");
			Map(x => x.ParentId).Column("ParentId");
			Map(x => x.CapterCode).Column("CapterCode");
			Map(x => x.CourseId).Column("CourseId");
            Map(x => x.OrderNo).Column("OrderNo");


            this.AutoMap();
		}
    }
}

