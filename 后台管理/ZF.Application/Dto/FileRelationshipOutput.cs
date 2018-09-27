using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 数据表FileRelationship 输出Dto
    /// </summary>
    public class FileRelationshipOutput
    {
        /// <summary>
        /// 编号
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 模块编号
        /// </summary>     
        public string ModuleId { get; set; }
        /// <summary>
        /// 类型
        /// </summary>     
        public int? Type { get; set; }
        /// <summary>
        /// 七牛云存储文件名
        /// </summary>     
        public string QlyName { get; set; }
        /// <summary>
        /// 上传时间
        /// </summary>     
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 文件显示默认图片
        /// </summary>
        public string FileUrl { get; set; }

        /// <summary>
        /// 文件下载地址
        /// </summary>
        public string DownloadUrl { get; set; }
    }
}

