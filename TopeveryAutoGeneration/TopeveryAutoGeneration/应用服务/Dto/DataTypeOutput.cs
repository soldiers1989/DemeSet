using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 数据表DataType 输出Dto
    /// </summary>
    public class DataTypeOutput
    {
       /// <summary>
       /// 主键编号
       /// </summary>     
      public string Id{ get; set; }
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
       /// <summary>
       /// 创建时间
       /// </summary>     
      public DateTime AddTime{ get; set; }
       /// <summary>
       /// 创建人
       /// </summary>     
      public string AddUserId{ get; set; }
       /// <summary>
       /// 修改时间
       /// </summary>     
      public DateTime? UpdateTime{ get; set; }
       /// <summary>
       /// 修改人
       /// </summary>     
      public string UpdateUserId{ get; set; }
       /// <summary>
       /// 是否删除
       /// </summary>     
      public int IsDelete{ get; set; }
    }
}

