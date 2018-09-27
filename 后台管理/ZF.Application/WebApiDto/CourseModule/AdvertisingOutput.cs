namespace ZF.Application.WebApiDto.CourseModule
{
    public class AdvertisingOutput
    {
        /// <summary>
        /// 套餐课程
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 套餐封面
        /// </summary>
        public string CourseIamge { get; set; }

        /// <summary>
        /// 套餐名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 0 课程  1 套餐
        /// </summary>
        public int? CourseType { get; set; }
    }
}