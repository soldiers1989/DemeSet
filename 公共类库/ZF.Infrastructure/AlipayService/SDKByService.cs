using Aliyun.Acs.Core;
using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Infrastructure.AlipayService
{
    public class SDKByService : AlipayConfig
    {


        /// <summary>
        /// 支付宝扫码支付
        /// </summary>
        /// <returns></returns>
        public string GetSDK(string bizContent)
        {

            ////SDK验证
            //IAopClient client = new DefaultAopClient(
            //    URL,
            //    APP_ID,
            //    APP_PRIVATE_KEY,
            //    FORMAT,
            //    VERSION,
            //    SIGN_TYPE,
            //    ALIPAY_PUBLIC_KEY,
            //    CHARSET,
            //    false);
            //AlipayTradeAppPayRequest request = new AlipayTradeAppPayRequest();
            //request.BizContent = bizContent;
            ////回调URL
            //return response.Body;
            return null;
        }
    }
}

