using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.WebApiAppService.SheetModule;
using ZF.Application.WebApiDto.SheetDtoModule;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure.AlipayService;
using ZF.Infrastructure.RandomHelper;
using ZF.Infrastructure.TwoDimensionalCode;
using ZF.Infrastructure.WikiService.lib;
using PurchaseDiscountAppService = ZF.Application.WebApiAppService.PurchaseDiscountModule.PurchaseDiscountAppService;
using UserDiscountCardAppService = ZF.Application.WebApiAppService.UserDiscountCardModule.UserDiscountCardAppService;

namespace ZF.Application.WebApiAppService.WxpayModule
{
    /// <summary>
    /// 微信支付服务类
    /// </summary>
    public class WxpayModuleAppService : BaseAppServer<CourseResource, Guid>
    {
        private SheetApiService sheetApiService;

        private readonly UserDiscountCardAppService _discountCardAppService;
        private static string DefuleDomain = ConfigurationManager.AppSettings["DefuleDomain"];

        private readonly OrderSheetAppService _orderSheetAppService;

        //订单学习卡使用记录
        private readonly IOrderCardRepository _iOrderCardRepository;
        private readonly PurchaseDiscountAppService _purchaseDiscountAppService;
        /// <summary>
        /// 单例
        /// </summary>
        /// <param name="_sheetApiService"></param>
        public WxpayModuleAppService(SheetApiService _sheetApiService, OrderSheetAppService orderSheetAppService, UserDiscountCardAppService discountCardAppService, PurchaseDiscountAppService purchaseDiscountAppService, IOrderCardRepository iOrderCardRepository)
        {
            sheetApiService = _sheetApiService;
            _orderSheetAppService = orderSheetAppService;
            _discountCardAppService = discountCardAppService;
            _purchaseDiscountAppService = purchaseDiscountAppService;
            _iOrderCardRepository = iOrderCardRepository;
        }

        /**
       * 生成扫描支付模式一URL
       * @param productId 商品ID
       * @return 模式一URL
       */
        public string GetPrePayUrl(string productId)
        {
            WxPayLog.Info(this.GetType().ToString(), "Native pay mode 1 url is producing...");

            WxPayData data = new WxPayData();
            data.SetValue("mch_id", WxPayConfig.MCHID);//商户号
            data.SetValue("appid", WxPayConfig.APPID);//公众帐号id
            data.SetValue("time_stamp", WxPayApi.GenerateTimeStamp());//时间戳
            data.SetValue("nonce_str", WxPayApi.GenerateNonceStr());//随机字符串
            data.SetValue("product_id", productId);//商品ID
            data.SetValue("sign", data.MakeSign());//签名
            string str = ToUrlParams(data.GetValues());//转换为URL串
            string url = "weixin://wxpay/bizpayurl?" + str;

            WxPayLog.Info(this.GetType().ToString(), "Get native pay mode 1 url : " + url);
            return url;
        }

        /**
        * 生成直接支付url，支付url有效期为2小时,模式二
        * @param productId 商品ID
        * @return 模式二URL
        */
        public MessagesOutPut GetPayUrl(SheetModelInput input, string userId)
        {
            input.OrderAmount = input.OrderAmount * 100;

            var orderSheet = _orderSheetAppService.GetModel(input.OrderNo);
            if (orderSheet == null)
            {
                return new MessagesOutPut { Success = false, Message = null };
            }
            var orderAmountold = (decimal)orderSheet.OrderAmount;
            var orderAmount = new decimal();
            var model = _discountCardAppService.GetUseCard(userId, "", input.OrderNo);
            if (model != null)
            {
                var purchaseDiscount = _purchaseDiscountAppService.GetGetBestDiscountNum(orderSheet.OrderAmount);
                if (purchaseDiscount != null)
                {
                    orderAmount = (decimal)purchaseDiscount.MinusNum;
                }
                try
                {
                    var arr = input.CardNo.TrimEnd(';').Split(';');
                    foreach (var item in model)
                    {
                        foreach (var item1 in arr)
                        {
                            if (!string.IsNullOrEmpty(item1))
                            {
                                var number = item1.Split(',');
                                if (item.CardCode == number[0])
                                {

                                    if (int.Parse(number[1]) <= item.Count)
                                    {
                                        orderAmount += item.dj * int.Parse(number[1]);
                                    }
                                    else
                                    {
                                        return new MessagesOutPut { Success = false, Message = null };
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return new MessagesOutPut { Success = false, Message = null };
                }
                if (orderAmountold - orderAmount != input.OrderAmount / 100)
                {
                    if (orderAmountold - orderAmount < input.OrderAmount / 100)
                    {
                        var arr2 = input.CardNo.TrimEnd(';').Split(';');
                        #region  写入订单学习卡使用表
                        _orderSheetAppService.deleteCard(input.OrderNo);
                        foreach (var cardCode in arr2)
                        {
                            if (!string.IsNullOrEmpty(cardCode))
                            {
                                var number = cardCode.Split(',');
                                var cardModel =_discountCardAppService.GetCardList(userId, number[0]);
                                for (int i = 0; i < cardModel.Count; i++)
                                {
                                    if (int.Parse(number[1]) > i)
                                    {
                                        _iOrderCardRepository.Insert(new OrderCard
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            OrderNo = orderSheet.OrderNo,
                                            CardCode = number[0],
                                            CardId = cardModel[i].Id,
                                            State = 0
                                        });
                                    }
                                }
                            }
                        }
                        #endregion

                    }
                    return new MessagesOutPut { Success = false, Message = null };
                }
            }
            var arr1 = input.CardNo.TrimEnd(';').Split(';');
            #region  写入订单学习卡使用表
            _orderSheetAppService.deleteCard(input.OrderNo);
            foreach (var cardCode in arr1)
            {
                if (!string.IsNullOrEmpty(cardCode))
                {
                    var number = cardCode.Split(',');
                    if (model != null)
                    {
                        var cardModel = _discountCardAppService.GetCardList(userId, number[0]);
                        for (int i = 0; i < cardModel.Count; i++)
                        {
                            if (int.Parse(number[1]) > i)
                            {
                                _iOrderCardRepository.Insert(new OrderCard
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    OrderNo = orderSheet.OrderNo,
                                    CardCode = number[0],
                                    CardId = cardModel[i].Id,
                                    State = 0
                                });
                            }
                        }
                    }
                }
            }
            #endregion


            WxPayLog.Info(this.GetType().ToString(), string.Format("订单{0}开始生成支付码", input.OrderNo));
            WxPayData data = new WxPayData();
            data.SetValue("body", "经济师数字课程");//商品描述
            data.SetValue("attach", input.OrderNo);//附加数据
            data.SetValue("out_trade_no", DateTime.Now.ToString("yyyyMMddHHmmssfff") + RandomHelper.GetRandom(3, 1)[0].ToString());//随机字符串
            data.SetValue("total_fee", Convert.ToInt32(input.OrderAmount));//总金额
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));//交易结束时间
            data.SetValue("goods_tag", input.OrderNo);//商品标记
            data.SetValue("trade_type", "NATIVE");//交易类型
            data.SetValue("product_id", input.OrderNo);//商品ID
            try
            {
                WxPayData result = WxPayApi.UnifiedOrder(data);//调用统一下单接口
                string url = result.GetValue("code_url").ToString();//获得统一下单接口返回的二维码链接
                string twoCode = string.Empty;
                if (!string.IsNullOrEmpty(url))
                {
                    var md5 = Guid.NewGuid().ToString();
                    Bitmap bt;
                    bt = QRCodeHelper.Create(url, 6);
                    Stream stream = new MemoryStream(CreateTwoDimensionalCode.Bitmap2Byte(bt));
                    var isok = AliyunFileUpdata.ResumeUploader(stream, md5 + ".jpg");

                    if (isok)
                    {
                        twoCode = DefuleDomain + "/" + md5 + ".jpg";
                    }
                }

                WxPayLog.Info(this.GetType().ToString(), string.Format("订单{0}的二维码url : ", input.OrderNo) + url);
                return new MessagesOutPut { Success = true, Message = twoCode };
            }
            catch
            {
                return new MessagesOutPut { Success = false, Message = null };
            }
        }

        /**
        * 参数数组转换为url格式
        * @param map 参数名与参数值的映射表
        * @return URL字符串
        */
        private string ToUrlParams(SortedDictionary<string, object> map)
        {
            string buff = "";
            foreach (KeyValuePair<string, object> pair in map)
            {
                buff += pair.Key + "=" + pair.Value + "&";
            }
            buff = buff.Trim('&');
            return buff;
        }

    }
}
