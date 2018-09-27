using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：MyCollectionItem 
    /// </summary>
    public sealed class MyCollectionItemMap : BaseClassMapper<MyCollectionItem, Guid>
    {
		public MyCollectionItemMap ()
		{
			Table("t_My_CollectionItem");
				
			Map(x => x.UserId).Column("UserId");
			Map(x => x.QuestionId).Column("QuestionId");
			Map(x => x.AddTime).Column("AddTime");
			
			this.AutoMap();
		}
    }
}

