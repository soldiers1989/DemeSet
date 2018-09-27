using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：幻灯片设置表 
    /// </summary>
    [AutoMap(typeof(SlideSetting))]
    public class SlideSettingInput
    {
        /// <summary>
        /// 编号
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 图片地址
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
        /// 类型  0首页  1课程
        /// </summary>     
        public int? Type { get; set; }

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

        /// <summary>
        /// 文件集合
        /// </summary>
        public string IdFilehiddenFile { get; set; }

        /// <summary>
        /// 文件集合
        /// </summary>
        public string IdFilehiddenFile1 { get; set; }
    }
}

