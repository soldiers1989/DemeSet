using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.WebSite.Models.Entity
{
    [Serializable]
    public enum WeiXinMsgTypeEnum
    {
        /// <summary>
        /// 图文
        /// </summary>
        mpnews = 1,
        /// <summary>
        /// 文本
        /// </summary>
        text = 2,
        /// <summary>
        /// 文本
        /// </summary>
        voice = 3,
        /// <summary>
        /// 音乐
        /// </summary>
        music = 4,
        /// <summary>
        /// 图片
        /// </summary>
        image = 5,
        /// <summary>
        /// 视频
        /// </summary>
        video = 6,
        /// <summary>
        /// 卡券
        /// </summary>
        wxcard = 7
    }
}
