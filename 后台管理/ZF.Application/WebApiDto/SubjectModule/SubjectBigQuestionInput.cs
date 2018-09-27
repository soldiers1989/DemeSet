namespace ZF.Application.WebApiDto.SubjectModule
{
    /// <summary>
    /// 获取试题输入模型
    /// </summary>
    public class SubjectBigQuestionInput
    {
        /// <summary>
        /// 试题编号
        /// </summary>
        public string BigQuestionId { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 作答记录编码
        /// </summary>
        public  string PaperRecordsId { get; set; }
    }
}