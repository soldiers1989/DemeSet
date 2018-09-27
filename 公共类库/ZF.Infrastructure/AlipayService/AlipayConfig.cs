using System;
using System.Collections.Generic;

namespace ZF.Infrastructure.AlipayService
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
        } = "https://openapi.alipay.com/gateway.do";

        /// <summary>
        /// APPID即创建应用后生成(固定)
        /// </summary>
        public static string APP_ID { get; } = "2018062460437023";

        /// <summary>
        /// 开发者应用私钥，由开发者自己生成
        /// </summary>
        public static string APP_PRIVATE_KEY { get; } = "MIIEpAIBAAKCAQEA0xks6qm5HDrZGNx80fzM+Fu+wylDAqMMouOow6PD+7a5oLoDZx4534EwDtCxhcvXu10Nv2v6s2whbZ1/l4KfACEN6+KmHH4xxBo9ZNsPlPpsbPlrE6uJYBvSGS+uOnMcwy6Mrf3FX4beXIc7dCP523d0X2t8V/iscn8ylKqzm4EtfRqcQiNS6rnDoAAVq2u5s79ZKm0RH6ZtiuUgq+d4yziaOfjRINh/bA3IqKkXnv1RTz4gakw2SJsECW/VgPQ6ABuxSg45zVHi87dzJmi+FUEC6QyxqFY+AB30VE2BRZWLfV6bihN4gg8b5sp+VkAMpKVSm3OwrH/Grx6/8Xd72wIDAQABAoIBAQCmB4H9Wtn9qwc+94OwwM0RdU83Pge3VjhFERuWLAKjwlaDZGnyu6I2+Ousa1WEbwvREGIUsT+vyJcOGLbWmmSl/FlvsoaFmHdZzm2FGopJ2SBYIV+nS94zWg5HwumcjRcNKFp7KkIHXRMc4TvQn88PXweHWQmmQy6WqxZEOXIkFem2g0b3pXHjMl/GT6JUjYZZWEX9ISzrvgO2S0IKPyyfRFSLTdARWZhyiuQHp8dW3ZhGje08PiKXrixs9Fi7MDe4et0xExJX4FEhHEbf1BKAIP35i2JakWniy4YcZp2Ciphv9GLwaWG7D5JLbIm0T0HDtL5WWgvg89d4vlKAUUSJAoGBAPui6RgMDHGIMvuyi7kJSNNv2xrVF/fPta/Wo58W6d4f03oMXnHloHTd334K5C3I2vzBXFF2oIpxSxnQo0ZrqQvc5gL5YqLcItKCFTSWiDB52wYCUucx8BYTpprTZt7JcZ5gufeRUh+oa9GNQWtSGuXPBvX7XfRDnig1W/mCkDlHAoGBANbCTfAColAuGnnUkzxQr5ZD0abvvHZOWfb4wST6fFS2tD0R/gYOb/XfwmpenZ5Ed0rGPcURTdbfPWtFi1hMteRBwsqt5pcIIsl0gJ212f7ly2yC84W51LGf8d1TXTdTgjN5mkVa8m+Cn2ejLhd6Fen8EmtpBN+aQUmLhGi9yHLNAoGBALlA3koN1Ltggeg7MhIve6Xtm2jNqK+QAzpI0ny06cfVtmML6BwB6XcgQQESE5qBXHboA4cVxmslrRx8NTgK2pEZN1zJLIypdBl2GxZ0HB9UFqL94vrCEPav0N/68qjhtPvcSgywt70GyRmk3Jyd/Z9iIsXFIQ5LGYyQe/3c2UDRAoGARcuCLtV8UNia7CtE6p5CYnCKC1nwgvZ10IkJpG3vyExUiWB2kRtSEbN16MH1McGIj3mcC5bTGFkXCezhG4JavuMMXnhj74PKYnSFdDvsrCWclhh88mHxmQSlQ0vbiKaQMxB7Lb0f+1OVYMYsrZ6UzqWIivXnipAEVdWM48mCAqUCgYBv6zmOR3u54TdQdXA3HiovDW06iWqFq0JAmMltX8KKthYNoa2HrGNWTE6zQZO5HUBiKvxdOdCnc0iG/gF9yTHilr/fgkM2DN59KTHfPNtCohy4Q15vGEKhIqScScGmlYhVV8jw6cIqVM8jZciS/JasZxekpUsTxnrZOzWbcbAYvA==";
         //"MIIEpAIBAAKCAQEA2+YPfjs5xvvk1Y7XmMN536MsGQyqdAHiDYa32qRtx+8/M/+10nedvW3qCCp/D7pzd3P2W0B9DQDU4Pz44bt/sKjeRRto8LnOcJItONQSVJeH0iKHvmI1wUWhxpFsIewx7h4t0D9iNykm4VChnVVJM4felImDi5dVuOQYyKMdpTXR4t7zee6LHprN7qM9r1UaCax0bx01Sb+uNsnO77uEAi8M+MUfz/bG/tYIK94bv2lk8LLR1cA/nrlon0FTK0yjhFYQRnyqFnnD9HEg/NmevpXPL3N9UmqpG6xf8Oz2P+81veFgGugoPBdOuhLBoRoB4idWtFydNKF28QUD01tCpwIDAQABAoIBAQCGuuyfMPZzfn50gnKCPLJ9XWJ1bpc5QLYaB4K6SaihqWqWF4R1kahqGeAcZL05C8oXVEuLGXYE84960kRDOjhAKxcUTHj9lTpxKn7epMEeZ0FmdJWrBk1dP6ahHRSdrDPja0Yyo8MvfN0/i8GZvll9Yy/y3lZkvrJc+BRfSeGuCKrShhDv2GOt09/ajAXtx5nR+qTGijTdx+UbEkzD2t0IVyGr/aVl7o6XjkQtDFvxNCwaJOPxqkZsIJm3HcRjmWy02KxKWwCV/o2tKhD50iaQzVsCvLpTlvdscH1z9KaiSiUamzlHlR7M/t9ePfl6brV95wr7rfgxWs0mP7w/pMrBAoGBAPR7XyP1upnzwmn7M2adlNnn2dyDS4xpGm7CUG8qkie62+t2iJJVx9qwpOKo623n7iJ+37PPpulWk15bexF9Pgkf1EMEkGwzZc9m9EgyIRYYUx64FQEtAemjRt5BFVt/o5IQlcEFYeG/I1wX0tZjrh1YhmQ3LzuS4XhwRvzbRFx3AoGBAOZCMnrTUFdj2D1M3BrkhsjjZ2rQS9e5dQSuDFrY6kNoGzSQbsHCOek4EE1JNVwphSVbl/CTd4cGp0WFIEs8AFh7PvEHrmGOxkDbHepZwNuQArthZqOE12tbORVGm7jzukhp9Kx3pyhRltNZRIYVmWNxifBeIq8mH6KwjXSx00dRAoGBAMBgB8gzBsc3kQZ1/MdFPiiNENg8lAkDdyIqUsJ2vBT5Ky3H0sVbLGy/zK7x/nc4JiEMtpg10IAReNqpn1hutY7Wdd7aS45ojzc5KwVYNMK/F0C31wda1AN1UEF48wZRlHNOC5ib1J6fGYLQ2D2MqCg9TVq0Nb1p4XM03hb/rnYXAoGAPyWGXILZNK4wHleWwVVM1Yjv0q4/LnEw81CcGMoGLOg/FcJKbZ7LbPcwGVHrbVQBrMYavCJlFEWx1/Hzck6JNbrO/yEBJMYX/q9Y2+0zY8NxzFug31VOEqYY2IndqPJGcxeDv0ytLfR6LYn8rdz8jPYVQzM9xdTpth8/G/Kso5ECgYAqf/xnz/6KnbCvXSauUVFLRffPtNO4cnuQMTXo3d8OO+JE+bYtQTTrFlcF9OqU3fsbbM1/RbEffKwc8JegT83zFYgDIgAjcnpZPEbVL3GIC7jPRteAVMe0r8bunORFXdCN4m2Q/5lxdmJQHrH8YgieGH7t9YhlD+CVCfj+OavVcg==";

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
        public static string ALIPAY_PUBLIC_KEY { get; } = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAmC2wdpGbjYL2j/ubve1szGKWfsOvvSahOQsRX8kIoEERVOPlIdQ6nupAx3bYCedWgqUDLiNfsPW23RJgVNkgC2ba8EZ9lUhTPUarFdt2CJqyjl3lt86Ai22t+aW/svREQyBKYei/D7Oga4mXQ7MpBCMbUtFZN4ajX6/hsiKD3AAbuMrTrcTWveK6q7JH+j6/IbK41UqPSFQ6hvfPwxH1tRIa9RN7npudmxlwqWqTCDMf5qL+nHs5a7sSOfSdm0yGfwTQuRmqth0veaKlikMv2M4u6cP+ClwvisK4CuDSKVvVxh07pLAwvhVJwGMJIu/B9d0irt94lJW+xBgUxWJL2wIDAQAB";
                                                        //"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAmC2wdpGbjYL2j/ubve1szGKWfsOvvSahOQsRX8kIoEERVOPlIdQ6nupAx3bYCedWgqUDLiNfsPW23RJgVNkgC2ba8EZ9lUhTPUarFdt2CJqyjl3lt86Ai22t+aW/svREQyBKYei/D7Oga4mXQ7MpBCMbUtFZN4ajX6/hsiKD3AAbuMrTrcTWveK6q7JH+j6/IbK41UqPSFQ6hvfPwxH1tRIa9RN7npudmxlwqWqTCDMf5qL+nHs5a7sSOfSdm0yGfwTQuRmqth0veaKlikMv2M4u6cP+ClwvisK4CuDSKVvVxh07pLAwvhVJwGMJIu/B9d0irt94lJW+xBgUxWJL2wIDAQAB";

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
        /// 回调地址
        /// </summary>
        public static string NOTIFY_URL { get;}= "http://205c8u6006.imwork.net/Cart/AliPayResultNotifyPage";

        /// <summary>
        /// 回跳页
        /// </summary>
        public static string RETURN_URL { get; } = "http://205c8u6006.imwork.net/Cart/MyOrder";
    }

    /// <summary>
    /// 商品信息
    /// </summary>
    [Serializable]
    public class CartInfo
    {
        /// <summary>
        /// 必填
        /// 商户订单号,64个字符以内、只能包含字母、数字、下划线；需保证在商户端不重复
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 可选
        /// 卖家支付宝用户ID。 如果该值为空，则默认为商户签约账号对应的支付宝用户ID
        /// </summary>
        public string seller_id { get; set; }

        /// <summary>
        /// 必填
        /// 订单总金额，单位为元，精确到小数点后两位，取值范围[0.01,100000000] 如果同时传入了【打折金额】，【不可打折金额】，【订单总金额】三者，则必须满足如下条件：【订单总金额】=【打折金额】+【不可打折金额】
        /// </summary>
        public double total_amount { get; set; }

        /// <summary>
        /// 可选
        /// 可打折金额. 参与优惠计算的金额，单位为元，精确到小数点后两位，取值范围[0.01,100000000] 如果该值未传入，但传入了【订单总金额】，【不可打折金额】则该值默认为【订单总金额】-【不可打折金额】
        /// </summary>
        public double discountable_amount { get; set; }

        /// <summary>
        /// 必选
        /// 订单标题
        /// </summary>
        public string subject { get; set; }

        /// <summary>
        /// 订单包含的商品列表信息.Json格式
        /// </summary>
        public List<CartDetailInfo> goods_detail { get; set; }

        /// <summary>
        /// 可选
        /// 对交易或商品的描述
        /// </summary>
        public string body { get; set; }

        /// <summary>
        /// 可选
        /// 商户操作员编号
        /// </summary>
        public string operator_id { get; set; }

        /// <summary>
        /// 可选
        /// 门店编号
        /// </summary>
        public string store_id { get; set; }

        /// <summary>
        /// 可选
        /// 禁用渠道，用户不可用指定渠道支付 当有多个渠道时用“,”分隔注，与enable_pay_channels互斥
        /// </summary>
        public string disable_pay_channels { get; set; }

        /// <summary>
        /// 可选
        /// 可用渠道，用户只能在指定渠道范围内支付 当有多个渠道时用“,”分隔注，与disable_pay_channels互斥
        /// </summary>
        public string enable_pay_channels { get; set; }

        /// <summary>
        /// 可选
        /// 商户机具终端编号
        /// </summary>
        public string terminal_id { get; set; }

        /// <summary>
        /// 可选
        /// 业务扩展参数
        /// </summary>
        public ExtendedBusiness extend_params { get; set; }

        /// <summary>
        /// 可选
        /// 该笔订单允许的最晚付款时间，逾期将关闭交易。取值范围：1m～15d。m-分钟，h-小时，d-天，1c-当天（1c-当天的情况下，无论交易何时创建，都在0点关闭）。 该参数数值不接受小数点， 如 1.5h，可转换为 90m。
        /// </summary>
        public string timeout_express { get; set; }

        /// <summary>
        /// 可选
        /// 商户传入业务信息，具体值要和支付宝约定，应用于安全，营销等参数直传场景，格式为json格式
        /// </summary>
        public string business_params { get; set; }

    }

    /// <summary>
    /// 商品详情
    /// </summary>
    [Serializable]
    public class CartDetailInfo
    {
        /// <summary>
        /// 必填
        /// 商品的编号
        /// </summary>
        public string goods_id { get; set; }

        /// <summary>
        /// 必填
        /// 商品名称
        /// </summary>
        public string goods_name { get; set; }

        /// <summary>
        /// 必填
        /// 商品数量
        /// </summary>
        public int quantity { get; set; }

        /// <summary>
        /// 必填
        /// 商品单价，单位为元
        /// </summary>
        public double price { get; set; }

        /// <summary>
        /// 可选
        /// 商品类目
        /// </summary>
        public string goods_category { get; set; }

        /// <summary>
        /// 可选
        /// 商品描述信息
        /// </summary>
        public string body { get; set; }

        /// <summary>
        /// 可选
        /// 商品的展示地址
        /// </summary>
        public string show_url { get; set; }
    }

    /// <summary>
    /// 扩展业务
    /// </summary>
    [Serializable]
    public class ExtendedBusiness
    {
        /// <summary>
        /// 可选
        /// 系统商编号 该参数作为系统商返佣数据提取的依据，请填写系统商签约协议的PID
        /// </summary>
        public string sys_service_provider_id { get; set; }
    }


}
