namespace ZF.Application.WebApiAppService.CourseSecurityCodeModule
{
    public class ComplaintsInput
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 网店名称
        /// </summary>
        public string BranchesName { get; set; }

        /// <summary>
        /// 实体店地址
        /// </summary>
        public string PhysicalStoreName { get; set; }

        /// <summary>
        /// 书籍封面照片
        /// </summary>
        public string ImageUrl { get; set; }
    }
}