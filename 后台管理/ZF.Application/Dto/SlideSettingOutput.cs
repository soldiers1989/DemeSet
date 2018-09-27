using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输出Output:幻灯片设置表 
    /// </summary>
    public class SlideSettingOutput
    {
        /// <summary>
        /// 编号
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 图片地址  web 
        /// </summary>     
        public string Url { get; set; }

        /// <summary>
        /// 图片地址  移动端
        /// </summary>     
        public string AppUrl { get; set; }

        /// <summary>
        /// 状态
        /// </summary>     
        public int? State { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>     
        public string LinkAddress { get; set; }

        /// <summary>
        /// 链接地址
        /// </summary>     
        public string AppLinkAddress { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>     
        public int? OrderNo { get; set; }
        /// <summary>
        /// 上传时间
        /// </summary>     
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>     
        public string Remark { get; set; }
    }
}

