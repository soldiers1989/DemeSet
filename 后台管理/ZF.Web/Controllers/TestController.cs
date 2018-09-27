using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZF.Application.AppService;
using ZF.Application.Dto;
using ZF.Infrastructure;
using ZF.Infrastructure.NPOI;
using ZF.Infrastructure.VFP;

namespace ZF.Web.Controllers
{
    public class TestController : Controller
    {

        private readonly UserAppService _userAppService;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        public TestController(UserAppService userAppService, OperatorLogAppService operatorLogAppService)
        {
            _userAppService = userAppService;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 列表页面视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return PartialView();
        }

        /// <summary>
        /// 新增or修改视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult UserAddOrEdit(string id)
        {
            ViewBag.Id = id ?? "";
            ViewBag.ModuleId = ((int)Model.User).ToString();
            return PartialView();
        }

        /// <summary>
        /// 导入视图
        /// </summary>
        /// <returns></returns>
        public ActionResult ImpRept()
        {
            return View();
        }

        /// <summary>
        /// 导入dbf  或xlsx
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Export()
        {
            var fileNameView = "用户信息导入试例";
            HttpPostedFileBase file = Request.Files["files"];
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
                if (fileEx.IndexOf("xls") >-1 || fileEx.IndexOf("xlsx") > -1)
                {
                    Dictionary<string, int> dict = new Dictionary<string, int>();
                    dict.Add("0", 0); //从第1列开始
                    DataSet ds = NPOIHelper.ImportExcelToDataSet(fs, dict);
                    //判断第1个sheet是否为空字段
                    DataTable dthouseBan = ds.Tables[0]; //获取用户信息
                    var date = dthouseBan.DefaultView;
                    var message = _userAppService.ReptInsert(date);

                    ViewBag.error = message.Message;
                }
                else
                {
                    BaseVFP mBaseVfp = new BaseVFP(savePath);
                    string distinctcount1 = string.Format("select top 1  * from {0} order by 用户名 ", savePath);
                    string distinctcount = string.Format("select distinct * from {0} ", savePath);
                    using (DataTable dt1 = mBaseVfp.SelectInfo(distinctcount1).Tables[0])
                    {
                        if (dt1 != null)
                        {
                            if (!dt1.Columns.Contains("用户名") || !dt1.Columns.Contains("密码"))
                            {
                                ViewBag.error = "导入的数据模板不正确，请确保模板中包含用户名,密码两列";
                            }
                        }
                    }
                    using (DataTable dt = mBaseVfp.SelectInfo(distinctcount).Tables[0])
                    {
                        var message = _userAppService.ReptInsert(dt.DefaultView);
                        ViewBag.error = message.Message;
                    }
                }
                fs.Close();
                //上传完成进行删除文件
                System.IO.File.Delete(savePath);
            }
            catch (Exception ex)
            {
                ViewBag.error = "上传文件失败"+ ex.Message;
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
                fileName = "用户信息导入试例.xlsx";
            }
            else if (type == "dbf")
            {
                fileName = "UserExport.dbf";
            }
            string path = AppDomain.CurrentDomain.BaseDirectory + "ExcelTemplate/";
            return File(path + fileName, "text/plain", fileName);
        }

        /// <summary>
        /// 导出到dbf
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public FileResult ExportDbf(UserInput input)
        {
            MemoryStream be = new MemoryStream();
            try
            {
                var count = 0;
                var list = _userAppService.GetList(input, out count);
                string modulefile = Server.MapPath("/ExcelTemplate/UserExport.dbf");

                string saveDirection = AppDomain.CurrentDomain.BaseDirectory + "/file";
                if (!Directory.Exists(saveDirection))
                {
                    Directory.CreateDirectory(saveDirection);
                }
                string desfile = saveDirection + "/UserExport.dbf";
                {
                    if (list != null && list.Count > 0)
                    {
                        System.IO.File.Copy(modulefile, desfile, true);
                        var firstStr = desfile.Substring(0,
                            desfile.Replace("/", "\\").LastIndexOf("\\", StringComparison.Ordinal));
                        BaseVFP mBaseVfp = new BaseVFP(firstStr);
                        string fileName =
                            desfile.Substring(desfile.Replace("/", "\\").LastIndexOf(@"\", StringComparison.Ordinal) + 1);
                        for (int i = 0; i < list.Count; i++)
                        {
                            string distinctcount1 = string.Format(@"Insert into {0}(用户名,密码) values('{1}','{2}') ",
                                fileName,
                                list[i].UserName,
                                list[i].PassWord);
                            mBaseVfp.InsertInfo(distinctcount1);
                        }
                    }
                }
                FileStream fs = new FileStream(desfile, FileMode.Open, FileAccess.Read);
                int nBytes = (int)fs.Length;//计算流的长度
                byte[] byteArray = new byte[nBytes];//初始化用于MemoryStream的Buffer
                fs.Read(byteArray, 0, nBytes);//将File里的内容一次性的全部读到byteArray中去
                be = new MemoryStream(byteArray);//初始化MemoryStream,并将Buffer指向FileStream的读取结果数组
                fs.Close();
                return File(be, "application/octet-stream", "UserExport.dbf");
            }
            catch (Exception)
            {
                be.Dispose();
                throw;
            }
        }

        /// <summary>
        /// 导出到xlsx
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public FileResult Export(UserInput input)
        {
            var count = 0;
            var list = _userAppService.GetList(input, out count);
            var query = from q in list
                        select new
                        {
                            q.UserName,
                            q.PassWord,
                        };
            var dt = NPOIHelper.ToDataTable(query);
            dt.Columns[0].ColumnName = "用户名";
            dt.Columns[1].ColumnName = "密码";
            var ms = NPOIHelper.ExportDataTable(dt, "用户表导出");
            ms.Seek(0, SeekOrigin.Begin);// 写入到客户端 
            return File(ms, "application/vnd.ms-excel", "用户表导出.xls");
        }
    }
}