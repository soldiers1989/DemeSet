using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Newtonsoft.Json;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Application.Dto.SalesReports;
using ZF.Infrastructure.NPOI;
using ZF.Infrastructure.RedisCache;
using ZF.Infrastructure.VFP;

namespace ZF.Web.Controllers
{
    public class OrderSheetController : BaseController
    {
        private readonly OrderSheetAppService _orderSheetAppService;

        public OrderSheetController(OrderSheetAppService orderSheetAppService)
        {
            _orderSheetAppService = orderSheetAppService;
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
        /// <returns></returns>
        public ActionResult AddOrEdit()
        {
            return View();
        }

        /// <summary>
        /// 新增or修改视图
        /// </summary>
        /// <returns></returns>
        public ActionResult PlAddOrEdit()
        {
            return View();
        }

        /// <summary>
        /// 数量选择
        /// </summary>
        /// <returns></returns>
        public ActionResult NumberChoose(string id, string rowIndex, string callback, string number1)
        {
            ViewBag.Id = id;
            ViewBag.Number1 = number1;
            ViewBag.RowIndex = rowIndex;
            ViewBag.Callback = callback;
            return PartialView();
        }

        /// <summary>
        /// 订单详情
        /// </summary>
        /// <returns></returns>
        public ActionResult ToView(string OrderNo)
        {
            ViewBag.OrderNo = OrderNo;
            return PartialView();
        }


        /// <summary>
        /// 销售统计报表
        /// </summary>
        /// <returns></returns>
        public ActionResult SalesReports()
        {
            return PartialView();
        }


        /// <summary>
        /// 销售统计报表 月
        /// </summary>
        /// <returns></returns>
        public ActionResult SalesReportsMonth()
        {
            return PartialView();
        }

        /// <summary>
        /// 销售统计报表 年
        /// </summary>
        /// <returns></returns>
        public ActionResult SalesReportsYear()
        {
            return PartialView();
        }


        /// <summary>
        /// 导出到xlsx  销售统计(日)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public FileResult ExportDay(SalesReportsInput input)
        {
            var list = _orderSheetAppService.GetSalesReportsDayList(input);
            var query = from q in list
                        select new
                        {
                            q.Date,
                            q.TradeCount,
                            q.TotalMoney,
                        };
            var dt = NPOIHelper.ToDataTable(query);
            dt.Columns[0].ColumnName = "日期";
            dt.Columns[1].ColumnName = "笔数";
            dt.Columns[2].ColumnName = "金额";
            var ms = NPOIHelper.ExportDataTable(dt, "销售统计日统计信息");
            ms.Seek(0, SeekOrigin.Begin);// 写入到客户端 
            return File(ms, "application/vnd.ms-excel", "销售统计日统计信息.xls");
        }

        /// <summary>
        /// 导出到xlsx  销售统计(月)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public FileResult ExportMonth(SalesReportsInput input)
        {
            var list = _orderSheetAppService.GetSalesReportsMonthList(input);
            var query = from q in list
                        select new
                        {
                            q.Date,
                            q.TradeCount,
                            q.TotalMoney,
                        };
            var dt = NPOIHelper.ToDataTable(query);
            dt.Columns[0].ColumnName = "月份";
            dt.Columns[1].ColumnName = "笔数";
            dt.Columns[2].ColumnName = "金额";
            var ms = NPOIHelper.ExportDataTable(dt, "销售统计月统计信息");
            ms.Seek(0, SeekOrigin.Begin);// 写入到客户端 
            return File(ms, "application/vnd.ms-excel", "销售统计月统计信息.xls");
        }

        /// <summary>
        /// 导出到xlsx  销售统计(年)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public FileResult ExportYear(SalesReportsInput input)
        {
            var list = _orderSheetAppService.GetSalesReportsYearList(input);
            var query = from q in list
                        select new
                        {
                            q.Date,
                            q.TradeCount,
                            q.TotalMoney,
                        };
            var dt = NPOIHelper.ToDataTable(query);
            dt.Columns[0].ColumnName = "年份";
            dt.Columns[1].ColumnName = "笔数";
            dt.Columns[2].ColumnName = "金额";
            var ms = NPOIHelper.ExportDataTable(dt, "销售统计年统计信息");
            ms.Seek(0, SeekOrigin.Begin);// 写入到客户端 
            return File(ms, "application/vnd.ms-excel", "销售统计年统计信息.xls");
        }

        /// <summary>
        /// 订单电子发票信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ElectronicInvoiceInfo()
        {
            return View();
        }

        /// <summary>
        /// 修改订单价格
        /// </summary>
        /// <returns></returns>
        public ActionResult EnditiOrderPrice()
        {
            return View();
        }

        /// <summary>
        /// 修改应支付金额
        /// </summary>
        /// <returns></returns>
        public ActionResult SetPrice()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Doit()
        {
            foreach (string upload in Request.Files.AllKeys)
            {
                HttpPostedFileBase file = Request.Files[upload];   //file可能为null
                if (file == null || file.ContentLength <= 0)
                {
                    return Json(new MessagesOutPut { Message = "文件不能为空", Success = false });
                }
                var fileName = Path.GetFileName(file.FileName);
                int fileSize = file.ContentLength; //获取上传文件的大小单位为字节byte
                string fileEx = Path.GetExtension(fileName); //获取上传文件的扩展名
                int maxSize = 4000 * 1024; //定义上传文件的最大空间大小为4M
                string fileType = ".xls,.xlsx"; //定义上传文件的类型字符串
                string ext = fileEx;
                fileName = DateTime.Now.ToString("yyyyMMddhhmmss") + "." + ext; // + fileEx;//noFileName +
                if (!fileType.ToUpper().Contains(fileEx.ToUpper()))
                {
                    return Json(new MessagesOutPut { Message = "文件类型不对，只能导入xls和xlsx格式的文件", Success = false });
                }
                if (fileSize >= maxSize)
                {
                    return Json(new MessagesOutPut { Message = "上传文件超过4M，不能上传", Success = false });
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
                    if (fileEx.IndexOf("xls", StringComparison.Ordinal) > -1 || fileEx.IndexOf("xlsx", StringComparison.Ordinal) > -1)
                    {
                        Dictionary<string, int> dict = new Dictionary<string, int>();
                        dict.Add("0", 0); //从第1列开始
                        DataSet ds = NPOIHelper.ImportExcelToDataSet(fs, dict);
                        //判断第1个sheet是否为空字段
                        DataTable dthouseBan = ds.Tables[0]; //获取用户信息

                        DataView dv = new DataView(dthouseBan);
                        if (!dthouseBan.Columns.Contains("手机号码"))
                        {
                            return Json(new MessagesOutPut { Message = "校验失败,上传文件不包含手机号码列!", Success = false });
                        }
                        if (dthouseBan.Columns.Count > 1)
                        {
                            return Json(new MessagesOutPut { Message = "校验失败,上传文件包含多列数据!", Success = false });
                        }
                        if (dv.Count != dv.ToTable(true, "手机号码").Rows.Count)
                        {
                            return Json(new MessagesOutPut { Message = "校验失败,上传文件存在重复数据!", Success = false });
                        }
                        string jsonString = string.Empty;
                        jsonString = JsonConvert.SerializeObject(dthouseBan);

                        return Json(new MessagesOutPut { Message = jsonString, Success = true });
                        // RedisCacheHelper.Add("", jsonString);
                    }
                    fs.Close();
                    //上传完成进行删除文件
                    System.IO.File.Delete(savePath);
                }
                catch (Exception ex)
                {
                    return Json(new MessagesOutPut { Message = "校验失败" + ex.Message, Success = false });
                }

            }
            return Json(new MessagesOutPut { Message = "校验成功!", Success = true });
        }

        /// <summary>
        /// 导出到xlsx
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public FileResult Export(int type)
        {
            var title = "已寄出电子发票导出";
            if (type == 0)
            {
                title = "未寄出电子发票导出";
            }
            var list = _orderSheetAppService.GetElectronicInvoiceInfoByOrderSheet(new OrderSheetListInput { InvoiceState = type });
            var query = from q in list
                        select new
                        {
                            q.OrderNo,
                            q.InvoiceHeader,
                            q.TaxpayerIdentificationNumber,
                            q.InvoiceMailbox,
                            q.InvoicePhone
                        };

            var dt = NPOIHelper.ToDataTable(query);
            dt.Columns[0].ColumnName = "订单号";
            dt.Columns[1].ColumnName = "发票抬头";
            dt.Columns[2].ColumnName = "纳税人识别号";
            dt.Columns[3].ColumnName = "发票邮箱";
            dt.Columns[4].ColumnName = "手机号";
            var ms = NPOIHelper.ExportDataTable(dt, title);
            ms.Seek(0, SeekOrigin.Begin);// 写入到客户端 
            return File(ms, "application/vnd.ms-excel", title + ".xls");
        }

        public ActionResult CourseSaleReport( ) {
            return View( );
        }


        /// <summary>
        /// 导出到xlsx  课程销量统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public FileResult ExportCourseSale( OrderSaleInput input )
        {
            var list = _orderSheetAppService.GetSaleReportByCourse( input );
            var query = from q in list
                        select new
                        {
                            q.CourseName,
                            q.CourseCount,
                        };
            var dt = NPOIHelper.ToDataTable( query );
            dt.Columns[0].ColumnName = "课程名称";
            dt.Columns[1].ColumnName = "购买数";
            var ms = NPOIHelper.ExportDataTable( dt, "课程销量统计信息" );
            ms.Seek( 0, SeekOrigin.Begin );// 写入到客户端 
            return File( ms, "application/vnd.ms-excel", "课程销量统计信息.xls" );
        }
    }
}