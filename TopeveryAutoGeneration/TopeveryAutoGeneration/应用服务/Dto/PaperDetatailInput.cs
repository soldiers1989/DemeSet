using System;
using Topevery.Application.BaseDto;

namespace Topevery.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：PaperDetatail 
    /// </summary>
   [AutoMap(typeof(PaperDetatail ))]
    public class PaperDetatailInput
    {
       /// <summary>
       /// 编码
       /// </summary>     
       public string Id{ get; set; }
       /// <summary>
       /// 试卷编码
       /// </summary>     
       public string PaperId{ get; set; }
       /// <summary>
       /// 试题编码
       /// </summary>     
       public string QuestionId{ get; set; }
       /// <summary>
       /// 试题类型
       /// </summary>     
       public string QuestionTypeId{ get; set; }
       /// <summary>
       /// 试题分数
       /// </summary>     
       public decimal QuestionScore{ get; set; }
       /// <summary>
       /// 创建时间
       /// </summary>     
       public DateTime AddTime{ get; set; }
       /// <summary>
       /// 创建人
       /// </summary>     
       public string AddUserId{ get; set; }
       /// <summary>
       /// 修改时间
       /// </summary>     
       public DateTime? UpdateTime{ get; set; }
       /// <summary>
       /// 修改人
       /// </summary>     
       public string UpdateUserId{ get; set; }
       /// <summary>
       /// 是否删除
       /// </summary>     
       public int IsDelete{ get; set; }
    }
}

