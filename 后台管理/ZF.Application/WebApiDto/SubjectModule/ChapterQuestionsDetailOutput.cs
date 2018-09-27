using System;
using System.Collections.Generic;
using ZF.Core;

namespace ZF.Application.WebApiDto.SubjectModule
{

    /// <summary>
    /// 
    /// </summary>
    public class ChapterQuestionsDetailModel : ChapterQuestionsModel
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ChapterQuestionsDetailOutput> ChapterQuestionsDetailOutput { get; set; }
    }

    /// <summary>
    /// 试题大表输出模型
    /// </summary>
    public class ChapterQuestionsDetailOutput : BaseEntity<Guid>
    {

       

        /// <summary>
        /// 练习标题
        /// </summary>
        public string ChapterTiele { get; set; }
        /// <summary>
        ///章节名称
        /// </summary>
        public string CapterName { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 考试时间
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// 章节试题统计
        /// </summary>
        public int Count { get; set; }


        /// <summary>
        ///答案是否正确
        /// </summary>
        public string Whether { get; set; }


        /// <summary>
        ///答案是否正确
        /// </summary>
        public int IsWhether { get; set; }
        

        /// <summary>
        /// 收藏编号
        /// </summary>
        public string CollectionId { get; set; }

        /// <summary>
        /// 试题编码
        /// </summary>     
        public string QuestionId { get; set; }

        /// <summary>
        /// 试题文字解析
        /// </summary>
        public string QuestionTextAnalysis { get; set; }

        /// <summary>
        /// 课程章节练习编号
        /// </summary>     
        public string ChapterQuestionsId { get; set; }

        /// <summary>
        /// 用户编码
        /// </summary>     
        public string UserId { get; set; }

        /// <summary>
        /// 试题编码
        /// </summary>     
        public string BigQuestionId { get; set; }

        /// <summary>
        /// 小题编码
        /// </summary>     
        public string SmallQuestionId { get; set; }

        /// <summary>
        /// 考生答案
        /// </summary>     
        public string StuAnswer { get; set; }

        /// <summary>
        /// 时间
        /// </summary>     
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 大题描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 试题内容
        /// </summary>
        public string QuestionContent { get; set; }

        /// <summary>
        /// 试题数量
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 试题类型名称
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// 试题类型
        /// </summary>
        public string SubjectType { get; set; }

        /// <summary>
        /// 选项1
        /// </summary>
        public string Option1 { get; set; }

        /// <summary>
        /// 选项2
        /// </summary>
        public string Option2 { get; set; }

        /// <summary>
        /// 选项3
        /// </summary>
        public string Option3 { get; set; }

        /// <summary>
        /// 选项4
        /// </summary>
        public string Option4 { get; set; }

        /// <summary>
        /// 选项5
        /// </summary>
        public string Option5 { get; set; }

        /// <summary>
        /// 选项6
        /// </summary>
        public string Option6 { get; set; }

        /// <summary>
        /// 选项7
        /// </summary>
        public string Option7 { get; set; }

        /// <summary>
        /// 选项8
        /// </summary>
        public string Option8 { get; set; }

        /// <summary>
        /// 试题选项集合
        /// </summary>
        public List<Option> option { get; set; }

        /// <summary>
        /// 试题正确答案
        /// </summary>
        public string RightAnswer { get; set; }

        /// <summary>
        /// 是否收藏
        /// </summary>
        public int IsCollection { get; set; }
        /// <summary>
        /// 视频Id
        /// </summary>
        public string VideoId { get; set;}

        /// <summary>
        /// 知识点名称
        /// </summary>
        public string KnowledgePointName { get; set; }

        /// <summary>
        /// 视频名称
        /// </summary>
        public string VideoName { get; set; }
        /// <summary>
        /// 视频编码
        /// </summary>
        public string Code { get; set; }
    }
}