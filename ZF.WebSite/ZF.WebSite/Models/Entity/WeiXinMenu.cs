using System;
using System.Collections.Generic;
using System.Text;

namespace ZF.WebSite.Models.Entity
{
    [Serializable]
    public class WeiXinClick
    {
        /// <summary>
        /// 菜单的响应动作类型，目前有click、view两种类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 菜单标题，不超过16个字节，子菜单不超过40个字节
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 菜单KEY值，用于消息接口推送，不超过128字节
        /// </summary>
        public string key { get; set; }
    }

    public class WeiXinView
    {
        /// <summary>
        /// 菜单的响应动作类型，目前有click、view两种类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 菜单标题，不超过16个字节，子菜单不超过40个字节
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 网页链接，用户点击菜单可打开链接，不超过256字节
        /// </summary>
        public string url { get; set; }
    }

    [Serializable]
    public class FirstMenu<T>
    {
        private List<T> _button = new List<T>();

        public List<T> button
        {
            get { return _button; }
            set { _button = value; }
        }
    }

    [Serializable]
    public class SecondMenu<T>
    {
        public List<T> button;
    }

    [Serializable]
    public class SecondChildMenu<T>
    {
        public string name { get; set; }

        private List<T> _button;

        public List<T> sub_button
        {
            get { return _button; }
            set { _button = value; }
        }
    }

    [Serializable]
    public class WeiXinMenu
    {
        /// <summary>
        /// 菜单的响应动作类型，目前有click、view两种类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 菜单标题，不超过16个字节，子菜单不超过40个字节
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 菜单KEY值，用于消息接口推送，不超过128字节
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// 网页链接，用户点击菜单可打开链接，不超过256字节
        /// </summary>
        public string url { get; set; }
    }
}
