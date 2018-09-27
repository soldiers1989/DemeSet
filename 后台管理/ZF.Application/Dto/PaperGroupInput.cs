using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：试卷组表 
    /// </summary>
   [AutoMap(typeof(PaperGroup ))]
    public class PaperGroupInput
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

