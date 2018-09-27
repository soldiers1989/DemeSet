using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：使用指南 
    /// </summary>
    public class UseDescriptionListInput: BasePageInput
    {
       /// <summary>
       /// 大类编号
       /// </summary>     
       public string BigClassId { get; set; }
       /// <summary>
       /// 小类编号
       /// </summary>     
       public string ClassId{ get; set; }
    
    }
}
