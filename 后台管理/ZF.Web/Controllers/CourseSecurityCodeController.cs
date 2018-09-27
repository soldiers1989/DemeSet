using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZF.Application.AppService;
using ZF.Application.Dto;
using ZF.Infrastructure.NPOI;
using ZF.Infrastructure.VFP;

namespace ZF.Web.Controllers
{
    public class CourseSecurityCodeController : BaseController
    {

        private readonly CourseSecurityCodeAppService _courseSecurityCodeAppService;

        public CourseSecurityCodeController(CourseSecurityCodeAppService courseSecurityCodeAppService)
        {
            _courseSecurityCodeAppService = courseSecurityCodeAppService;
        }


        /// <summary>
        /// 课程防伪码
        /// </summary>
        /// <returns></returns>
        // GET: CourseSecurityCode
        public ActionResult Index(string courseId)
        {
            ViewBag.CourseId = courseId;
            return View();
        }

        /// <summary>
        /// 课程防伪码
        /// </summary>
        /// <returns></returns>
        // GET: CourseSecurityCode
        public ActionResult ImpRept(string courseId)
        {
            ViewBag.CourseId = courseId;
            return View();
        }

        public ActionResult AddOrEdit(string id, string courseId)
        {
            ViewBag.Id = id ?? "";
            ViewBag.CourseId = courseId;
            return PartialView();
        }

        /// <summary>
        /// 导入dbf  或xlsx
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Export()
        {
            var fileNameView = "防伪码导入";
            HttpPostedFileBase file = Request.Files["files"];
            var courseId = Request.Form["CourseId"].ToString();
            if (file == null || file.ContentLength <= 0)
            {
                ViewBag.error = "文件不能为空";
                return View("ImpRept");
            }
            var fileName = Path.GetFileName(file.FileName);
            ViewBag.Name = fileNameView;
            int fileSize = file.ContentLength; //获取上传文件的大小单位为字节byte
            string fileEx = Path.GetExtension(fileName); //获取上传文件的扩展名
            string noFileName = Path.GetFileNameWithoutExtension(fileName); //获取无扩展名的文件名
            int maxSize = 4000 * 1024; //定义上传文件的最大空间大小为4M
            string fileType = ".xls,.xlsx,.dbf"; //定义上传文件的类型字符串
            string ext = fileEx;
            fileName = DateTime.Now.ToString("yyyyMMddhhmmss") + "." + ext; // + fileEx;//noFileName +
            if (!fileType.ToUpper().Contains(fileEx.ToUpper()))
            {
                ViewBag.error = "文件类型不对，只能导入xls和xlsx,dbf格式的文件";
                return View("ImpRept");
            }
            if (fileSize >= maxSize)
            {
                ViewBag.error = "上传文件超过4M，不能上传";
                return View("ImpRept");
            }
            try
            {
                string saveFolder = AppDomain.CurrentDomain.BaseDirectory + "Upload/";
                if (!Directory.Exists(saveFolder))
                {
                    Directory.CreateDirectory(saveFolder);
                }
                var savePath = Path.Combine(saveFolder, fileName);
                file.SaveAs(savePath);
                FileStream fs = new FileStream(savePath, FileMode.Open, FileAccess.Read);
                if (fileEx.IndexOf("xls") > -1 || fileEx.IndexOf("xlsx") > -1)
                {
                    Dictionary<string, int> dict = new Dictionary<string, int>();
                    dict.Add("0", 0); //从第1列开始
                    DataSet ds = NPOIHelper.ImportExcelToDataSet(fs, dict);
                    //判断第1个sheet是否为空字段
                    DataTable dthouseBan = ds.Tables[0]; //获取用户信息
                    var date = dthouseBan.DefaultView;
                    var message = _courseSecurityCodeAppService.ReptInsert(date, courseId);

                    ViewBag.error = message.Message;
                }
                else
                {
                    BaseVFP mBaseVfp = new BaseVFP(savePath);
                    string distinctcount1 = string.Format("select top 1  * from {0} order by 防伪码 ", savePath);
                    string distinctcount = string.Format("select distinct * from {0} ", savePath);
                    using (DataTable dt1 = mBaseVfp.SelectInfo(distinctcount1).Tables[0])
                    {
                        if (dt1 != null)
                        {
                            if (!dt1.Columns.Contains("防伪码")|| !dt1.Columns.Contains("是否增值"))
                            {
                                ViewBag.error = "导入的数据模板不正确，请确保模板中包含防伪码,是否增值";
                            }
                        }
                    }
                    using (DataTable dt = mBaseVfp.SelectInfo(distinctcount).Tables[0])
                    {
                        var message = _courseSecurityCodeAppService.ReptInsert(dt.DefaultView, courseId);
                        ViewBag.error = message.Message;
                    }
                }
                fs.Close();
                //上传完成进行删除文件
                System.IO.File.Delete(savePath);
            }
            catch (Exception ex)
            {
                ViewBag.error = "上传文件失败" + ex.Message;
            }
            return View("ImpRept");
        }

        /// <summary>
        /// 下载模板
        /// </summary>
        /// <returns></returns>
        public FileResult GetFile(string type)
        {
            string fileName = "";
            if (type == "xlsx")
            {
                fileName = "防伪码导入.xlsx";
            }
            else if (type == "dbf")
            {
                fileName = "防伪码导入.dbf";
            }
            string path = AppDomain.CurrentDomain.BaseDirectory + "ExcelTemplate/";
            return File(path + fileName, "text/plain", fileName);
        }
    }
}