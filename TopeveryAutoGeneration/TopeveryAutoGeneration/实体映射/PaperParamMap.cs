using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：PaperParam 
    /// </summary>
    public sealed class PaperParamMap : BaseClassMapper<PaperParam, Guid>
    {
		public PaperParamMap ()
		{
			Table("t_Paper_Param");
				
			Map(x => x.PaperStructureDetailId).Column("PaperStructureDetailId");
			Map(x => x.KnowledgePointId).Column("KnowledgePointId");
			Map(x => x.QuestionCount).Column("QuestionCount");
			Map(x => x.DifficultLevel).Column("DifficultLevel");
			Map(x => x.QuestionScoreSum).Column("QuestionScoreSum");
			Map(x => x.AddTime).Column("AddTime");
			Map(x => x.AddUserId).Column("AddUserId");
			Map(x => x.UpdateTime).Column("UpdateTime");
			Map(x => x.UpdateUserId).Column("UpdateUserId");
			Map(x => x.IsDelete).Column("IsDelete");
			
			this.AutoMap();
		}
    }
}

