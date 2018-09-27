using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：PaperPaperParam 
    /// </summary>
    public sealed class PaperPaperParamMap : FullAuditEntityClassMapper<PaperPaperParam, Guid>
    {
		public PaperPaperParamMap ()
		{
			Table("t_Paper_PaperParam");
				
			Map(x => x.StuctureId).Column("StuctureId");
			Map(x => x.ParamName).Column("ParamName");
            Map(x => x.State).Column("State");

			this.AutoMap();
		}
    }
}

