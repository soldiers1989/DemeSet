using System;
using ZF.Application.BaseDto;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：SubjectBigQuestion 
    /// </summary>
   [AutoMap(typeof(SubjectBigQuestion ))]
    public class SubjectBigQuestionInput :BasePageInput
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
        /// 选项数量
        /// </summary>
        public int Number { get; set; }

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
       public int SubjectType{ get; set; }
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
        /// 视频编码
        /// </summary>     
        public string VideoId { get; set; }


    }

    /// <summary>
    /// 
    /// </summary>
    public class SubjectBigQuestionInputForCourseChapter : SubjectBigQuestionInput {

        /// <summary>
        /// 课程ID
        /// </summary>
        public string CourseId { get; set; }
        /// <summary>
        /// 课程章节ID
        /// </summary>
        public string ChapterId { get; set; }

        /// <summary>
        /// 课程父章节
        /// </summary>
        public string ParentId { get; set; }


        #region 解决同一主页面多个name属性相同问题
        /// <summary>
        /// 试题分类
        /// </summary>     
        public string SubjectClass_Id { get; set; }
        /// <summary>
        /// 所属科目
        /// </summary>     
        public string Subject_Id { get; set; }
        /// <summary>
        /// 试题所属知识点编码
        /// </summary>     
        public string KnowledgePoint_Id { get; set; }
        /// <summary>
        /// 试题类型
        /// </summary>     
        public int Subject_Type { get; set; }

        /// <summary>
        /// 章节编码
        /// </summary>
        public string Chapter_Id { get; set; }


        #endregion
    }
}

