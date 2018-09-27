using System.Collections.Generic;
using System.Xml.Schema;

namespace ZF.Application.WebApiDto.SubjectModule
{
    /// <summary>
    /// 
    /// </summary>
    public class PaperInfoOutput
    {
        /// <summary>
        /// 分类编号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 试题数量
        /// </summary>
        public int QuestionCount { get; set; }

        /// <summary>
        /// 题型总分
        /// </summary>
        public int QuestionTypeScoreSum { get; set; }

        /// <summary>
        /// 每题分数
        /// </summary>
        public string QuestionScore { get; set; }

        /// <summary>
        /// 题型类型
        /// </summary>
        public int QuestionType { get; set; }

        /// <summary>
        /// 题型分类
        /// </summary>
        public string QuestionClass { get; set; }

        /// <summary>
        /// 难度等级
        /// </summary>
        public int DifficultLevel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Question> Question { get; set; } = new List<Question>();
    }

    /// <summary>
    /// 试题明细
    /// </summary>
    public class Question
    {
        /// <summary>
        /// 试题编号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 试题类型
        /// </summary>
        public string SubjectType { get; set; }

        /// <summary>
        /// 题目序号
        /// </summary>
        public int RowsIndex { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PaperInfoOutputView
    {
        /// <summary>
        /// 分类编号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 试题数量
        /// </summary>
        public int QuestionCount { get; set; }

        /// <summary>
        /// 题型总分
        /// </summary>
        public int QuestionTypeScoreSum { get; set; }

        /// <summary>
        /// 每题分数
        /// </summary>
        public string QuestionScore { get; set; }

        /// <summary>
        /// 题型类型
        /// </summary>
        public int QuestionType { get; set; }

        /// <summary>
        /// 题型分类
        /// </summary>
        public string QuestionClass { get; set; }

        /// <summary>
        /// 难度等级
        /// </summary>
        public int DifficultLevel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<QuestionView> Question { get; set; } = new List<QuestionView>();
    }

    /// <summary>
    /// 试题明细
    /// </summary>
    public class QuestionView
    {
        /// <summary>
        /// 试题编号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 试题类型
        /// </summary>
        public string SubjectType { get; set; }

        /// <summary>
        /// 题目序号
        /// </summary>
        public int RowsIndex { get; set; }

        /// <summary>
        /// 是否答题正确
        /// </summary>
        public int IsCollection { get; set; }
    }


    public class PaperInfoModel
    {
        /// <summary>
        /// 试卷名称
        /// </summary>
        public string PaperName { get; set; }

        /// <summary>
        /// 考试时长
        /// </summary>
        public int? TestTime { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 试题数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 练习人数
        /// </summary>
        public int Count1 { get; set; }

        /// <summary>
        /// 头像路径
        /// </summary>
        public string ImgUrl { get; set; }

        /// <summary>
        /// 试卷练习编号
        /// </summary>
        public string PracticeNo { get; set; }

        /// <summary>
        /// 试卷总分
        /// </summary>
        public string QuestionScore { get; set; }

        /// <summary>
        /// 试卷得分
        /// </summary>
        public decimal? Score { get; set; }
    }

}