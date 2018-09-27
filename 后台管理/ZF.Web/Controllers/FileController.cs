using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Infrastructure.AlipayService;
using ZF.Infrastructure.Common;
using ZF.Infrastructure.QiniuYun;

namespace ZF.Web.Controllers
{

    /// <summary>
    /// 文件上传Control
    /// </summary>
    public class FileController : BaseController
    {

        /// <summary>
        /// 默认域名
        /// </summary>
        private static string DefuleDomain = ConfigurationManager.AppSettings["DefuleDomain"];


        private string saveDirection = ConfigurationManager.AppSettings["UpLoadTempFilePath"];

        /// <summary>
        /// 
        /// </summary>
        private readonly FileRelationshipAppService _fileRelationshipAppService;

        public FileController(FileRelationshipAppService fileRelationshipAppService)
        {
            _fileRelationshipAppService = fileRelationshipAppService;
        }

        /// <summary>
        ///查看详情
        /// </summary>
        /// <returns></returns>
        // GET: InventoryManagementIndex
        public ActionResult FileIndex(string id)
        {
            ViewBag.Id = id;
            return PartialView();
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UploadFile()
        {
            // 获取从MIME中读取出来的文件
            var listFile = Request.Files;
            Guid fid = Guid.Empty;
            string fileId2Return = "";
            string message = "";
            int maxWidth = 0;
            int maxHeight = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["fileId"]))
            {
                fid = new Guid(Request.QueryString["fileId"]);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["parameter"]))
            {
                var parameter = Request.QueryString["parameter"].Split(',');
                maxWidth = int.Parse(parameter[0]);
                maxHeight = int.Parse(parameter[1]);
                if (maxWidth > 0 && maxHeight > 0)
                {
                    for (int n = 0; n < listFile.Count; n++)
                    {
                        Guid fileId = fid;
                        if (fid == Guid.Empty)
                            fileId = Guid.NewGuid();
                        HttpPostedFileBase file = listFile[n];
                        if (!string.IsNullOrEmpty(file?.FileName))
                        {
                            fileId2Return = fileId + "." + file.FileName.Split('.')[1];
                            try
                            {
                                var fileName = "";
                                if (!Directory.Exists(saveDirection + "/photo/"))
                                    Directory.CreateDirectory(saveDirection + "/photo/");
                                fileName = saveDirection + "/photo/" + fileId2Return;
                                ImageHelp.CutForCustom(file.InputStream, fileName, maxWidth, maxHeight, 100);
                                // 打开文件
                                FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                                // 读取文件的 byte[]
                                byte[] bytes = new byte[fileStream.Length];
                                fileStream.Read(bytes, 0, bytes.Length);
                                fileStream.Close();
                                //把 byte[] 转换成 Stream
                                Stream stream = new MemoryStream(bytes);
                                //QiniuHelp.FormUploader( stream, fileId2Return );
                                //QiniuHelp.ResumeUploader(stream, fileId2Return);
                                AliyunFileUpdata.ResumeUploader(stream, fileId2Return);
                            }
                            catch (Exception ex)
                            {
                                message = ex.ToString();
                                Logger.Error("FMP错误", ex);
                            }
                        }
                    }
                }
            }
            else
            {
                for (int n = 0; n < listFile.Count; n++)
                {
                    Guid fileId = fid;
                    if (fid == Guid.Empty)
                        fileId = Guid.NewGuid();
                    HttpPostedFileBase file = listFile[n];
                    if (!string.IsNullOrEmpty(file?.FileName))
                    {
                        fileId2Return = fileId + "." + file.FileName.Split('.')[1];
                        try
                        {
                            //QiniuHelp.FormUploader(file.InputStream, fileId2Return);
                            //QiniuHelp.ResumeUploader( file.InputStream, fileId2Return );
                            AliyunFileUpdata.ResumeUploader(file.InputStream, fileId2Return);
                        }
                        catch (Exception ex)
                        {
                            message = ex.ToString();
                            Logger.Error("FMP错误", ex);
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(message) && !string.IsNullOrEmpty(fileId2Return))
            {
                return Json(new { Success = true, Message = DefuleDomain + "/" + fileId2Return, name = fileId2Return });
            }
            return Json(new { Success = false, Message = message });
        }

        [HttpPost]
        public JsonResult UploadFileToLocalServer( ) {
              // 获取从MIME中读取出来的文件
            var listFile = Request.Files;
            Guid fid = Guid.Empty;
            string fileId2Return = "";
            string message = "";
            var fileName = "";
            int maxWidth = 0;
            int maxHeight = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["fileId"]))
            {
                fid = new Guid(Request.QueryString["fileId"]);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["parameter"]))
            {
                var parameter = Request.QueryString["parameter"].Split(',');
                maxWidth = int.Parse(parameter[0]);
                maxHeight = int.Parse(parameter[1]);
                if (maxWidth > 0 && maxHeight > 0)
                {
                    for (int n = 0; n < listFile.Count; n++)
                    {
                        Guid fileId = fid;
                        if (fid == Guid.Empty)
                            fileId = Guid.NewGuid();
                        HttpPostedFileBase file = listFile[n];
                        if (!string.IsNullOrEmpty(file?.FileName))
                        {
                            fileId2Return = fileId + "." + file.FileName.Split('.')[1];
                            try
                            {
                                if (!Directory.Exists(saveDirection + "/photo/"))
                                    Directory.CreateDirectory(saveDirection + "/photo/");
                                fileName = saveDirection + "/photo/" + fileId2Return;
                                ImageHelp.CutForCustom(file.InputStream, fileName, maxWidth, maxHeight, 100);
                                //// 打开文件
                                //FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                                //// 读取文件的 byte[]
                                //byte[] bytes = new byte[fileStream.Length];
                                //fileStream.Read(bytes, 0, bytes.Length);
                                //fileStream.Close();
                                ////把 byte[] 转换成 Stream
                                //Stream stream = new MemoryStream(bytes);
                                ////QiniuHelp.FormUploader( stream, fileId2Return );
                                ////QiniuHelp.ResumeUploader(stream, fileId2Return);
                                //AliyunFileUpdata.ResumeUploader(stream, fileId2Return);
                            }
                            catch (Exception ex)
                            {
                                message = ex.ToString();
                                Logger.Error("FMP错误", ex);
                            }
                        }
                    }
                }
            }
            else
            {
                for (int n = 0; n < listFile.Count; n++)
                {
                    Guid fileId = fid;
                    if (fid == Guid.Empty)
                        fileId = Guid.NewGuid();
                    HttpPostedFileBase file = listFile[n];
                    if (!string.IsNullOrEmpty(file?.FileName))
                    {
                        fileId2Return = fileId + "." + file.FileName.Split('.')[1];
                        try
                        {
                            //QiniuHelp.FormUploader(file.InputStream, fileId2Return);
                            //QiniuHelp.ResumeUploader( file.InputStream, fileId2Return );
                            //AliyunFileUpdata.ResumeUploader(file.InputStream, fileId2Return);
                        }
                        catch (Exception ex)
                        {
                            message = ex.ToString();
                            Logger.Error("FMP错误", ex);
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(message) && !string.IsNullOrEmpty(fileId2Return))
            {
                return Json(new { Success = true, Message =fileName });
            }
            return Json(new { Success = false, Message = message });
        }


        [HttpGet]
        public FileResult ProcessRequest(string FID)
        {
            var url = AliyunFileUpdata.DownloadUrl(FID);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            var response = request.GetResponse();
            var stream = response.GetResponseStream();
            return File(stream, "application/octet-stream", FID);
        }

        [HttpPost]
        public JsonResult GetFileRDtoList(FileRelationshipListInput input)
        {
            var data = _fileRelationshipAppService.GetList(input);
            foreach (var item in data)
            {
                item.DownloadUrl = DefuleDomain + "/" + item.QlyName;
                var ext = item.QlyName.Split('.')[1];
                switch (ext)
                {
                    case "doc":
                    case "docx":
                        item.FileUrl = "Images/Word.png";
                        break;
                    case "xls":
                    case "xlsx":
                        item.FileUrl = "Images/Excel.png";
                        break;
                    case "pdf":
                        item.FileUrl = "Images/pdf.png";
                        break;
                    case "mp4":
                        item.FileUrl = "Images/mp4.png";
                        break;
                    default:
                        item.FileUrl = "File/ProcessRequest?FID=" + item.QlyName;
                        break;
                }
            }
            return Json(new { Success = true, Result = data, Message = "操作成功!" });
        }
    }
}