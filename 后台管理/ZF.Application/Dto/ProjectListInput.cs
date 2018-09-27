using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：Project 
    /// </summary>
    public class ProjectListInput: BasePageInput
    {
       /// <summary>
       /// 项目名称
       /// </summary>     
       public string ProjectName{ get; set; }
       /// <summary>
       /// 所属项目分类
       /// </summary>     
       public string ProjectClassId{ get; set; }
      
    }
}
