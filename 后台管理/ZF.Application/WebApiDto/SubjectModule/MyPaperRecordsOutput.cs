namespace ZF.Application.WebApiDto.SubjectModule
{
    /// <summary>
    /// 
    /// </summary>
    public class MyPaperRecordsOutput
    {
        /// <summary>
        /// 试卷编号
        /// </summary>
        public string PaperId { get; set; }

        /// <summary>
        /// 测评记录编号
        /// </summary>
        public string PaperRecordsId { get; set; }

        /// <summary>
        /// 试卷组编号
        /// </summary>
        public string PaperGroupId { get; set; }
    }
}