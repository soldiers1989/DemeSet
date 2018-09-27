using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ZF.Infrastructure.Json;
using ZF.WebSite.Models;

namespace ZF.WebSite.Controllers
{
    public class BaseController : Controller
    {
        public string DefuleDomain = ConfigurationManager.AppSettings["DefuleDomain"];


        public string WebApi = ConfigurationManager.AppSettings["WebApi"];

        /// <summary>
        /// 获取标题 描述 关键字
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ModelClass GetTitle(string id, int type)
        {
            var url = WebApi + "api/CourseInfo/GetTitle";
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/json";
            #region 添加Post 参数  
            byte[] data = Encoding.UTF8.GetBytes("{\"Id\": \"" + id + "\",\"Type\": \"" + type + "\"}");
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion
            HttpWebResponse resp;
            try
            {
                resp = (HttpWebResponse)req.GetResponse();
            }
            catch (WebException ex)
            {
                resp = (HttpWebResponse)ex.Response;
                return new ModelClass();
            }
            Stream stream = resp.GetResponseStream();
            //获取响应内容  
            if (stream != null)
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
            ModelJob<ModelClass> qtinfo = (ModelJob<ModelClass>)JsonHelper.jsonDes<ModelJob<ModelClass>>(result);
            return qtinfo.Result;
        }


        /// <summary>
        /// 获取视频所属课程
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string GetCourseId(string code)
        {
            var url = WebApi + "api/CourseInfo/GetCourseId";
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/json";
            #region 添加Post 参数  
            byte[] data = Encoding.UTF8.GetBytes("{\"Id\": \"" + code + "\"}");
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion
            HttpWebResponse resp;
            try
            {
                resp = (HttpWebResponse)req.GetResponse();
            }
            catch (WebException ex)
            {
                return "";
            }
            Stream stream = resp.GetResponseStream();
            //获取响应内容  
            if (stream != null)
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
            ModelJob<string> qtinfo = (ModelJob<string>)JsonHelper.jsonDes<ModelJob<string>>(result);
            return qtinfo.Result;
        }
    }
}