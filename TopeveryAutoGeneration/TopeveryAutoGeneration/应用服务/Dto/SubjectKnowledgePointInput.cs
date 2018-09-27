using System;
using Topevery.Application.BaseDto;

namespace Topevery.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：SubjectKnowledgePoint 
    /// </summary>
   [AutoMap(typeof(SubjectKnowledgePoint ))]
    public class SubjectKnowledgePointInput
    {
       /// <summary>
       /// 知识点编码
       /// </summary>     
       public string Id{ get; set; }
       /// <summary>
       /// 知识点名称
       /// </summary>     
       public string KnowledgePointName{ get; set; }
       /// <summary>
       /// 知识点代码
       /// </summary>     
       public string KnowledgePointCode{ get; set; }
       /// <summary>
       /// 所属科目
       /// </summary>     
       public string SubjectId{ get; set; }
       /// <summary>
       /// 上级知识点编码
       /// </summary>     
       public string ParentId{ get; set; }
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

