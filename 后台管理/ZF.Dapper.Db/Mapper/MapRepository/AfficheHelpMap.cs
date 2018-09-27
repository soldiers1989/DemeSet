using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：资讯,帮助管理表 
    /// </summary>
    public sealed class AfficheHelpMap : FullAuditEntityClassMapper<AfficheHelp, Guid>
    {
		public AfficheHelpMap ()
		{
			Table("T_Base_AfficheHelp");
				
			Map(x => x.Type).Column("Type");
			Map(x => x.Title).Column("Title");
			Map(x => x.Content).Column("Content");
			Map(x => x.BigClassId).Column("BigClassId");
			Map(x => x.ClassId).Column("ClassId");
			Map(x => x.IsTop).Column("IsTop");
			Map(x => x.IsIndex).Column("IsIndex");
            Map(x => x.AfficheIamge).Column("AfficheIamge");

            this.AutoMap();
		}
    }
}

