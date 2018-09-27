﻿namespace ZF.Web.Models.File
{
    /// <summary>
    /// 文件上传Dto
    /// 袁贝尔
    /// 20161031
    /// </summary>
    public class UploadFileDto
    {
        /// <summary>
        /// 文件上传控件Id
        /// </summary>
        public string UploadId { set; get; }

        /// <summary>
        /// 相关主体Id（业务Id）
        /// </summary>
        public string KeyId { set; get; }

        /// <summary>
        /// 上传类型
        /// </summary>
        public string UploadContentType { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public int Type { get; set; } = 0;

        /// <summary>
        /// 上传地址
        /// </summary>
        public string ServerUrl { get; set; }
    }
}