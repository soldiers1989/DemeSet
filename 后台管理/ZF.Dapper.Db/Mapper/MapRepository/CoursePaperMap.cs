using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：课程试卷表 
    /// </summary>
    public sealed class CoursePaperMap : BaseClassMapper<CoursePaper, Guid>
    {
		public CoursePaperMap ()
		{
			Table("t_Course_Paper");
				
			Map(x => x.CourseId).Column("CourseId");
			Map(x => x.PaperInfoId).Column("PaperInfoId");
			Map(x => x.PaperInfoName).Column("PaperInfoName");
            Map(x => x.OrderNo).Column("OrderNo");

            this.AutoMap();
		}
    }
}

