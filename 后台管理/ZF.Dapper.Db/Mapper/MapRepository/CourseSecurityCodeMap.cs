using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：课程防伪码管理 
    /// </summary>
    public sealed class CourseSecurityCodeMap : BaseClassMapper<CourseSecurityCode, Guid>
    {
		public CourseSecurityCodeMap ()
		{
			Table("t_Course_SecurityCode");
				
			Map(x => x.CourseId).Column("CourseId");
			Map(x => x.Code).Column("Code");
            Map(x => x.IsValueAdded).Column("IsValueAdded");
            Map(x => x.IsUse).Column("IsUse");
			Map(x => x.UserId).Column("UserId");
            Map(x => x.GetDateTime).Column("GetDateTime");

            this.AutoMap();
		}
    }
}

