using System;
using System.Collections.Generic;
using System.Text;

namespace ZF.WebSite.Models.Entity
{
    [Serializable]
    public class WeiXinMessageBass
    {
        /// <summary>
        /// 接收方微信号
        /// </summary>
        public string ToUserName { get; set; }
        /// <summary>
        /// 发送方微信号，若为普通用户，则是一个OpenID
        /// </summary>
        public string FromUserID { get; set; }
        /// <summary>
        /// 发送方微信号，若为普通用户名称
        /// </summary>
        public string FromUserName { get; set; }
        /// <summary>
        /// 消息创建时间 （整型）
        /// </summary>
        public int CreateTime { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public string MsgType { get; set; }
        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public Int64 MsgId { get; set; }

        /// <summary>
        /// 文本消息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 图片链接
        /// </summary>
        public string PicUrl { get; set; }
        /// <summary>
        /// 图片消息媒体id，可以调用多媒体文件下载接口拉取数据。语音消息媒体id，可以调用多媒体文件下载接口拉取数据。视频消息媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// 语音格式，如amr，speex等
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// 视频消息缩略图的媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        public string ThumbMediaId { get; set; }

        /// <summary>
        /// 地理位置维度
        /// </summary>
        public string Location_X { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Location_Y { get; set; }
        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public string Scale { get; set; }
        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 消息描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 消息链接
        /// </summary>
        public string Url { get; set; }

        #region 接收事件推送
        public string Event { get; set; }

        public string EventKey { get; set; }

        public string Ticket { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string Precision { get; set; }
        #endregion
    }

    [Serializable]
    public class WeiXinMessage : WeiXinMessageBass
    {
        /// <summary>
        /// 文本消息内容
        /// </summary>
       // public string Content { get; set; }
    }

    [Serializable]
    public class WeiXinPicMessage : WeiXinMessageBass
    {
        /// <summary>
        /// 图片链接
        /// </summary>
       // public string PicUrl { get; set; }
        /// <summary>
        /// 图片消息媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
       // public string MediaId { get; set; }
    }

    [Serializable]
    public class WeiXinVioceMessage : WeiXinMessageBass
    {
        /// <summary>
        /// 语音消息媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
        //public string MediaId { get; set; }

        /// <summary>
        /// 语音格式，如amr，speex等
        /// </summary>
        //public string Format { get; set; }
    }


    [Serializable]
    public class WeiXinVideoMessage : WeiXinMessageBass
    {
        /// <summary>
        /// 视频消息媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
       // public string MediaId { get; set; }
        /// <summary>
        /// 视频消息缩略图的媒体id，可以调用多媒体文件下载接口拉取数据。
        /// </summary>
       // public string ThumbMediaId { get; set; }
    }

    [Serializable]
    public class WeiXinLocationMessage : WeiXinMessageBass
    {
        /// <summary>
        /// 地理位置维度
        /// </summary>
        //public string Location_X { get; set; }
        /// <summary>
        /// 地理位置经度
        /// </summary>
       // public string Location_Y { get; set; }
        /// <summary>
        /// 地图缩放大小
        /// </summary>
       // public string Scale { get; set; }
        /// <summary>
        /// 地理位置信息
        /// </summary>
       // public string Label { get; set; }
    }

    [Serializable]
    public class WeiXinLinkMessage : WeiXinMessageBass
    {
        /// <summary>
        /// 消息标题
        /// </summary>
        //public string Title { get; set; }
        /// <summary>
        /// 消息描述
        /// </summary>
        //public string Description { get; set; }
        /// <summary>
        /// 消息链接
        /// </summary>
        //public string Url { get; set; }
    }
}
