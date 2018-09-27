using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：编码 
    /// </summary>
    public sealed class DeliveryAddressMap : BaseClassMapper<DeliveryAddress, Guid>
    {
		public DeliveryAddressMap ()
		{
			Table("DeliveryAddress");
				
			Map(x => x.UserId).Column("UserId");
			Map(x => x.Contact).Column("Contact");
			Map(x => x.ContactPhone).Column("ContactPhone");
			Map(x => x.Zip).Column("Zip");
			Map(x => x.DetailedAddress).Column("DetailedAddress");
			Map(x => x.DefaultAddress).Column("DefaultAddress");
			Map(x => x.Province).Column("Province");
			Map(x => x.City).Column("City");
			Map(x => x.Town).Column("Town");
			Map(x => x.AddTime).Column("AddTime");
			
			this.AutoMap();
		}
    }
}

