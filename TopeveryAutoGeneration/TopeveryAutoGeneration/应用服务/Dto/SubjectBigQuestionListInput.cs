using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：SubjectBigQuestion 
    /// </summary>
    public class SubjectBigQuestionListInput: BasePageInput
    {
       /// <summary>
       /// 试题编码
       /// </summary>     
       public string Id{ get; set; }
       /// <summary>
       /// 试题标题
       /// </summary>     
       public string QuestionTitle{ get; set; }
       /// <summary>
       /// 试题内容
       /// </summary>     
       public string QuestionContent{ get; set; }
       /// <summary>
       /// 选项1
       /// </summary>     
       public string Option1{ get; set; }
       /// <summary>
       /// 选项2
       /// </summary>     
       public string Option2{ get; set; }
       /// <summary>
       /// 选项3
       /// </summary>     
       public string Option3{ get; set; }
       /// <summary>
       /// 选项4
       /// </summary>     
       public string Option4{ get; set; }
       /// <summary>
       /// 选项5
       /// </summary>     
       public string Option5{ get; set; }
       /// <summary>
       /// 选项6
       /// </summary>     
       public string Option6{ get; set; }
       /// <summary>
       /// 选项7
       /// </summary>     
       public string Option7{ get; set; }
       /// <summary>
       /// 选项8
       /// </summary>     
       public string Option8{ get; set; }
       /// <summary>
       /// 所属科目
       /// </summary>     
       public string SubjectId{ get; set; }
       /// <summary>
       /// 试题所属知识点编码
       /// </summary>     
       public string KnowledgePointId{ get; set; }
       /// <summary>
       /// 试题类型
       /// </summary>     
       public string SubjectType{ get; set; }
       /// <summary>
       /// 试题分类
       /// </summary>     
       public string SubjectClassId{ get; set; }
       /// <summary>
       /// 正确答案
       /// </summary>     
       public string RightAnswer{ get; set; }
       /// <summary>
       /// 参考答案
       /// </summary>     
       public string ConsultAnswer{ get; set; }
       /// <summary>
       /// 试题状态
       /// </summary>     
       public int State{ get; set; }
       /// <summary>
       /// 试题文字解析
       /// </summary>     
       public string QuestionTextAnalysis{ get; set; }
       /// <summary>
       /// 试题音频解析
       /// </summary>     
       public string QuestionAudioAnalysis{ get; set; }
       /// <summary>
       /// 试题视频解析
       /// </summary>     
       public string QuestionVedioAnalysis{ get; set; }
       /// <summary>
       /// 使用次数
       /// </summary>     
       public int? UseTimes{ get; set; }
       /// <summary>
       /// 电子书页码
       /// </summary>     
       public string DigitalBookPage{ get; set; }
       /// <summary>
       /// 难度等级
       /// </summary>     
       public int DifficultLevel{ get; set; }
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
