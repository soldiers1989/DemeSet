using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：DataType 
    /// </summary>
    public class DataTypeListInput: BasePageInput
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
      
    }
}
