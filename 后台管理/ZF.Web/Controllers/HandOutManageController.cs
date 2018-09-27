using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZF.Application.AppService;
using ZF.Application.Dto;
using ZF.Infrastructure.NPOI;

namespace ZF.Web.Controllers
{
    /// <summary>
    /// 讲义管理
    /// </summary>
    public class HandOutManageController : BaseController
    {
        private readonly DeliveryAddressAppService _deliveryAddressAppService;

        public HandOutManageController(DeliveryAddressAppService deliveryAddressAppService)
        {
            _deliveryAddressAppService = deliveryAddressAppService;
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddOrEdit(string Id)
        {
            ViewBag.gid = Id ?? "";
            return View();
        }


        /// <summary>
        /// 导出到xlsx
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public FileResult Export(int type)
        {

            var title = "已处理讲义导出";
            if (type == 0)
            {
                title = "未处理讲义导出";
            }

            var list = _deliveryAddressAppService.GetList(new DeliveryAddressListInput { SelectType = type });
            var query = from q in list
                        select new
                        {
                            q.OrderNo,
                            q.CourseName,
                            q.Contact,
                            q.ContactPhone,
                            q.Zip,
                            q.Province,
                            q.City,
                            q.Town,
                            q.DetailedAddress,
                            q.Name,
                            q.ExpressNumber
                        };

            var dt = NPOIHelper.ToDataTable(query);
            dt.Columns[0].ColumnName = "订单号";
            dt.Columns[1].ColumnName = "讲义名称";
            dt.Columns[2].ColumnName = "姓名";
            dt.Columns[3].ColumnName = "联系电话";
            dt.Columns[4].ColumnName = "邮编";
            dt.Columns[5].ColumnName = "省";
            dt.Columns[6].ColumnName = "城市";
            dt.Columns[7].ColumnName = "街道,镇";
            dt.Columns[8].ColumnName = "详细地址";
            dt.Columns[9].ColumnName = "快递公司";
            dt.Columns[10].ColumnName = "快递编号";
            var ms = NPOIHelper.ExportDataTable(dt, title);
            ms.Seek(0, SeekOrigin.Begin);// 写入到客户端 
            return File(ms, "application/vnd.ms-excel", title + ".xls");
        }

    }
}