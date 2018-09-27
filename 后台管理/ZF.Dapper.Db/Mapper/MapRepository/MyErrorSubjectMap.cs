using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：MyErrorSubject 
    /// </summary>
    public sealed class MyErrorSubjectMap : BaseClassMapper<MyErrorSubject, Guid>
    {
		public MyErrorSubjectMap ()
		{
			Table("t_My_ErrorSubject");
				
			Map(x => x.UserId).Column("UserId");
			Map(x => x.BigQuestionId).Column("BigQuestionId");
            Map(x => x.SmallQuestionId).Column("SmallQuestionId");
            Map(x => x.AddTime).Column("AddTime");
            Map( x => x.StuAnswer ).Column( "StuAnswer" );

            this.AutoMap();
		}
    }
}

