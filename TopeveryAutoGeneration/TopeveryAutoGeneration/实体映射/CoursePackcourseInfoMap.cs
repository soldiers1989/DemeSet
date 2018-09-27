using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：CoursePackcourseInfo 
    /// </summary>
    public sealed class CoursePackcourseInfoMap : BaseClassMapper<CoursePackcourseInfo, Guid>
    {
		public CoursePackcourseInfoMap ()
		{
			Table("t_Course_PackcourseInfo");
				
			Map(x => x.CourseName).Column("CourseName");
			Map(x => x.CourseIamge).Column("CourseIamge");
			Map(x => x.CourseContent).Column("CourseContent");
			Map(x => x.Price).Column("Price");
			Map(x => x.FavourablePrice).Column("FavourablePrice");
			Map(x => x.ValidityPeriod).Column("ValidityPeriod");
			Map(x => x.State).Column("State");
			Map(x => x.IsTop).Column("IsTop");
			Map(x => x.IsRecommend).Column("IsRecommend");
			Map(x => x.CourseTag).Column("CourseTag");
			Map(x => x.CourseLongTime).Column("CourseLongTime");
			Map(x => x.CourseWareCount).Column("CourseWareCount");
			Map(x => x.AddTime).Column("AddTime");
			Map(x => x.AddUserId).Column("AddUserId");
			Map(x => x.UpdateTime).Column("UpdateTime");
			Map(x => x.UpdateUserId).Column("UpdateUserId");
			Map(x => x.IsDelete).Column("IsDelete");
			
			this.AutoMap();
		}
    }
}

