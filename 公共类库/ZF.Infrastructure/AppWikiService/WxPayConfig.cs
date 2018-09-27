﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Infrastructure.AppWikiService
{
    /**
    * 	配置账号信息
    */
    public class WxPayConfig
    {
        //=======【基本信息设置】=====================================
        /* 微信公众号信息配置
        * APPID：绑定支付的APPID（必须配置）
        * MCHID：商户号（必须配置）
        * KEY：商户支付密钥，参考开户邮件设置（必须配置）
        * APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置）
        */
        public const string APPID = "wx37d2ac10a0eb03c6";
        public const string MCHID = "1507727881";
        public const string KEY = "baizqvr5osrq6g2xxi5o1p9wvzvak087";
        public const string APPSECRET = "ba6a172fdb04e51e9da4605ed3e2a720";

        //=======【证书路径设置】===================================== 
        /* 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        */
        public const string SSLCERT_PATH = "cert/apiclient_cert.p12";
        public const string SSLCERT_PASSWORD = "1507727881";



        //=======【支付结果通知url】===================================== 
        /* 支付结果通知回调url，用于商户接收支付结果
        */
        public const string NOTIFY_URL = "http://zgrskspx.class.com.cn/jjs/PersonalCenter/MyOrder";

        //=======【商户系统后台机器IP】===================================== 
        /* 此参数可手动配置也可在程序中自动获取
        */
        public const string IP = "113.246.85.236";


        //=======【代理服务器设置】===================================
        /* 默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）
        */
        public const string PROXY_URL = "http://10.152.18.220:8080";

        //=======【上报信息配置】===================================
        /* 测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
        */
        public const int REPORT_LEVENL = 1;

        //=======【日志级别】===================================
        /* 日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
        */
        public const int LOG_LEVENL = 0;
    }

    /// <summary>
    /// 移动端微信config
    /// </summary>
    public class ToAppConfig
    {
        //=======【基本信息设置】=====================================
        /* 微信公众号信息配置
        * appid：绑定支付的APPID
        * mch_id：商户号（必须配置）
        * KEY：商户支付密钥，参考开户邮件设置（必须配置）
        * APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置）
        */
        public const string appid = "wxa3c4dbfb94279de6";

        public const string mch_id = "1509758921";

        public const string appkey = "KGUsualN2NSegTZUxXyTHYJTqAs1RJI4";

        public const string secret = "ef817131cd53ce84f23a89578ae9cc9d";
        /// <summary>
        /// 随机字符串，不长于32位。推荐随机数生成算法
        /// </summary>
        public string nonce_str { get; set; }
        /// <summary>
        /// APP——需传入应用市场上的APP名字-实际商品名称，天天爱消除-游戏充值。
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public int total_fee { get; set; }

        /// <summary>
        /// 用户端实际ip
        /// </summary>
        public string spbill_create_ip { get; set; }
        /// <summary>
        /// 接收微信支付异步通知回调地址，通知url必须为直接可访问的url，不能携带参数。
        /// </summary>
        public string notify_url { get; set; }

        public string trade_type { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }

        public string key { get; set; }
        /// <summary>
        ///  // 必填，生成签名的时间戳
        /// </summary>
        public string timestamp { get; set; }

    }
}
