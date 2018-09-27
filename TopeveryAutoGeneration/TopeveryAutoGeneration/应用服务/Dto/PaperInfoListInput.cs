using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：PaperInfo 
    /// </summary>
    public class PaperInfoListInput: BasePageInput
    {
       /// <summary>
       /// 编码
       /// </summary>     
       public string Id{ get; set; }
       /// <summary>
       /// 试卷名称
       /// </summary>     
       public string PaperName{ get; set; }
       /// <summary>
       /// 试卷结构编码
       /// </summary>     
       public string PaperStructureId{ get; set; }
       /// <summary>
       /// 所属科目编码
       /// </summary>     
       public string SubjectId{ get; set; }
       /// <summary>
       /// 考试时长
       /// </summary>     
       public int? TestTime{ get; set; }
       /// <summary>
       /// 试卷状态
       /// </summary>     
       public int? State{ get; set; }
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
