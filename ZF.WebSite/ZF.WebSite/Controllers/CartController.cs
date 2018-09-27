using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WxPayAPI;
using ZF.WebSite.Models;

namespace ZF.WebSite.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult Index()
        {
            ViewBag.userIp = PublicCommon.GetWebClientIp();
            return View();
        }
        public ActionResult IframMyOrder()
        {
            return View();
        }


        // GET: Cart
        public ActionResult Indexbak()
        {
            string ss = PublicCommon.GetIP();
            ViewBag.userIp = PublicCommon.GetWebClientIp();
            return View();
        }

        /// <summary>
        /// 未登录时的购物车
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderNoLogin()
        {
            ViewBag.userIp = PublicCommon.GetWebClientIp();
            return View();
        }

        /// <summary>
        /// 提交订单
        /// </summary>
        /// <returns></returns>
        public ActionResult SubmitOrder(SheetModelInput parment)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            if (parment == null)
            {
                string ss = Request.Form["parment"];
                SheetModelInput model = js.Deserialize<SheetModelInput>(Request.Form["parment"]);
                ViewBag.parment = js.Serialize(model);
                ViewBag.cartcount = model.CartCount;
            }
            else
            {
                string jsonData = js.Serialize(parment);//序列化
                ViewBag.parment = jsonData;
                ViewBag.cartcount = parment.CartCount;
            }
            return View();
        }

        public ActionResult IframSubmitOrder(SheetModelInput parment)
        {
            string parments = Request.QueryString["parment"];
            if (string.IsNullOrEmpty(parments))
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                string jsonData = js.Serialize(parment);//序列化
                ViewBag.parment = jsonData;
            }
            else
            {
                ViewBag.parment = parments;
            }

            return View();
        }

        /// <summary>
        /// 我的订单
        /// </summary>
        /// <returns></returns>
        public ActionResult MyOrder()
        {
            return View();
        }

        /// <summary>
        /// 在线支付
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="OrderAmount"></param>
        /// <returns></returns>
        public ActionResult OnlinePayment(string OrderNo, string OrderAmount, int type,string cardNo)
        {
            ViewBag.OrderNo = OrderNo;
            ViewBag.OrderAmount = OrderAmount;
            ViewBag.OrderType = type;
            ViewBag.CardNo = cardNo;
            return View();
        }

        #region MyRegion

        /// <summary>
        /// 讲义收货地址
        /// </summary>
        /// <returns></returns>
        public ActionResult HandOutPath()
        {
            return PartialView();
        }


        /// <summary>
        /// 新增或修改讲义
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        public ActionResult HandOutPathAddOrEnditi(string id)
        {
            ViewBag.handid = id ?? "";
            return PartialView();
        }

        #endregion


        /// <summary>
        /// 个人中心左边菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult CartLeftMouldPlate()
        {
            return View();
        }

        /// <summary>
        /// 阿里支付
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="OrderAmount"></param>
        /// <returns></returns>
        public ActionResult OnlinePaymentAli(SheetModelInput parment)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string jsonData = js.Serialize(parment);//序列化
            ViewBag.parment = jsonData;
            return View();
        }

        /// <summary>
        /// 阿里支付回调页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AliPayResultNotifyPage()
        {
            AliPayCommon aliPayCommon = new AliPayCommon();

            SortedDictionary<string, string> sPara = aliPayCommon.GetRequestPost();

            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Infrastructure.AlipayService.Business.Notify aliNotify = new Infrastructure.AlipayService.Business.Notify(AlipayConfig.CHARSET,
                AlipayConfig.SIGN_TYPE,
                AlipayConfig.PID,
                AlipayConfig.MAPIURL,
                AlipayConfig.ALIPAY_PUBLIC_KEY);

                //对异步通知进行验签
                bool verifyResult = aliNotify.Verify(sPara, Request.Form["notify_id"], Request.Form["sign"]);

                if (verifyResult && aliPayCommon.CheckParams()) //验签成功 && 关键业务参数校验成功
                {

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //请在这里加上商户的业务逻辑程序代码


                    //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                    //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表

                    //商户订单号
                    string out_trade_no = Request.Form["out_trade_no"];


                    //支付宝交易号
                    string trade_no = Request.Form["trade_no"];

                    //交易状态
                    //在支付宝的业务通知中，只有交易通知状态为TRADE_SUCCESS或TRADE_FINISHED时，才是买家付款成功。
                    string trade_status = Request.Form["trade_status"];


                    //判断是否在商户网站中已经做过了这次通知返回的处理
                    //如果没有做过处理，那么执行商户的业务程序
                    //如果有做过处理，那么不执行商户的业务程序

                    Response.Write("success");  //请不要修改或删除

                    //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
                else//验证失败
                {
                    Response.Write("fail");
                }
            }
            return View("/AliPayRerurnUrl");
        }

        /// <summary>
        /// 阿里支付回跳页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AliPayRerurnUrl()
        {
            return View();
        }

        public ActionResult Print()
        {
            return PartialView();
        }

        public string WebApi = ConfigurationManager.AppSettings["WebApi"];
        /// <summary>
        /// 微信支付回调页面
        /// </summary>
        /// <returns></returns>
        public ActionResult WxPayResultNotifyPage()
        {

            WxPayData notifyData = GetNotifyData();
            SheetModelInput info = new SheetModelInput();
            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                ViewBag.isTrue = false;
            }

            string transaction_id = notifyData.GetValue("transaction_id").ToString();

            //查询订单，判断订单真实性
            if (!QueryOrder(transaction_id))
            {
                //若订单查询失败，则立即返回结果给微信支付后台
                ViewBag.isTrue = false;
            }
            //查询订单成功
            else
            {
                var url = WebApi + "api/Account/EnditSheetStates";
                var _weixinApi = new WeiXinApi();
                info.OrderNo = notifyData.GetValue("attach").ToString();
                info.OrderAmount = Convert.ToDecimal(notifyData.GetValue("total_fee")) / 100;
                //微信支付
                info.PayType = "648a1ab9-4575-49d9-bc29-0b5bb5e3006a";
                string input = JsonConvert.SerializeObject(info);
                var ddd = _weixinApi.WikiLogin(url, input);
                Response.Redirect("/LearningCenter/Index");
            }
            return View();
        }

        public bool ProcessNotify()
        {
            WxPayData notifyData = GetNotifyData();
            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                Response.Write(res.ToXml());
                Response.End();
                return false;
            }

            string transaction_id = notifyData.GetValue("transaction_id").ToString();

            //查询订单，判断订单真实性
            if (!QueryOrder(transaction_id))
            {
                //若订单查询失败，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单查询失败");
                Response.Write(res.ToXml());
                Response.End();
                return false;
            }
            //查询订单成功
            else
            {
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "SUCCESS");
                res.SetValue("return_msg", "OK");
                Log.Info(this.GetType().ToString(), "order query success : " + res.ToXml());
                string strXml = res.ToXml();
                Response.Write(res.ToXml());
                Response.End();
                return true;//如果我们走到这一步了，那就代表，用户已经支付成功了，所以，该干嘛干嘛了。
            }
        }

        // <summary>
        /// 接收从微信支付后台发送过来的数据并验证签名
        /// </summary>
        /// <returns>微信支付后台返回的数据</returns>
        public WxPayData GetNotifyData()
        {
            //接收从微信后台POST过来的数据
            System.IO.Stream s = Request.InputStream;
            int count = 0;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while ((count = s.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            s.Flush();
            s.Close();
            s.Dispose();

            Log.Info(this.GetType().ToString(), "Receive data from WeChat : " + builder.ToString());

            //转换数据格式并验证签名
            WxPayData data = new WxPayData();
            try
            {
                data.FromXml(builder.ToString());
            }
            catch (WxPayException ex)
            {
                //若签名错误，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", ex.Message);
                Log.Error(this.GetType().ToString(), "Sign check error : " + res.ToXml());
                return res;
            }

            return data;
        }

        //查询订单
        private bool QueryOrder(string transaction_id)
        {
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transaction_id);
            WxPayData res = WxPayApi.OrderQuery(req);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 订单详情
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderSheetDetail(string OrderNo)
        {
            ViewBag.OrderNo = OrderNo;
            return View();
        }

        /// <summary>
        /// 增值服务
        /// </summary>
        /// <returns></returns>
        public ActionResult SecurityCodeToCollect()
        {
            return View();
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

        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayType { get; set; }

        public string cookieSubmit { get; set; }

        /// <summary>
        /// 推广员编码
        /// </summary>
        public string PromotionCode { get; set; }
    }
}