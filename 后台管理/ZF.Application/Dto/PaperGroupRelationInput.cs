using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：试卷组试卷关系表 
    /// </summary>
    public class PaperGroupRelationInput
    {
       /// <summary>
       /// 试卷组编号
       /// </summary>     
       public string PaperGroupId{ get; set; }
       /// <summary>
       /// 试卷编号
       /// </summary>     
       public string PaperIds{ get; set; }
    }
}

