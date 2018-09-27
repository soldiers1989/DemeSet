using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：PaperParam 
    /// </summary>
    public sealed class PaperParamMap : FullAuditEntityClassMapper<PaperParam, Guid>
    {
		public PaperParamMap ()
		{
			Table("t_Paper_Param");
				
			Map(x => x.PaperStructureDetailId).Column("PaperStructureDetailId");
			Map(x => x.KnowledgePointId).Column("KnowledgePointId");
			Map(x => x.QuestionCount).Column("QuestionCount");
			Map(x => x.DifficultLevel).Column("DifficultLevel");
			Map(x => x.QuestionScoreSum).Column("QuestionScoreSum");
			
			
			this.AutoMap();
		}
    }
}

