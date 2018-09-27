using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace ZF.WebSite
{
    public class AliPayCommon
    {
        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = HttpContext.Current.Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], HttpContext.Current.Request.Form[requestItem[i]]);
            }

            return sArray;
        }

        /// <summary>
        /// 对支付宝异步通知的关键参数进行校验
        /// </summary>
        /// <returns></returns>
        public bool CheckParams()
        {
            bool ret = true;

            //获得商户订单号out_trade_no
            string out_trade_no = HttpContext.Current.Request.Form["out_trade_no"];
            //TODO 商户需要验证该通知数据中的out_trade_no是否为商户系统中创建的订单号，

            //获得支付总金额total_amount
            string total_amount = HttpContext.Current.Request.Form["total_amount"];
            //TODO 判断total_amount是否确实为该订单的实际金额（即商户订单创建时的金额），

            //获得卖家账号seller_email
            string seller_email = HttpContext.Current.Request.Form["seller_email"];
            //TODO 校验通知中的seller_email（或者seller_id) 是否为out_trade_no这笔单据的对应的操作方（有的时候，一个商户可能有多个seller_id / seller_email）

            //获得调用方的appid；
            //如果是非授权模式，appid是商户的appid；如果是授权模式（token调用），appid是系统商的appid
            string app_id = HttpContext.Current.Request.Form["app_id"];
            //TODO 验证app_id是否是调用方的appid；。

            //验证上述四个参数，完全吻合则返回参数校验成功
            return ret;

        }
    }
}