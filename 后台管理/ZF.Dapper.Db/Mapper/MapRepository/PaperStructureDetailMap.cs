using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：PaperStructureDetail 
    /// </summary>
    public sealed class PaperStructureDetailMap : BaseClassMapper<PaperStructureDetail, Guid>
    {
		public PaperStructureDetailMap ()
		{
			Table("t_Paper_StructureDetail");
				
			Map(x => x.StuctureId).Column("StuctureId");
			Map(x => x.QuesitonTypeName).Column("QuesitonTypeName");
			Map(x => x.QuestionType).Column("QuestionType");
			Map(x => x.QuestionClass).Column("QuestionClass");
			Map(x => x.DifficultLevel).Column("DifficultLevel");
			Map(x => x.QuestionCount).Column("QuestionCount");
			Map(x => x.QuestionTypeScoreSum).Column("QuestionTypeScoreSum");
			Map(x => x.OrderNo).Column("OrderNo");
			Map(x => x.AddTime).Column("AddTime");
			Map(x => x.AddUserId).Column("AddUserId");
			Map(x => x.UpdateTime).Column("UpdateTime");
			Map(x => x.UpdateUserId).Column("UpdateUserId");
			Map(x => x.IsDelete).Column("IsDelete");
			
			this.AutoMap();
		}
    }
}

