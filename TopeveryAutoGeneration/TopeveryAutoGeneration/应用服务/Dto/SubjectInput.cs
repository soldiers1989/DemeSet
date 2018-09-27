using System;
using Topevery.Application.BaseDto;

namespace Topevery.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：Subject 
    /// </summary>
   [AutoMap(typeof(Subject ))]
    public class SubjectInput
    {
       /// <summary>
       /// 科目编码
       /// </summary>     
       public string Id{ get; set; }
       /// <summary>
       /// 科目名称
       /// </summary>     
       public string SubjectName{ get; set; }
       /// <summary>
       /// 所属项目
       /// </summary>     
       public string ProjectId{ get; set; }
       /// <summary>
       /// 排序号
       /// </summary>     
       public int OrderNo{ get; set; }
       /// <summary>
       /// 科目说明
       /// </summary>     
       public string Remark{ get; set; }
       /// <summary>
       /// 创建时间
       /// </summary>     
       public DateTime? AddTime{ get; set; }
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
       public bool? IsDelete{ get; set; }
    }
}

