using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：DataType 
    /// </summary>
    public class DataType:FullAuditEntity<Guid>
    {
       /// <summary>
       /// 数据类型代码
       /// </summary>     
       public string DataTypeCode{ get; set; }

       /// <summary>
       /// 数据类型名称
       /// </summary>     
       public string DataTypeName{ get; set; }

       /// <summary>
       /// 说明
       /// </summary>     
       public string Desc{ get; set; }

       /// <summary>
       /// 排序号
       /// </summary>     
       public int? Sort{ get; set; }

    }
}

