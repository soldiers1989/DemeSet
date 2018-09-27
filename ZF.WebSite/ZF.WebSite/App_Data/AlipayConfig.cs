using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZF.WebSite
{
    /// <summary>
    /// 支付宝SDK配置文件
    /// </summary>
    public class AlipayConfig
    {
        /// <summary>
        /// 沙盒测试网关 https://openapi.alipaydev.com/gateway.do
        /// 正式地址网关 https://openapi.alipay.com/gateway.do
        /// </summary>
        public static string URL
        {
            get;
        } = "https://openapi.alipaydev.com/gateway.do";

        /// <summary>
        /// APPID即创建应用后生成(固定)
        /// </summary>
        public static string APP_ID { get; } = "2016091100486216";

        /// <summary>
        /// 开发者应用私钥，由开发者自己生成
        /// </summary>
        public static string APP_PRIVATE_KEY { get; } = "MIIEpAIBAAKCAQEA2+YPfjs5xvvk1Y7XmMN536MsGQyqdAHiDYa32qRtx+8/M/+10nedvW3qCCp/D7pzd3P2W0B9DQDU4Pz44bt/sKjeRRto8LnOcJItONQSVJeH0iKHvmI1wUWhxpFsIewx7h4t0D9iNykm4VChnVVJM4felImDi5dVuOQYyKMdpTXR4t7zee6LHprN7qM9r1UaCax0bx01Sb+uNsnO77uEAi8M+MUfz/bG/tYIK94bv2lk8LLR1cA/nrlon0FTK0yjhFYQRnyqFnnD9HEg/NmevpXPL3N9UmqpG6xf8Oz2P+81veFgGugoPBdOuhLBoRoB4idWtFydNKF28QUD01tCpwIDAQABAoIBAQCGuuyfMPZzfn50gnKCPLJ9XWJ1bpc5QLYaB4K6SaihqWqWF4R1kahqGeAcZL05C8oXVEuLGXYE84960kRDOjhAKxcUTHj9lTpxKn7epMEeZ0FmdJWrBk1dP6ahHRSdrDPja0Yyo8MvfN0/i8GZvll9Yy/y3lZkvrJc+BRfSeGuCKrShhDv2GOt09/ajAXtx5nR+qTGijTdx+UbEkzD2t0IVyGr/aVl7o6XjkQtDFvxNCwaJOPxqkZsIJm3HcRjmWy02KxKWwCV/o2tKhD50iaQzVsCvLpTlvdscH1z9KaiSiUamzlHlR7M/t9ePfl6brV95wr7rfgxWs0mP7w/pMrBAoGBAPR7XyP1upnzwmn7M2adlNnn2dyDS4xpGm7CUG8qkie62+t2iJJVx9qwpOKo623n7iJ+37PPpulWk15bexF9Pgkf1EMEkGwzZc9m9EgyIRYYUx64FQEtAemjRt5BFVt/o5IQlcEFYeG/I1wX0tZjrh1YhmQ3LzuS4XhwRvzbRFx3AoGBAOZCMnrTUFdj2D1M3BrkhsjjZ2rQS9e5dQSuDFrY6kNoGzSQbsHCOek4EE1JNVwphSVbl/CTd4cGp0WFIEs8AFh7PvEHrmGOxkDbHepZwNuQArthZqOE12tbORVGm7jzukhp9Kx3pyhRltNZRIYVmWNxifBeIq8mH6KwjXSx00dRAoGBAMBgB8gzBsc3kQZ1/MdFPiiNENg8lAkDdyIqUsJ2vBT5Ky3H0sVbLGy/zK7x/nc4JiEMtpg10IAReNqpn1hutY7Wdd7aS45ojzc5KwVYNMK/F0C31wda1AN1UEF48wZRlHNOC5ib1J6fGYLQ2D2MqCg9TVq0Nb1p4XM03hb/rnYXAoGAPyWGXILZNK4wHleWwVVM1Yjv0q4/LnEw81CcGMoGLOg/FcJKbZ7LbPcwGVHrbVQBrMYavCJlFEWx1/Hzck6JNbrO/yEBJMYX/q9Y2+0zY8NxzFug31VOEqYY2IndqPJGcxeDv0ytLfR6LYn8rdz8jPYVQzM9xdTpth8/G/Kso5ECgYAqf/xnz/6KnbCvXSauUVFLRffPtNO4cnuQMTXo3d8OO+JE+bYtQTTrFlcF9OqU3fsbbM1/RbEffKwc8JegT83zFYgDIgAjcnpZPEbVL3GIC7jPRteAVMe0r8bunORFXdCN4m2Q/5lxdmJQHrH8YgieGH7t9YhlD+CVCfj+OavVcg==";

        /// <summary>
        /// 参数返回格式，只支持json
        /// </summary>
        public static string FORMAT { get; } = "json";

        /// <summary>
        /// 请求和签名使用的字符编码格式，支持GBK和UTF-8
        /// </summary>
        public static string CHARSET { get; } = "UTF-8";

        /// <summary>
        /// 支付宝公钥，由支付宝生成
        /// </summary>
        public static string ALIPAY_PUBLIC_KEY { get; } = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwTe9N3tEvlOcBayIukSwJyx2fo5O71OJmAnK8j+A4uRcYol7S/ubczOmjggqenYPVvw2tE04kpkwBHzMZyGuEqosACH2H5wcrN/tUpqp7TMQbWH7NAPsUJvH6I/f41qnUcsvO5QPDHMg65ZaFJivw2Owy73LeKROAt02FJtCvQ+1sBghnBvuaX69ZJtg9dpeFnDRd0CslstHFwhtbtIkgM6lpx0gf+T90UbdsEMVPrmvfiubZ0vTX54ivf9/oW2nIetWTC3T3Ww7BYVz0uADsLe03aF4Rda8ikHf+PWNHx0HKR9lbcdbm1JZrREdgonG9kaioIlJ9JPEjmS7w5wvrwIDAQAB";

        /// <summary>
        /// 商户生成签名字符串所使用的签名算法类型，目前支持RSA2和RSA，推荐使用RSA2
        /// </summary>
        public static string SIGN_TYPE { get; } = "RSA2";

        /// <summary>
        /// 版本
        /// </summary>
        public static string VERSION { get; } = "1.1";

        /// <summary>
        /// 请求参数的集合，最大长度不限，除公共参数外所有请求参数都必须放在这个参数中传递
        /// </summary>
        public static string BIZ_CONTENT { get; set; }

        /// <summary>
        /// 合作伙伴ID：partnerID
        /// </summary>
        public static string PID { get; set; }

        public static string MAPIURL { get; } = "https://mapi.alipay.com/gateway.do";

    }
}