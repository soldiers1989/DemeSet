using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Http;
using ZF.WebSite.App_Data;
using ZF.WebSite.Models;
using ZF.WebSite.Models.Entity;

namespace ZF.WebSite.API
{

    /// <summary>
    /// 作者：陈孝义
    /// 本地应用对微信服务器请求的中转者
    /// 本地应用经常是部署在没有外网的服务器上，所以需要要用到这个中转者，也就是说这个中转者必须要部署在有外网的服务器上
    /// 该中转者用WebApi进行包装，所以要通过HTTP请求来调用
    /// 调用的URL格式如：http://{该Web应用程序的路径}/api/WeiXin/{方法名}
    /// 获取微信用户列表例子：http://localhost/Topevery.DUM.WeixinService.Web/api/WeiXin/GetWeiXinUserList
    /// </summary>

    public class WeiXin11Controller : ApiController
    {
        public static string DefuleDomainQn = ConfigurationManager.AppSettings["DefuleDomainQn"];

        // GET: Login

        #region 用户管理

        /// <summary>
        /// 获取微信用户列表
        /// </summary>
        /// <returns></returns>
        public WeiXinUserList GetWeiXinUserList()
        {
            return WeiXinServiceManager.GetWeiXinUserList();
        }

        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WeiXinUser GetWeiXinUser(string id)
        {
            return WeiXinServiceManager.GetGetWeiXinUser(id);
        }

        /// <summary>
        /// 创建分组
        /// </summary>
        /// <param name="gd"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public int CreateWeiXinGroup(WeiXinGroupData gd)
        {
            return WeiXinServiceManager.CreateWeiXinGroup(gd.name);
        }

        /// <summary>
        /// 修改组名
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public WeiXinReturnMessag UpdateGroupName()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];//获取传统context
            HttpRequestBase request = context.Request;//定义传统request对象

            string groupId = request["groupId"];
            string groupName = request["groupName"];
            WeiXinReturnMessag result = WeiXinServiceManager.UpdateGroupName(groupId, groupName);
            return result;
        }

        /// <summary>
        /// 查询所有分组
        /// </summary>
        /// <returns></returns>
        public WeiXinGroupList GetWeiXinGroups()
        {
            return WeiXinServiceManager.GetWeiXinGroups();
        }

        /// <summary>
        /// 设置备注名
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public WeiXinReturnMessag UpdateRemarkName()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];//获取传统context
            HttpRequestBase request = context.Request;//定义传统request对象

            string openId = request["openId"];
            string remarkName = request["remarkName"];
            WeiXinReturnMessag result = WeiXinServiceManager.UpdateRemarkName(openId, remarkName);
            return result;
        }

        /// <summary>
        /// 批量移动用户到其它用户组
        /// </summary>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public WeiXinReturnMessag BatchMoveUser()
        {
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];//获取传统context
            HttpRequestBase request = context.Request;//定义传统request对象

            List<string> list = new List<string>();
            string strOpenList = request["openIdList"];
            if (!string.IsNullOrEmpty(strOpenList))
            {
                foreach (var item in strOpenList.Split(','))
                {
                    list.Add(item);
                }
            }
            string groupId = request["groupId"];
            WeiXinReturnMessag result = WeiXinServiceManager.BatchMoveUser(list, groupId);
            return result;
        }

        #endregion

        #region 发送消息

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public WeiXinReturnMessag SendTextMessage(SendMessage message)
        {
            return WeiXinServiceManager.SendTextMessage(message.openId, message.context);
        }


        /// <summary>
        /// 发送图片消息
        /// </summary>
        /// <param name="message"></param>
        [System.Web.Http.HttpPost]
        public WeiXinReturnMessag SendPicMessage(SendMessage message)
        {
            return WeiXinServiceManager.SendPicMessage(message.url, message.openId, message.mediaid);
        }

        /// <summary>
        /// 发送语音消息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="openId"></param>
        /// <param name="mediaId"></param>
        [System.Web.Http.HttpPost]
        public WeiXinReturnMessag SendVioMessage(SendMessage message)
        {
            return WeiXinServiceManager.SendVioMessage(message.url, message.openId, message.mediaid);
        }

        /// <summary>
        /// 发送视频消息
        /// </summary>
        /// <param name="message"></param>
        public WeiXinReturnMessag SendVidMessage(SendMessage message)
        {
            return WeiXinServiceManager.SendVidMessage(message.url, message.openId, message.mediaid, message.title, message.description);
        }

        /// <summary>
        /// 发送图文消息
        /// </summary>
        /// <param name="message"></param>
        [System.Web.Http.HttpPost]
        public WeiXinReturnMessag SendNewsMessage(SendMessage message)
        {
            return WeiXinServiceManager.SendNewsMessage(message.url, message.openId, message.newsMessage);
        }

        /// <summary>
        /// 发送图文消息
        /// </summary>
        /// <param name="message"></param>
        [System.Web.Http.HttpPost]
        public WeiXinReturnMessag SendMpNewsMessage(SendMessage message)
        {
            return WeiXinServiceManager.SendNewsMessage(message.openId, message.mediaid);
        }


        /// <summary>
        /// 发送音乐消息
        /// </summary>
        /// <param name="message"></param>
        [System.Web.Http.HttpPost]
        public WeiXinReturnMessag SendMusicMessage(SendMessage message)
        {
            return WeiXinServiceManager.SendMusicMessage(message.url, message.openId, message.newsMessage);
        }

        #endregion

        #region 创建菜单

        /// <summary>
        /// 创建一级菜单
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <returns></returns>
        public WeiXinReturnMessag CreateFirstMenu<T>(FirstMenu<T> m)
        {
            return WeiXinServiceManager.CreateFirstMenu<T>(m);
        }

        /// <summary>
        /// 创建一级菜单, 同时二级菜单
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <returns></returns>
        public WeiXinReturnMessag CreateSecondMenu<T>(SecondMenu<T> m)
        {
            return WeiXinServiceManager.CreateSecondMenu<T>(m);
        }

        public static WeiXinReturnMessag DeleteSecondMenu()
        {
            return WeiXinServiceManager.DeleteSecondMenu();
        }



        #endregion

        #region 多媒体

        [System.Web.Http.HttpPost]
        public string GetWeiXinMedia(string mediaId)
        {
            var guid = Guid.NewGuid().ToString() + ".jpg";
            if (AliyunFileUpdata.ResumeUploader(WeiXinServiceManager.GetWeiXinMedia(mediaId), guid))
            {
                return DefuleDomainQn + "/" + guid;
            }
            return "";
        }

        [System.Web.Http.HttpPost]
        public WeiXinUploadMediaResult UploadWeiXinImage()
        {
            byte[] bs = Request.Content.ReadAsByteArrayAsync().Result;
            return WeiXinServiceManager.UploadWeiXinMedia("image", bs);
        }


        #endregion

        #region 高级接口

        //发送图文消息素材
        [System.Web.Http.HttpPost]
        public WeiXinUploadMediaResult UploadWeiXinNews(ArticleListData articls)
        {
            if (articls != null)
            {
                return WeiXinServiceManager.UploadWeiXinNews(articls);
            }
            return null;
        }

        [System.Web.Http.HttpPost]
        public WeiXinMediaReturn MulitySendNewsMessage(SendNewsMessage mes)
        {
            return WeiXinServiceManager.MulitySendNewsMessage(mes.openIds, mes.mediaId, mes.msgtype);
        }

        [System.Web.Http.HttpPost]
        public WeiXinMediaReturn SendTemplateMessage(TemplateMessage msg)
        {
            return WeiXinServiceManager.SendTemplateMessage(msg);
        }
        #endregion
    }
}