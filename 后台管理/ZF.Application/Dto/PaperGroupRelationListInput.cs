using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：试卷组试卷关系表 
    /// </summary>
    public class PaperGroupRelationListInput: BasePageInput
    {
       /// <summary>
       /// 编码
       /// </summary>     
       public string Id{ get; set; }
       /// <summary>
       /// 试卷组编号
       /// </summary>     
       public string PaperGroupId{ get; set; }
       /// <summary>
       /// 试卷编号
       /// </summary>     
       public string PaperName { get; set; }
    }
}
