using System;
using System.Collections.Generic;
using System.Text;

namespace ZF.WebSite.Models.Entity
{
    [Serializable]
    public class WeiXinSendMessage
    {
        /// <summary>
        /// 普通用户openid
        /// </summary>
        public string touser { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public string msgtype { get; set; }
    }

    [Serializable]
    public class TextMessage : WeiXinSendMessage
    {
        /// <summary>
        /// 文本消息内容
        /// </summary>
        public ContentClass text { get; set; }

    }

    [Serializable]
    public class ContentClass
    {
        public string content { get; set; }
    }


    [Serializable]
    public class ImgMessage : WeiXinSendMessage
    {
        /// <summary>
        /// 图片消息
        /// </summary>
        public MediaClass image { get; set; }
    }

    [Serializable]
    public class MediaClass
    {
        public string media_id { get; set; }
    }

    [Serializable]
    public class VidMessage : WeiXinSendMessage
    {
        /// <summary>
        /// 视频消息
        /// </summary>
        private List<VideoClass> _video = new List<VideoClass>();
        public List<VideoClass> video
        {
            get
            {
                return _video;
            }
        }
    }

    [Serializable]
    public class VoiMessage : WeiXinSendMessage
    {
        /// <summary>
        /// 语音消息
        /// </summary>
        private List<MediaClass> _voice = new List<MediaClass>();
        public List<MediaClass> voice
        {
            get
            {
                return _voice;
            }
        }
    }

    [Serializable]
    public class VideoClass : MediaClass
    {
        public string title { get; set; }
        public string description { get; set; }
    }


    [Serializable]
    public class NewsMessage : WeiXinSendMessage
    {
        /// <summary>
        /// 图文消息
        /// </summary>
        private ArticlesClass _news = new ArticlesClass();
        public ArticlesClass news
        {
            get
            {
                return _news;
            }
        }
    }
    [Serializable]
    public class ArticlesClass
    {
        private List<NewsClass> _articles = new List<NewsClass>();

        public List<NewsClass> articles
        {
            get { return _articles; }
            set { _articles = value; }
        }

    }

    public class NewsClass
    {
        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string picurl { get; set; }
    }

    [Serializable]
    public class MusicMessage : WeiXinSendMessage
    {
        /// <summary>
        /// 音乐消息
        /// </summary>
        private List<Music> _music = new List<Music>();
        public List<Music> music
        {
            get
            {
                return _music;
            }
        }
    }

    [Serializable]
    public class Music
    {
        public string title { get; set; }
        public string description { get; set; }
        public string musicurl { get; set; }
        public string hqmusicurl { get; set; }
        public string thumb_media_id { get; set; }
    }

    //图文素材
    
    public class ArticleClass
    {
        public string thumb_media_id { get; set; }
        public string author { get; set; }
        public string title { get; set; }
        public string content_source_url { get; set; }
        public string content { get; set; }
        public string digest { get; set; }
        public string show_cover_pic { get; set; }
    }

    
    public class ArticleListData
    {
        public List<ArticleClass> articles { get; set; }
    }

    [Serializable]
    public class MpNews : WeiXinSendMessage
    {
        /// <summary>
        /// 图文消息
        /// </summary>
        public MediaClass mpnews { get; set; }
    }
}
