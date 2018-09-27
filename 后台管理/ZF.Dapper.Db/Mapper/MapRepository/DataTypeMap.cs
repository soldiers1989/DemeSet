using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：DataType 
    /// </summary>
    public sealed class DataTypeMap : BaseClassMapper<DataType, Guid>
    {
        public DataTypeMap()
        {
            Table("t_Base_DataType");

            Map(x => x.DataTypeCode).Column("DataTypeCode");
            Map(x => x.DataTypeName).Column("DataTypeName");
            Map(x => x.Desc).Column("Desc");
            Map(x => x.Sort).Column("Sort");
            Map(x => x.AddTime).Column("AddTime");
            Map(x => x.AddUserId).Column("AddUserId");
            Map(x => x.UpdateTime).Column("UpdateTime");
            Map(x => x.UpdateUserId).Column("UpdateUserId");
            Map(x => x.IsDelete).Column("IsDelete");

            this.AutoMap();
        }
    }
}

