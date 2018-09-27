using System;
using System.Collections.Generic;
using System.Text;

namespace ZF.WebSite.Models.Entity
{
    [Serializable]
    public enum WeiXInMediaTypeEnum
    {
        /// <summary>
        /// 文本消息内容
        /// </summary>
        Text = 0,
        //图片,1M，支持JPG格式
        Image = 1,
        //语音,2M，播放长度不超过60s，支持AMR\MP3格式
        Voice = 2,
        // 视频,10MB，支持MP4格式
        Video = 3,
        //缩略图,64KB，支持JPG格式 
        Thumb = 4,
        /// <summary>
        /// 地理位置消息
        /// </summary>
        Location = 5,
        /// <summary>
        /// 链接消息
        /// </summary>
        Link = 6,
        /// <summary>
        /// 菜单事件、消息类型
        /// </summary>
        MenuEvent = 7,


    }
}
