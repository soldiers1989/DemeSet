using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：科目关联书籍管理 
    /// </summary>
    public class SubjectBookListInput: BasePageInput
    {
       /// <summary>
       /// 书籍名称
       /// </summary>     
       public string BookName{ get; set; }
       /// <summary>
       /// 所属科目
       /// </summary>     
       public string SubjectId{ get; set; }
    }
}
