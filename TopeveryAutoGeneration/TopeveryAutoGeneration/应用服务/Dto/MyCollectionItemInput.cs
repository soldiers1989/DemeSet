using System;
using Topevery.Application.BaseDto;

namespace Topevery.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：MyCollectionItem 
    /// </summary>
   [AutoMap(typeof(MyCollectionItem ))]
    public class MyCollectionItemInput
    {
       /// <summary>
       /// 编码
       /// </summary>     
       public string Id{ get; set; }
       /// <summary>
       /// 用户编码
       /// </summary>     
       public string UserId{ get; set; }
       /// <summary>
       /// 试题编码
       /// </summary>     
       public string QuestionId{ get; set; }
       /// <summary>
       /// 添加时间
       /// </summary>     
       public DateTime? AddTime{ get; set; }
    }
}

