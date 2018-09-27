using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：幻灯片设置表 
    /// </summary>
    public sealed class SlideSettingMap : BaseClassMapper<SlideSetting, Guid>
    {
        public SlideSettingMap()
        {
            Table("t_Base_SlideSetting");

            Map(x => x.Url).Column("Url");
            Map(x => x.State).Column("State");
            Map(x => x.Type).Column("Type");
            Map(x => x.LinkAddress).Column("LinkAddress");
            Map(x => x.OrderNo).Column("OrderNo");
            Map(x => x.CreateTime).Column("CreateTime");
            Map(x => x.Remark).Column("Remark");
            Map(x => x.AppUrl).Column("AppUrl");
            Map(x => x.AppLinkAddress).Column("AppLinkAddress");
            

            this.AutoMap();
        }
    }
}

