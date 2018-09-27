using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：CourseSuitDetail 
    /// </summary>
    public sealed class CourseSuitDetailMap : BaseClassMapper<CourseSuitDetail, Guid>
    {
		public CourseSuitDetailMap ()
		{
			Table("t_Course_SuitDetail");
				
			Map(x => x.PackCourseId).Column("PackCourseId");
			Map(x => x.CourseId).Column("CourseId");
            Map( x => x.OrderNo ).Column( "OrderNo");
			this.AutoMap();
		}
    }
}

