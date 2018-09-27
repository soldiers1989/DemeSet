using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：PaperDetatail 
    /// </summary>
    public sealed class PaperDetatailMap : BaseClassMapper<PaperDetatail, Guid>
    {
		public PaperDetatailMap ()
		{
			Table("t_Paper_Detatail");
				
			Map(x => x.PaperId).Column("PaperId");
			Map(x => x.QuestionId).Column("QuestionId");
			Map(x => x.QuestionTypeId).Column("QuestionTypeId");
			Map(x => x.QuestionScore).Column("QuestionScore");
			Map(x => x.AddTime).Column("AddTime");
			Map(x => x.AddUserId).Column("AddUserId");
			Map(x => x.UpdateTime).Column("UpdateTime");
			Map(x => x.UpdateUserId).Column("UpdateUserId");
			Map(x => x.IsDelete).Column("IsDelete");
			
			this.AutoMap();
		}
    }
}

