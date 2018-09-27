using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：CourseOnTeachers 
    /// </summary>
    public sealed class CourseOnTeachersMap : BaseClassMapper<CourseOnTeachers, Guid>
    {
		public CourseOnTeachersMap ()
		{
			Table("t_Course_OnTeachers");
				
			Map(x => x.TeachersName).Column("TeachersName");
			Map(x => x.TheLabel).Column("TheLabel");
			Map(x => x.Synopsis).Column("Synopsis");
			Map(x => x.AddTime).Column("AddTime");
			Map(x => x.AddUserId).Column("AddUserId");
			Map(x => x.UpdateTime).Column("UpdateTime");
			Map(x => x.UpdateUserId).Column("UpdateUserId");
			Map(x => x.IsDelete).Column("IsDelete");
			
			this.AutoMap();
		}
    }
}

