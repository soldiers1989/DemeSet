using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：意见反馈表 
    /// </summary>
    public sealed class MyFeedbackMap : BaseClassMapper<MyFeedback, Guid>
    {
		public MyFeedbackMap ()
		{
			Table("t_My_Feedback");
				
			Map(x => x.Title).Column("Title");
			Map(x => x.Advice).Column("Advice");
			Map(x => x.Relation).Column("Relation");
			Map(x => x.AddTime).Column("AddTime");
			
			this.AutoMap();
		}
    }
}

