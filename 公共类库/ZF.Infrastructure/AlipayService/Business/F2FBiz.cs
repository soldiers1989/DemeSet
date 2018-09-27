using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Infrastructure.AlipayService.Business
{
    /// <summary>
    /// F2FBiz 的摘要说明
    /// </summary>
    public class F2FBiz
    {
        private F2FBiz() { }

        public static IAlipayTradeService serviceClient = null;


        public static IAlipayTradeService CreateClientInstance(string serverUrl, string appId, string merchant_private_key, string version,
                             string sign_type, string alipay_public_key, string charset)
        {



            serviceClient = new AlipayTradeImpl(serverUrl, appId, merchant_private_key, "json", version,
            sign_type, alipay_public_key, charset);

            return serviceClient;
        }

    }
}
