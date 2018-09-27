using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：CourseAppraise 
    /// </summary>
    public sealed class CourseAppraiseMap : BaseClassMapper<CourseAppraise, Guid>
    {
		public CourseAppraiseMap ()
		{
			Table("t_Course_Appraise");
				
			Map(x => x.AppraiseCotent).Column("AppraiseCotent");
			Map(x => x.UserId).Column("UserId");
			Map(x => x.AppraiseLevel).Column("AppraiseLevel");
			Map(x => x.AppraiseTime).Column("AppraiseTime");
			Map(x => x.AppraiseIp).Column("AppraiseIp");
			Map(x => x.ReplyTime).Column("ReplyTime");
			Map(x => x.ReplyContent).Column("ReplyContent");
			Map(x => x.ReplyAdminUserId).Column("ReplyAdminUserId");
			Map(x => x.CourseId).Column("CourseId");
			Map(x => x.CourseType).Column("CourseType");
			
			this.AutoMap();
		}
    }
}

