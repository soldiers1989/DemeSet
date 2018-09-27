using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZF.Infrastructure.Common;
using ZF.Infrastructure.QiniuYun;
using ZF.WebSite.App_Data;
using QiniuHelp = ZF.WebSite.App_Data.QiniuHelp;

namespace ZF.WebSite.Controllers
{
    public class UserController : Controller
    {


        /// <summary>
        /// 默认域名
        /// </summary>
        private static string DefuleDomain = ConfigurationManager.AppSettings["DefuleDomainQn"];


        private string saveDirection = ConfigurationManager.AppSettings["UpLoadTempFilePath"];

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Setup()
        {
            HttpPostedFileBase fileInfo = Request.Files["myfile"];
            //图片上传
            if (fileInfo != null)
            {
                string uid = Request.Form["hiuid"];
                string imagurl = Request.Form["hiimage"];
                ViewBag.uPhoto = ImageFileUp(fileInfo, uid, string.IsNullOrEmpty(imagurl) ? "" : imagurl.Split('/')[3]);
                ViewBag.uName = Request.Form["uname"];
            }
            return View();
        }
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <returns></returns>

        public ActionResult UserSetUp()
        {
           
            return View();
        }


        public JsonResult UserFliePhoto()
        {
            HttpPostedFileBase fileInfo = Request.Files["photoFile"];
            string photo = string.Empty;
            //图片上传
            if (fileInfo != null)
            {
                string uid = Request.Form["hiuid"];
                string imagurl = Request.Form["hiimage"];
                photo = ImageFileUp(fileInfo, uid, string.IsNullOrEmpty(imagurl) ? "" : imagurl.Split('/')[3]);
            }
            return Json(photo, JsonRequestBehavior.AllowGet);
        }


        private string ImageFileUp(HttpPostedFileBase fileInfo, string uid, string imagurl)
        {
            // 获取从MIME中读取出来的文件
            var listFile = Request.Files;
            string fileId2Return = "";
            string message = string.Empty;
            int maxWidth = 0;
            int maxHeight = 0;

            maxWidth = 120;
            maxHeight = 120;
            if (maxWidth > 0 && maxHeight > 0)
            {
                if (!string.IsNullOrEmpty(fileInfo?.FileName))
                {
                    Random ran = new Random();
                    int newname = ran.Next(10000, 99999);
                    fileId2Return = uid + "-" + newname.ToString() + "." + fileInfo.FileName.Split('.')[1];
                    try
                    {
                        //判断文件是否存在，存在就先删后写
                        if (AliyunFileUpdata.GetFileDate(imagurl) > 0)
                        {
                            if (AliyunFileUpdata.Delete(imagurl))
                            {
                                var fileName = "";
                                if (!Directory.Exists(saveDirection + "/photo/"))
                                    Directory.CreateDirectory(saveDirection + "/photo/");
                                fileName = saveDirection + "/photo/" + fileId2Return;
                                ImageHelp.CutForCustom(fileInfo.InputStream, fileName, maxWidth, maxHeight, 100);
                                // 打开文件
                                FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                                // 读取文件的 byte[]
                                byte[] bytes = new byte[fileStream.Length];
                                fileStream.Read(bytes, 0, bytes.Length);
                                fileStream.Close();
                                // 把 byte[] 转换成 Stream
                                Stream stream = new MemoryStream(bytes);
                                AliyunFileUpdata.ResumeUploader(stream, fileId2Return);
                            }
                        }
                        else
                        {
                            var fileName = "";
                            if (!Directory.Exists(saveDirection + "/photo/"))
                                Directory.CreateDirectory(saveDirection + "/photo/");
                            fileName = saveDirection + "/photo/" + fileId2Return;
                            ImageHelp.CutForCustom(fileInfo.InputStream, fileName, maxWidth, maxHeight, 100);
                            // 打开文件
                            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                            // 读取文件的 byte[]
                            byte[] bytes = new byte[fileStream.Length];
                            fileStream.Read(bytes, 0, bytes.Length);
                            fileStream.Close();
                            // 把 byte[] 转换成 Stream
                            Stream stream = new MemoryStream(bytes);
                            AliyunFileUpdata.ResumeUploader(stream, fileId2Return);

                        }
                    }
                    catch (Exception ex)
                    {
                        message = "";
                    }
                }

            }

            if (string.IsNullOrEmpty(message) && !string.IsNullOrEmpty(fileId2Return))
            {
                message = DefuleDomain + "/" + fileId2Return;
            }

            return message;

        }


        public ActionResult UserInfo(string userid)
        {
            ViewBag.UserId = userid;
            return View();
        }
    }
}