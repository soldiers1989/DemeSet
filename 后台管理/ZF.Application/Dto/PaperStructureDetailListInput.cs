using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：PaperStructureDetail 
    /// </summary>
    public class PaperStructureDetailListInput: BasePageInput
    {
       /// <summary>
       /// 编码
       /// </summary>     
       public string Id{ get; set; }
       /// <summary>
       /// 试卷结构编码
       /// </summary>     
       public string StuctureId{ get; set; }
       /// <summary>
       /// 题型名称
       /// </summary>     
       public string QuesitonTypeName{ get; set; }
       /// <summary>
       /// 题型类型
       /// </summary>     
       public string QuestionType{ get; set; }
       /// <summary>
       /// 题型分类
       /// </summary>     
       public string QuestionClass{ get; set; }
       /// <summary>
       /// 难度等级
       /// </summary>     
       public int? DifficultLevel{ get; set; }
       /// <summary>
       /// 试题数量
       /// </summary>     
       public int QuestionCount{ get; set; }
       /// <summary>
       /// 题型总分
       /// </summary>     
       public int QuestionTypeScoreSum{ get; set; }
       /// <summary>
       /// 排序号
       /// </summary>     
       public int OrderNo{ get; set; }
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
