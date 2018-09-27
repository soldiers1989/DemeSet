using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：ProjectClass 
    /// </summary>
    public class ProjectClassListInput: BasePageInput
    {
       /// <summary>
       /// 项目分类名称
       /// </summary>     
       public string ProjectClassName{ get; set; }
    }
}
