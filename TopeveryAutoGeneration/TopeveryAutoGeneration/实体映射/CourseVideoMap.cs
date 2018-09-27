using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：CourseVideo 
    /// </summary>
    public sealed class CourseVideoMap : BaseClassMapper<CourseVideo, Guid>
    {
		public CourseVideoMap ()
		{
			Table("t_Course_Video");
				
			Map(x => x.VideoName).Column("VideoName");
			Map(x => x.VideoUrl).Column("VideoUrl");
			Map(x => x.ChapterId).Column("ChapterId");
			Map(x => x.VideoLongTime).Column("VideoLongTime");
			Map(x => x.VideoContent).Column("VideoContent");
			Map(x => x.IsTaste).Column("IsTaste");
			Map(x => x.TasteLongTime).Column("TasteLongTime");
			Map(x => x.StudyCount).Column("StudyCount");
			Map(x => x.AddTime).Column("AddTime");
			Map(x => x.AddUserId).Column("AddUserId");
			Map(x => x.UpdateTime).Column("UpdateTime");
			Map(x => x.UpdateUserId).Column("UpdateUserId");
			Map(x => x.IsDelete).Column("IsDelete");
			
			this.AutoMap();
		}
    }
}

