using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输出Output:视频文件管理 
    /// </summary>
    public class CourseVideoFileOutput
    {
       /// <summary>
       /// 编码
       /// </summary>     
      public string Id{ get; set; }
    
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

