namespace ZF.Application.WebApiDto.SubjectModule
{
    public class AnswerQuestionInput
    {
        /// <summary>
        /// 试题编号
        /// </summary>
        public string questionId { get; set; }

        /// <summary>
        /// 答案
        /// </summary>
        public string answer;

        /// <summary>
        /// 试题类型
        /// </summary>
        public int type;

        /// <summary>
        /// 试卷编号
        /// </summary>
        public string paperId;

        /// <summary>
        /// 试题分数
        /// </summary>
        public decimal Score { get; set; }

        /// <summary>
        /// 试卷作答编码
        /// </summary>
        public string PaperRecordsId { get; set; }
    }
}