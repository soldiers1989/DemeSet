using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：CourseChapter 
    /// </summary>
    public sealed class CourseChapterMap : BaseClassMapper<CourseChapter, Guid>
    {
		public CourseChapterMap ()
		{
			Table("t_Course_Chapter");
				
			Map(x => x.CapterName).Column("CapterName");
			Map(x => x.ParentId).Column("ParentId");
			Map(x => x.CapterCode).Column("CapterCode");
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

