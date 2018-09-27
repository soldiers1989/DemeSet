using System;
using System.Collections.Generic;
using ZF.Core;

namespace ZF.Application.WebApiDto.SubjectModule
{
    /// <summary>
    /// 试题大表输出模型
    /// </summary>
    public class SubjectBigQuestionOutput : BaseEntity<Guid>
    {

        /// <summary>
        /// 大题描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 试题标题
        /// </summary>
        public string QuestionTitle { get; set; }

        /// <summary>
        /// 试题内容
        /// </summary>
        public string QuestionContent { get; set; }

        /// <summary>
        /// 试题类型
        /// </summary>
        public int SubjectType { get; set; }

        /// <summary>
        /// 试题分类
        /// </summary>
        public string SubjectClassId { get; set; }

        /// <summary>
        /// 试题数量
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 输入框类型
        /// </summary>
        public string input_type { get; set; }

        /// <summary>
        /// 试题类型名称
        /// </summary>
        public string SubjectTypeName { get; set; }

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
        /// 试题答案
        /// </summary>
        public string StuAnswer { get; set; }
    }

    public class Option
    {
        /// <summary>
        /// 选项序号
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 答案内容
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public string ischeck { get; set; }
    }

    /// <summary>
    /// 试题大表输出模型
    /// </summary>
    public class SubjectBigQuestionOutputView : BaseEntity<Guid>
    {

        /// <summary>
        /// 是否收藏
        /// </summary>
        public  int IsCollection { get; set; }

        /// <summary>
        /// 正确答案
        /// </summary>
        public string RightAnswer { get; set; }

        /// <summary>
        /// 收藏编号
        /// </summary>
        public string CollectionId { get; set; }

        /// <summary>
        /// 试题解析
        /// </summary>
        public string QuestionTextAnalysis { get; set; }

        /// <summary>
        /// 大题描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 试题标题
        /// </summary>
        public string QuestionTitle { get; set; }

        /// <summary>
        /// 试题内容
        /// </summary>
        public string QuestionContent { get; set; }

        /// <summary>
        /// 试题类型
        /// </summary>
        public int SubjectType { get; set; }

        /// <summary>
        /// 试题分类
        /// </summary>
        public string SubjectClassId { get; set; }

        /// <summary>
        /// 试题数量
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 输入框类型
        /// </summary>
        public string input_type { get; set; }

        /// <summary>
        /// 试题类型名称
        /// </summary>
        public string SubjectTypeName { get; set; }

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
        /// 试题答案
        /// </summary>
        public string StuAnswer { get; set; }
        /// <summary>
        /// 视频编码
        /// </summary>
        public string VideoId { get; set; }
        /// <summary>
        /// 视频名称
        /// </summary>
        public string VideoName { get; set; }
    }
}