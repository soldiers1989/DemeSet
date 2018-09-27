using System.Collections.Generic;

namespace ZF.Web.Models
{
    public class StreamInfos
    {
        /// <summary>
        /// 视频流转码状态，取值：success(成功)，fail(失败)
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 视频流码率，单位Kbps
        /// </summary>
        public bool IsAudio { get; set; }
        /// <summary>
        /// 视频流清晰度定义, 取值：FD(流畅)，LD(标清)，SD(高清)，HD(超清)，OD(原画)，2K(2K)，4K(4K)
        /// </summary>
        public int Size { get; set; }
        /// <summary>
        /// 视频流长度，单位秒
        /// </summary>
        public string Definition { get; set; }
        /// <summary>
        /// 视频流是否加密流
        /// </summary>
        public string Fps { get; set; }
        /// <summary>
        /// 视频流转码出错的时候，会有该字段表示出错代码
        /// </summary>
        public float? Duration { get; set; }
        /// <summary>
        /// 视频流转码出错的时候，会有该字段表示出错信息
        /// </summary>
        public string Bitrate { get; set; }
        /// <summary>
        /// 视频流的播放地址，不带鉴权的auth_key，如果开启了播放鉴权，此地址会无法访问
        /// </summary>
        public bool Encrypt { get; set; }
        /// <summary>
        /// http://adc.onlydwy.com/a81a8879118d45499b92832639ed8405/fd9c22591333483896cca0ec40836e64-761c81cd92f7eadfc8ed443b97e87ef7-ld.m3u8
        /// </summary>
        public string FileUrl { get; set; }
        /// <summary>
        /// 视频流格式，取值：mp4, m3u8
        /// </summary>
        public string Format { get; set; }
        /// <summary>
        /// 视频流高度，单位px
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// 视频流宽度，单位px
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string JobId { get; set; }
    }

    public class Root
    {
        /// <summary>
        /// 处理状态，取值：success(成功)，fail(失败)
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 视频ID
        /// </summary>
        public string VideoId { get; set; }
        /// <summary>
        /// 事件类型
        /// </summary>
        public string EventType { get; set; }
        /// <summary>
        /// 事件产生时间, 为UTC时间：yyyy-MM-ddTHH:mm:ssZ
        /// </summary>
        public string EventTime { get; set; }
        /// <summary>
        /// StreamInfos
        /// </summary>
        public List<StreamInfos> StreamInfos { get; set; }
    }

}