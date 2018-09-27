using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：CourseSubject 
    /// </summary>
    public sealed class CourseSubjectMap : BaseClassMapper<CourseSubject, Guid>
    {
		public CourseSubjectMap ()
		{
			Table("t_Course_Subject");
				
			Map(x => x.CourseId).Column("CourseId");
			Map(x => x.ChapterId).Column("ChapterId");
			Map(x => x.SubjectId).Column("SubjectId");
			
			this.AutoMap();
		}
    }
}

