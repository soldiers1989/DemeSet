namespace ZF.Application.BaseDto
{
    /// <summary>
    /// 消息提示输出dto
    /// </summary>
    public class MessagesOutPut
    {
        /// <summary>
        /// 业务实例编号
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// 操作标志(ture成功false失败)
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 操作实体返回Id
        /// </summary>
        public string ModelId { get; set; }
    }
}