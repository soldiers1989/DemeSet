using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：MyCourse 
    /// </summary>
    public sealed class MyCourseMap : BaseClassMapper<MyCourse, Guid>
    {
		public MyCourseMap ()
		{
			Table("t_My_Course");
				
			Map(x => x.UserId).Column("UserId");
			Map(x => x.CourseId).Column("CourseId");
			Map(x => x.AddTime).Column("AddTime");
			Map(x => x.BeginTime).Column("BeginTime");
			Map(x => x.EndTime).Column("EndTime");
            Map(x => x.CourseType).Column("CourseType");
            Map(x => x.OrderId).Column("OrderId");

            this.AutoMap();
		}
    }
}

