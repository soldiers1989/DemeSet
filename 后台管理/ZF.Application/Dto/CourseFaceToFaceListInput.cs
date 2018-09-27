using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：面试班 
    /// </summary>
    public class CourseFaceToFaceListInput: BasePageInput
    {
       /// <summary>
       /// 课程编码
       /// </summary>     
       public string Id{ get; set; }
       /// <summary>
       /// 班级名称
       /// </summary>     
       public string ClassName{ get; set; }

        /// <summary>
        /// 主讲教师
        /// </summary>     
        public string TeacherId { get; set; }

        /// <summary>
        /// 上下架状态
        /// </summary>     
        public int? State { get; set; }
    }
}
