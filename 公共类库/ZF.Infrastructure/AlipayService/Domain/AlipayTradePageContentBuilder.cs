using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Infrastructure.AlipayService.Domain
{
    public class AlipayTradePageContentBuilder : JsonBuilder
    {

        /// <summary>
        /// 商户订单号，64个字符以内、可包含字母、数字、下划线；需保证在商户端不重复
        /// </summary>
        public string out_trade_no { get; set; }

        /// <summary>
        /// 销售产品码，与支付宝签约的产品码名称。 注：目前仅支持FAST_INSTANT_TRADE_PAY
        /// </summary>
        public string product_code { get; set; }

        /// <summary>
        /// 订单总金额，单位为元，精确到小数点后两位，取值范围[0.01,100000000]
        /// </summary>
        public double total_amount { get; set; }

        /// <summary>
        /// 订单描述
        /// </summary>
        public string body { get; set; }

        /// <summary>
        /// 订单标题
        /// </summary>
        public string subject { get; set; }

        public AlipayTradePageContentBuilder()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        public override bool Validate()
        {
            throw new NotImplementedException();
        }
    }
}
