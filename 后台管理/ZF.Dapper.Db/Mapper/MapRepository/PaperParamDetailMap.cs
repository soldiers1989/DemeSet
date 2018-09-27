using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：PaperParamDetail 
    /// </summary>
    public sealed class PaperParamDetailMap : BaseClassMapper<PaperParamDetail, Guid>
    {
		public PaperParamDetailMap ()
		{
			Table("t_Paper_ParamDetail");
				
			Map(x => x.PaperParamId).Column("PaperParamId");
			Map(x => x.PaperStuctureDetailId).Column("PaperStuctureDetailId");
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

