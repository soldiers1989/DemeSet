using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：视频文件管理 
    /// </summary>
    public class CourseVideoFileListInput: BasePageInput
    {
       /// <summary>
       /// 编码
       /// </summary>     
       public string Id{ get; set; }
       /// <summary>
       /// 视频编码
       /// </summary>     
       public string VideoId{ get; set; }
       /// <summary>
       /// 视频封面
       /// </summary>     
       public string CoverURL{ get; set; }
       /// <summary>
       /// 视频名称
       /// </summary>     
       public string Name{ get; set; }
       /// <summary>
       /// 视频类型
       /// </summary>     
       public string Type{ get; set; }
       /// <summary>
       /// 视频时长
       /// </summary>     
       public decimal? Duration{ get; set; }

        /// <summary>
        /// 视频别名
        /// </summary>
        public string VideoAlias { get; set; }
    }
}
