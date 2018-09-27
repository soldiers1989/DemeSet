using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZF.WebSite.Areas.jjs.Common.WxPayAPI;
using System.Configuration;
using ZF.WebSite.Models;
using static ZF.WebSite.Areas.jjs.Controllers.HomeController;
using System.Web.Security;

namespace ZF.WebSite.Areas.jjs.Controllers
{
    [CheckLogin(true)]
    public class WxPayController : Controller
    {

        public static string wxJsApiParam { get; set; } //H5调起JS API参数
        /// <summary>
        /// 微信支付界面
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public ActionResult Index(PayInfo payinfo)
        {
            //检测是否给当前页面传递了相关参数
            if (string.IsNullOrEmpty(payinfo.openid) || string.IsNullOrEmpty(payinfo.total_fee))
            {
                Response.Write("<span style='color:#FF0000;font-size:20px'>" + "页面传参出错,请返回重试" + "</span>");
                Log.Error(this.GetType().ToString(), "This page have not get params, cannot be inited, exit...");
            }

            //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
            JsApiPay jsApiPay = new JsApiPay(this);
            jsApiPay.openid = payinfo.openid;
            jsApiPay.total_fee = int.Parse("1");
            jsApiPay.out_trade_no = payinfo.orderNo;
            //JSAPI支付预处理
            try
            {
                WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult();
                wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数                    


                var jsapi_ticket = WeiXinServiceManager.GetTickect().ticket;
                string timestamp = CommonMethod.ConvertDateTimeInt(DateTime.Now).ToString();//生成签名的时间戳
                string nonceStr = CommonMethod.GetRandCode(16);//生成签名的随机串

                string[] ArrayList = { "jsapi_ticket=" + jsapi_ticket, "timestamp=" + timestamp, "noncestr=" + nonceStr, "url=" + "http://205c8u6006.imwork.net/jjs/WxPay/Index" };
                Array.Sort(ArrayList);
                string signature = string.Join("&", ArrayList);
                var hashPasswordForStoringInConfigFile = FormsAuthentication.HashPasswordForStoringInConfigFile(signature, "SHA1");
                if (hashPasswordForStoringInConfigFile != null)
                    signature = hashPasswordForStoringInConfigFile.ToLower();

                ViewBag.appId = ConfigurationManager.AppSettings["WeiXinAccountAppId"];
                ViewBag.timeStamp = timestamp;//WxPayApi.GenerateTimeStamp();
                ViewBag.nonceStr = nonceStr;//WxPayApi.GenerateNonceStr();
                ViewBag.package = "prepay_id=" + unifiedOrderResult.GetValue("prepay_id");
                ViewBag.signType = "MD5";
                ViewBag.paySign = unifiedOrderResult.MakeSign();
                ViewBag.signature = signature;

                ViewBag.key = WxPayConfig.KEY;

                //在页面上显示订单信息
            }
            catch (Exception ex)
            {
                Response.Write("<span style='color:#FF0000;font-size:20px'>" + "下单失败，请返回重试" + "</span>");
            }

            return View();
        }

        /// <summary>
        /// 微信支付回调页
        /// </summary>
        /// <returns></returns>
        public ActionResult ResultPayNotify()
        {
            WxPayData notifyData = new Notify(this).GetNotifyData();
            //签名验证失败
            if (notifyData.GetValue("return_code").ToString() == "FAIL")
            {

            }
            else
            {

            }
            return View();
        }
    }


    public class PayInfo
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string orderNo { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public double orderAmount { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public int type { get; set; }
        /// <summary>
        ///  openid用于调用统一下单接口
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 商品金额，用于统一下单
        /// </summary>
        public string total_fee { get; set; }
    }
}