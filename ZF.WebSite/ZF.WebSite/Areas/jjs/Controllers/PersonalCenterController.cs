using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using ZF.WebSite.App_Data;
using ZF.WebSite.Areas.jjs.Common.WxPayAPI;
using ZF.WebSite.Areas.jjs.Controllers;
using ZF.WebSite.Areas.jjs.Models;
using ZF.WebSite.Models;
using ZF.Infrastructure;
using ZF.Infrastructure.RedisCache;
using static ZF.WebSite.Areas.jjs.Controllers.HomeController;

namespace ZF.WebSite.Areas.jjs.Controllers
{
    [CheckLogin(true)]
    public class PersonalCenterController : BaseController
    {

        /// <summary>
        /// 调用js获取收货地址时需要传入的参数
        /// 格式：json串
        /// 包含以下字段：
        ///     appid：公众号id
        ///     scope: 填写“jsapi_address”，获得编辑地址权限
        ///     signType:签名方式，目前仅支持SHA1
        ///     addrSign: 签名，由appid、url、timestamp、noncestr、accesstoken参与签名
        ///     timeStamp：时间戳
        ///     nonceStr: 随机字符串
        /// </summary>
        public static string wxEditAddrParam { get; set; }



        public string JsapiTicket = ConfigurationManager.AppSettings["WechatPayUrl"];





        [HttpPost]
        public string GetJsapiTicketPublic(string findUrl)
        {
            //if (RedisCacheHelper.Exists(findUrl))
            //{
            //    return RedisCacheHelper.Get<string>(findUrl);
            //}
            var jsapi_ticket = WeiXinServiceManager.Api_ticket;
            string timestamp = CommonMethod.ConvertDateTimeInt(DateTime.Now).ToString();//生成签名的时间戳
            string nonceStr = CommonMethod.GetRandCode(16);//生成签名的随机串
            string url = JsapiTicket;//当前的地址

            string[] ArrayList = { "jsapi_ticket=" + jsapi_ticket, "timestamp=" + timestamp, "noncestr=" + nonceStr, "url=" + findUrl };
            Array.Sort(ArrayList);
            string signature = string.Join("&", ArrayList);
            var hashPasswordForStoringInConfigFile = FormsAuthentication.HashPasswordForStoringInConfigFile(signature, "SHA1");
            if (hashPasswordForStoringInConfigFile != null)
                signature = hashPasswordForStoringInConfigFile.ToLower();
            var ticket = "{\"appId\":\"" + ConfigurationManager.AppSettings["WeiXinAccountAppId"] + "\", \"timestamp\":" +
                     timestamp + ",\"nonceStr\":\"" + nonceStr + "\",\"signature\":\"" + signature + "\"}";
            //RedisCacheHelper.Add(findUrl, ticket, new TimeSpan(1, 0, 0));
            return ticket;
        }





        /// <summary>
        /// 个人中心首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 个人信息
        /// </summary>
        /// <returns></returns>
        public ActionResult PersonalInformation()
        {
            return View();
        }

        /// <summary>
        /// 个人中心首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Setting()
        {
            return View();
        }
        /// <summary>
        /// 我的订单
        /// </summary>
        /// <returns></returns>
        public ActionResult MyOrder()
        {
            JsApiPay jsApiPay = new JsApiPay(this);
            try
            {
                #region 微信支付
                //调用【网页授权获取用户信息】接口获取用户的openid和access_token
                jsApiPay.GetOpenidAndAccessToken();
                //获取收货地址js函数入口参数
                wxEditAddrParam = jsApiPay.GetEditAddressParameters();
                if (!string.IsNullOrEmpty(wxEditAddrParam))
                {
                    ViewBag.openid = jsApiPay.openid;
                    ViewBag.wxJsApiParam = wxEditAddrParam;
                }
                #endregion
                //提交订单业务
            }
            catch (Exception ex)
            {
            }
            return View();
        }

        /// <summary>
        /// 订单详情
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderDetail()
        {
            return View();
        }

        /// <summary>
        /// 学习卡
        /// </summary>
        /// <returns></returns>
        public ActionResult MyCard()
        {
            return View();
        }


        /// <summary>
        /// 学习卡 新增
        /// </summary>
        /// <returns></returns>
        public ActionResult MyCardAdd()
        {
            return View();
        }

        /// <summary>
        /// 我的讲义收货地址
        /// </summary>
        /// <returns></returns>
        public ActionResult MyHandOutPath()
        {
            return View();
        }
        /// <summary>
        /// 讲义新增或修改
        /// </summary>
        /// <returns></returns>
        public ActionResult MyHandOutPathEdit(string id)
        {
            ViewBag.handid = id ?? "";
            return View();
        }


        /// <summary>
        /// 我的购物车
        /// </summary>
        /// <returns></returns>
        public ActionResult MyShoppingCart()
        {
            JsApiPay jsApiPay = new JsApiPay(this);
            try
            {
                #region 微信支付
                //调用【网页授权获取用户信息】接口获取用户的openid和access_token
                jsApiPay.GetOpenidAndAccessToken();
                //获取收货地址js函数入口参数
                wxEditAddrParam = jsApiPay.GetEditAddressParameters();
                if (!string.IsNullOrEmpty(wxEditAddrParam))
                {
                    ViewBag.openid = jsApiPay.openid;
                    ViewBag.wxJsApiParam = wxEditAddrParam;
                }
                #endregion
                //提交订单业务
            }
            catch (Exception ex)
            {
            }

            ViewBag.userIp = PublicCommon.GetIpAddress();
            return View();
        }
        /// <summary>
        /// 我的支付
        /// </summary>
        /// <param name="parment"></param>
        /// <returns></returns>
        public ActionResult MyPay(SheetModelInput parment)
        {

            //检测是否给当前页面传递了相关参数
            if (string.IsNullOrEmpty(parment.openid) || string.IsNullOrEmpty(parment.OrderAmount.ToString()))
            {
                ViewBag.parment = "";
                ViewBag.wxJsApiParam = "";
                return View();
            }
            else
            {
                //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
                JsApiPay jsApiPay = new JsApiPay(this);
                jsApiPay.openid = parment.openid;
                jsApiPay.total_fee = Convert.ToInt32((parment.OrderAmount * 100));
                jsApiPay.out_trade_no = parment.OrderNo;
                //JSAPI支付预处理
                try
                {
                    WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult();
                    wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数     
                    ViewBag.wxJsApiParam = wxJsApiParam;
                    //在页面上显示订单信息
                }
                catch (Exception ex)
                {
                }

                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonData = js.Serialize(parment);//序列化
                ViewBag.parment = jsonData;
                return View();
            }


        }

        /// <summary>
        /// 领取增值服务
        /// </summary>
        /// <returns></returns>
        public ActionResult SecurityCodeToCollect()
        {
            return View();
        }

        /// <summary>
        /// 输入防伪码获取课程
        /// </summary>
        /// <returns></returns>
        public ActionResult SecurityTwoCode()
        {
            return View();
        }

        /// <summary>
        /// 输入防伪码获取课程
        /// </summary>
        /// <returns></returns>
        public ActionResult SecurityTwoCode1()
        {
            return View();
        }

        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ModifyPersonal()
        {
            return View();
        }

        /// <summary>
        /// 修改手机号码
        /// </summary>
        /// <returns></returns>
        public ActionResult ModifyPhone()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Advice()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Feedback()
        {
            return View();
        }





        public static string wxJsApiParam { get; set; } //H5调起JS API参数

        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="payinfo"></param>
        /// <returns></returns>
        public JsonResult SubmitOrder(PayInfo payinfo)
        {
            //检测是否给当前页面传递了相关参数
            if (string.IsNullOrEmpty(payinfo.openid) || string.IsNullOrEmpty(payinfo.total_fee))
            {
                Log.Error(this.GetType().ToString(), "This page have not get params, cannot be inited, exit...");
                return null;
            }

            //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
            JsApiPay jsApiPay = new JsApiPay(this);
            jsApiPay.openid = payinfo.openid;
            jsApiPay.total_fee = Convert.ToInt32((payinfo.orderAmount * 100));
            jsApiPay.out_trade_no = payinfo.orderNo;
            //JSAPI支付预处理
            try
            {
                WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult();
                wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数                    
                return Json(wxJsApiParam, JsonRequestBehavior.AllowGet);
                //在页面上显示订单信息
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 入参
        /// </summary>
        [Serializable]
        public class SheetModelInput
        {

            /// <summary>
            /// 明细id
            /// </summary>
            public string Id { get; set; }

            /// <summary>
            /// 订单金额
            /// </summary>
            public decimal OrderAmount { get; set; }

            /// <summary>
            /// 下单ip
            /// </summary>
            public string OrderIp { get; set; }

            /// <summary>
            /// 下单终端
            /// </summary>
            public int OrderType { get; set; }

            /// <summary>
            /// 订单号
            /// </summary>
            public string OrderNo { get; set; }

            /// <summary>
            /// 提交订单明细
            /// </summary>
            public string CarDetailIdList { get; set; }

            /// <summary>
            /// 购物车数量
            /// </summary>
            public int CartCount { get; set; }

            /// <summary>
            /// 学习卡，多个，以'，'给开
            /// </summary>
            public string DiscountCard { get; set; }

            public string openid { get; set; }
        }

        public class SubmitOrderModel
        {
            public string appId { get; set; }
            public string timeStamp { get; set; }
            public string nonceStr { get; set; }
            public string package { get; set; }
            public string signType { get; set; }
            public string paySign { get; set; }
            public string signature { get; set; }
        }
    }
}