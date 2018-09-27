using System.Collections.Generic;

namespace ZF.Application.WebApiDto.SubjectModule
{
    public class ChapterPracticeOutput
    {

        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CourseId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SubjectId { get; set; }
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
        /// 输入框类型
        /// </summary>
        public string input_type { get; set; }

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
        /// 类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 试题数量
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// 试题选项集合
        /// </summary>
        public List<Option> option { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ExaminationModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int SubjectType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ChapterPracticeOutput> ChapterPracticeOutput { get; set; }
    }
}