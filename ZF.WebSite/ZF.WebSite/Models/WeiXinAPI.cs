using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using ZF.WebSite.Models.Entity;

namespace ZF.WebSite.Models
{
    /// <summary>
    /// 作者：杨洋
    /// 对微信api进行包装
    /// </summary>
    public class WeiXinApi
    {
        protected string GetResponseString(string url)
        {
            string returnValue = "";
            WebRequest myWebRequest = WebRequest.Create(url);
            WebResponse myWebResponse = myWebRequest.GetResponse();
            using (StreamReader myStreamReader = new StreamReader(myWebResponse.GetResponseStream(), System.Text.Encoding.UTF8))
            {
                returnValue = myStreamReader.ReadToEnd();
            }
            return returnValue;
        }



        private Stream GetResponseStream(string url)
        {
            WebRequest myWebRequest = WebRequest.Create(url);
            WebResponse myWebResponse = myWebRequest.GetResponse();
            return myWebResponse.GetResponseStream();
        }

        protected string PostResponse(string url, string postValue)
        {
            byte[] data = new System.Text.UTF8Encoding().GetBytes(postValue);
            return PostResponse(url, data);
        }

        public string WikiLogin(string url, string postValue)
        {
            byte[] data = new System.Text.UTF8Encoding().GetBytes(postValue);
            string returnValue = "";
            WebRequest myWebRequest = WebRequest.Create(url);
            myWebRequest.Method = "POST";
            myWebRequest.ContentType = "application/json";
            myWebRequest.ContentLength = data.Length;
            Stream newStream = myWebRequest.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();

            WebResponse myWebResponse = myWebRequest.GetResponse();
            using (StreamReader myStreamReader = new StreamReader(myWebResponse.GetResponseStream(), System.Text.Encoding.UTF8))
            {
                returnValue = myStreamReader.ReadToEnd();
            }
            return returnValue;

        }

        protected string PostResponse(string url, byte[] postValue)
        {
            string returnValue = "";
            WebRequest myWebRequest = WebRequest.Create(url);
            myWebRequest.Method = "POST";
            myWebRequest.ContentType = "application/x-www-form-urlencoded";
            myWebRequest.ContentLength = postValue.Length;
            Stream newStream = myWebRequest.GetRequestStream();
            newStream.Write(postValue, 0, postValue.Length);
            newStream.Close();

            WebResponse myWebResponse = myWebRequest.GetResponse();
            using (StreamReader myStreamReader = new StreamReader(myWebResponse.GetResponseStream(), System.Text.Encoding.UTF8))
            {
                returnValue = myStreamReader.ReadToEnd();
            }
            return returnValue;
        }

        protected string PostFileResponse(string url, byte[] postValue)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Timeout = 10000;
            MemoryStream postStream = new MemoryStream();
            string boundary = "----" + DateTime.Now.Ticks.ToString("x");
            string formdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
            //foreach (KeyValuePair<string, string> file in fileDictionary)
            //{
            //    try
            //    {
            //        string fileName = file.Value;
            //        using (FileStream fileStream = FileHelper.GetFileStream(fileName))
            //        {
            //            string formdata = string.Format(formdataTemplate, file.Key, fileName);
            //            byte[] formdataBytes = Encoding.ASCII.GetBytes((postStream.Length == 0L) ? formdata.Substring(2, formdata.Length - 2) : formdata);
            //            postStream.Write(formdataBytes, 0, formdataBytes.Length);
            //            byte[] buffer = new byte[1024];
            //            int bytesRead;
            //            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            //            {
            //                postStream.Write(buffer, 0, bytesRead);
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}
            try
            {
                string fileName = string.Format("{0}.{1}", Guid.NewGuid(), "jpg");
                string formdata = string.Format(formdataTemplate, "media", fileName);
                byte[] formdataBytes = Encoding.ASCII.GetBytes((postStream.Length == 0L) ? formdata.Substring(2, formdata.Length - 2) : formdata);
                postStream.Write(formdataBytes, 0, formdataBytes.Length);
                postStream.Write(postValue, 0, postValue.Length);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            byte[] footer = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            postStream.Write(footer, 0, footer.Length);
            request.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
            request.ContentLength = ((postStream != null) ? postStream.Length : 0L);
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.KeepAlive = true;
            //if (!string.IsNullOrEmpty(refererUrl))
            //{
            //    request.Referer = refererUrl;
            //}
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36";
            //if (cookieContainer != null)
            //{
            //    request.CookieContainer = cookieContainer;
            //}
            if (postStream != null)
            {
                postStream.Position = 0L;
                Stream requestStream = request.GetRequestStream();
                //byte[] buffer = new byte[1024];
                //int bytesRead;
                //while ((bytesRead = postStream.Read(buffer, 0, buffer.Length)) != 0)
                //{
                //    requestStream.Write(buffer, 0, bytesRead);
                //}
                byte[] buffer = postStream.ToArray();
                requestStream.Write(buffer, 0, buffer.Length);
                postStream.Close();
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //if (cookieContainer != null)
            //{
            //    response.Cookies = cookieContainer.GetCookies(response.ResponseUri);
            //}
            string result;
            using (Stream responseStream = response.GetResponseStream())
            {
                using (StreamReader myStreamReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8")))
                {
                    string retString = myStreamReader.ReadToEnd();
                    result = retString;
                }
            }
            return result;
        }

        public AccessToken GetAccessToken(string url)
        {
            AccessToken at = null;
            string result = GetResponseString(url);
            object returnValue = JsonConvert.DeserializeObject<AccessToken>(result);
            at = returnValue as AccessToken;
            return at;
        }



        #region 用户管理

        /// <summary>
        /// 获取微信用户列表
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public WeiXinUserList GetWeiXinUserList(string url)
        {
            string resultUser = GetResponseString(url);
            object returnValue = JsonConvert.DeserializeObject<WeiXinUserList>(resultUser);
            return returnValue as WeiXinUserList;
        }

        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public WeiXinUser GetUserInfo(string url)
        {
            string resultUser = GetResponseString(url);
            object returnValue = JsonConvert.DeserializeObject<WeiXinUser>(resultUser);
            return returnValue as WeiXinUser;
        }

        /// <summary>
        /// 创建分组
        /// </summary>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public int CreateGroup(string url, string name)
        {
            StringBuilder group = new StringBuilder();
            group.Append(@"{""group"":{""name"":""");
            group.Append(name);
            group.Append(@"""}}");
            string returnValue = PostResponse(url, group.ToString());
            WeiXinGroup weixinGroup = JsonConvert.DeserializeObject<WeiXinGroup>(returnValue) as WeiXinGroup;
            if (weixinGroup.group.id > 0)
            {
                return weixinGroup.group.id;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 查询所有分组
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public WeiXinGroupList GetWeiXinGroupList(string url)
        {
            string returnValue = GetResponseString(url);
            object weixinGroups = JsonConvert.DeserializeObject<WeiXinGroupList>(returnValue);
            return weixinGroups as WeiXinGroupList;
        }

        /// <summary>
        /// 修改分组名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public WeiXinReturnMessag UpdateGroupName(string url, string groupId, string groupName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"group\":{");
            sb.Append("\"id\"" + ":\"" + groupId + "\",");
            sb.Append("\"name\"" + ":\"" + groupName + "\"");
            sb.Append("}}");
            string postData = sb.ToString();
            string returnValue = PostResponse(url, postData);
            WeiXinReturnMessag result = JsonConvert.DeserializeObject<WeiXinReturnMessag>(returnValue) as WeiXinReturnMessag;
            return result;
        }

        /// <summary>
        /// 设置用户的备注名
        /// </summary>
        /// <param name="url"></param>
        /// <param name="openId"></param>
        /// <param name="remarkName"></param>
        /// <returns></returns>
        public WeiXinReturnMessag UpdateRemarkName(string url, string openId, string remarkName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"openid\"" + ":\"" + openId + "\",");
            sb.Append("\"remark\"" + ":\"" + remarkName + "\"");
            sb.Append("}");
            string postData = sb.ToString();

            string returnValue = PostResponse(url, postData);
            WeiXinReturnMessag result = JsonConvert.DeserializeObject<WeiXinReturnMessag>(returnValue) as WeiXinReturnMessag;
            return result;
        }

        /// <summary>
        /// 批量移动用户到其它用户组
        /// </summary>
        /// <param name="url"></param>
        /// <param name="openId"></param>
        /// <param name="remarkName"></param>
        /// <returns></returns>
        public WeiXinReturnMessag BatchMoveUser(string url, List<string> openIdList, string groupId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"openid_list\"" + ":[");
            int index = 0;
            foreach (string item in openIdList)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    index++;
                    if (index == openIdList.Count)
                    {
                        sb.Append("\"" + item.Trim() + "\"]");
                    }
                    else
                    {
                        sb.Append("\"" + item.Trim() + "\",");
                    }
                }
            }
            sb.Append(",\"to_groupid\"" + ":\"" + groupId + "\"");
            sb.Append("}");
            string postData = sb.ToString();
            string returnValue = PostResponse(url, postData);
            WeiXinReturnMessag result = JsonConvert.DeserializeObject<WeiXinReturnMessag>(returnValue) as WeiXinReturnMessag;
            return result;
        }

        public WeiXinReturnMessag MoveUser(string url, string openId, string groupId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"openid\"" + ":\"" + openId + "\",");
            sb.Append("\"to_groupid\"" + ":\"" + groupId + "\"");
            sb.Append("}");
            string postData = sb.ToString();

            string returnValue = PostResponse(url, postData);
            WeiXinReturnMessag result = JsonConvert.DeserializeObject<WeiXinReturnMessag>(returnValue) as WeiXinReturnMessag;
            return result;
        }

        #endregion

        #region 接受消息
        #endregion

        #region 发送消息
        /// <summary>
        /// 发送微信信息
        /// </summary>
        private WeiXinReturnMessag SendWeiXinMessage(string url, string postData)
        {
            string returnValue = PostResponse(url, postData);
            WeiXinReturnMessag weixinGroup = JsonConvert.DeserializeObject<WeiXinReturnMessag>(returnValue) as WeiXinReturnMessag;
            return weixinGroup;
        }

        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="openId"></param>
        /// <param name="context"></param>
        public WeiXinReturnMessag SendTextMessage(string url, TextMessage txtMess)
        {
            string serialValue = JsonConvert.SerializeObject(txtMess);
            return SendWeiXinMessage(url, serialValue);
        }

        /// <summary>
        /// 发送图片消息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="openId"></param>
        /// <param name="mediaId"></param>
        public WeiXinReturnMessag SendPicMessage(string url, ImgMessage mess)
        {
            string serialValue = JsonConvert.SerializeObject(mess);
            return SendWeiXinMessage(url, serialValue);
        }

        /// <summary>
        /// 发送语音消息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="openId"></param>
        /// <param name="mediaId"></param>
        public WeiXinReturnMessag SendVioMessage(string url, VoiMessage mess)
        {
            string serialValue = JsonConvert.SerializeObject(mess);
            return SendWeiXinMessage(url, serialValue);
        }

        /// <summary>
        /// 发送视频消息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="openId"></param>
        /// <param name="mediaId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        public WeiXinReturnMessag SendVidMessage(string url, VidMessage mess)
        {
            string serialValue = JsonConvert.SerializeObject(mess);
            return SendWeiXinMessage(url, serialValue);
        }

        /// <summary>
        /// 发送图文消息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="mess"></param>
        public WeiXinReturnMessag SendNewsMessage(string url, NewsMessage mess)
        {
            string serialValue = JsonConvert.SerializeObject(mess);
            return SendWeiXinMessage(url, serialValue);
        }

        /// <summary>
        /// 发送图文消息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="mess"></param>
        public WeiXinReturnMessag SendNewsMessage(string url, MpNews mess)
        {
            string serialValue = JsonConvert.SerializeObject(mess);
            return SendWeiXinMessage(url, serialValue);
        }

        /// <summary>
        /// 发送音乐消息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="mess"></param>
        public WeiXinReturnMessag SendMusicMessage(string url, MusicMessage mess)
        {
            string serialValue = JsonConvert.SerializeObject(mess);
            return SendWeiXinMessage(url, serialValue);
        }

        #endregion

        #region 自定义菜单

        /// <summary>
        /// 创建一级菜单
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public WeiXinReturnMessag CreateFirstMenu<T>(string url, FirstMenu<T> m)
        {
            string serialValue = JsonConvert.SerializeObject(m);
            string returnValue = PostResponse(url, serialValue);
            WeiXinReturnMessag weixinGroup = JsonConvert.DeserializeObject<WeiXinReturnMessag>(returnValue) as WeiXinReturnMessag;
            return weixinGroup;
        }

        /// <summary>
        /// 创建一级菜单, 同时二级菜单
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public WeiXinReturnMessag CreateSecondMenu<T>(string url, SecondMenu<T> m)
        {
            string serialValue = JsonConvert.SerializeObject(m);
            string returnValue = PostResponse(url, serialValue);
            WeiXinReturnMessag weixinGroup = JsonConvert.DeserializeObject<WeiXinReturnMessag>(returnValue) as WeiXinReturnMessag;
            return weixinGroup;
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public WeiXinReturnMessag DeleteSecondMenu(string url)
        {
            string returnValue = GetResponseString(url);
            WeiXinReturnMessag weixinGroup = JsonConvert.DeserializeObject<WeiXinReturnMessag>(returnValue) as WeiXinReturnMessag;
            return weixinGroup;
        }

        /// <summary>
        /// 查询菜单
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string SearchCustomMenu(string url)
        {
            return GetResponseString(url);
        }


        #endregion

        #region 多媒体

        public Stream GetWeiXinMedia(string url)
        {
            return GetResponseStream(url);
        }

        public WeiXinUploadMediaResult UploadWeiXinMedia(string url, byte[] media)
        {
            string returnValue = PostFileResponse(url, media);
            WeiXinUploadMediaResult weixinReturn = JsonConvert.DeserializeObject<WeiXinUploadMediaResult>(returnValue) as WeiXinUploadMediaResult;
            return weixinReturn;
        }

        #endregion

        #region 高级接口

        //发送图文消息素材
        public WeiXinUploadMediaResult UploadWeiXinNews(string url, ArticleListData articls)
        {
            string serialValue = JsonConvert.SerializeObject(articls);
            string returnValue = PostResponse(url, serialValue);
            WeiXinUploadMediaResult weixinReturn = JsonConvert.DeserializeObject<WeiXinUploadMediaResult>(returnValue) as WeiXinUploadMediaResult;
            return weixinReturn;
        }

        //群发接口
        public WeiXinMediaReturn MulitySendNewsMessage(string url, List<string> openIds, string mediaId, string msgtype)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"touser\"" + ":[");
            int index = 0;
            foreach (string item in openIds)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    index++;
                    if (index == openIds.Count)
                    {
                        sb.Append("\"" + item.Trim() + "\"]");
                    }
                    else
                    {
                        sb.Append("\"" + item.Trim() + "\",");
                    }
                }
            }
            sb.Append(",\"mpnews\"" + ":{\"media_id\"" + ":\"" + mediaId + "\"}");
            sb.Append(",\"msgtype\"" + ":\"" + msgtype + "\"");
            sb.Append("}");
            string postData = sb.ToString();
            string returnValue = PostResponse(url, postData);

            WeiXinMediaReturn weixinReturn = JsonConvert.DeserializeObject<WeiXinMediaReturn>(returnValue) as WeiXinMediaReturn;
            return weixinReturn;
        }

        //模板消息
        public WeiXinMediaReturn SendTemplateMessage(string url, TemplateMessage msg)
        {
            string postData = JsonConvert.SerializeObject(msg);
            string returnValue = PostResponse(url, postData);

            WeiXinMediaReturn weixinReturn = JsonConvert.DeserializeObject<WeiXinMediaReturn>(returnValue) as WeiXinMediaReturn;
            return weixinReturn;
        }
        #endregion

        #region 网页授权
        /// <summary>
        /// 通过code获取web token
        /// </summary>
        /// <param name="url"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public WebAccessToken GetWebAccessToken(string url, string code)
        {
           
            WebAccessToken webAt = null;
            url += string.Format("&code={0}", code);
            //Topevery.DUM.GridingManagement.Framework.Utility.LogHelper.Log.InfoFormat("GetWebAccessToken,url:{0}", url);
            string result = GetResponseString(url);
            //Topevery.DUM.GridingManagement.Framework.Utility.LogHelper.Log.InfoFormat("GetWebAccessToken,result={0}", result);
            object returnValue = JsonConvert.DeserializeObject(result, typeof(WebAccessToken));
            webAt = returnValue as WebAccessToken;
            return webAt;
        }

        /// <summary>
        /// 拉取用户信息(需scope为 snsapi_userinfo)
        /// </summary>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public WeiXinUser GetSNSUserInfo(string url, string token)
        {
            WeiXinUser u = null;
            url += string.Format("&access_token={0}", token);
            //Topevery.DUM.GridingManagement.Framework.Utility.LogHelper.Log.InfoFormat("GetSNSUserInfo,url:{0}", url);
            string result = GetResponseString(url);
            //Topevery.DUM.GridingManagement.Framework.Utility.LogHelper.Log.InfoFormat("GetSNSUserInfo,result:{0}", result);
            object returnValue = JsonConvert.DeserializeObject(result, typeof(WeiXinUser));
            u = returnValue as WeiXinUser;
            return u;

        }
        #endregion

        #region 微信JS-SDK接口
        public ApiTicket GetTickect(string url,string token)
        {
            ApiTicket ticket = null;
            url = url.Replace("{0}", token);
            string result = GetResponseString(url);
            object returnValue = JsonConvert.DeserializeObject(result, typeof(ApiTicket));
            ticket = returnValue as ApiTicket;
            return ticket;
        }
        #endregion
    }
}
