namespace ZF.Application.WebApiDto.CourseModule
{
    /// <summary>
    /// 课程排名输入模型
    /// </summary>
    public class PaidListingsInput
    {
        /// <summary>
        /// 课程类型(0:免费 1：付费)
        /// </summary>
        public int Charge { get; set; }
    }
}