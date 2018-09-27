using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：试卷组表 
    /// </summary>
    public class PaperGroupListInput: BasePageInput
    {
       /// <summary>
       /// 编码
       /// </summary>     
       public string Id{ get; set; }
       /// <summary>
       /// 试卷名称
       /// </summary>     
       public string PaperGroupName{ get; set; }
       /// <summary>
       /// 所属科目编码
       /// </summary>     
       public string SubjectId{ get; set; }
       /// <summary>
       /// 试卷状态
       /// </summary>     
       public int? State{ get; set; }
       /// <summary>
       /// 试卷属性  0历年真题 1模拟试卷
       /// </summary>     
       public int Type{ get; set; }
       
    }
}
