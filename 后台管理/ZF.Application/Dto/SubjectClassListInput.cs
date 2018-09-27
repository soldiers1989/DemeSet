using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：SubjectClass 
    /// </summary>
    public class SubjectClassListInput: BasePageInput
    {
       /// <summary>
       /// 分类名称
       /// </summary>     
       public string ClassName{ get; set; }
       /// <summary>
       /// 分类所属项目
       /// </summary>     
       public string ProjectId{ get; set; }
    }
}
