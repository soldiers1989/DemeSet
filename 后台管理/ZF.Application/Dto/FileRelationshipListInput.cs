using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：FileRelationship 
    /// </summary>
    public class FileRelationshipListInput: BasePageInput
    {
    
       /// <summary>
       /// 模块编号
       /// </summary>     
       public string ModuleId{ get; set; }
       /// <summary>
       /// 类型
       /// </summary>     
       public int? Type{ get; set; }
    }
}
