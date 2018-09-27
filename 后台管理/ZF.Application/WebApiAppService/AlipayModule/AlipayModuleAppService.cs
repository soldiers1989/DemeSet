using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using ZF.Application.WebApiAppService.SheetModule;
using ZF.Application.WebApiDto.SheetDtoModule;
using ZF.Core.Entity;
using ZF.Infrastructure.AlipayService;
using ZF.Infrastructure.AlipayService.Business;
using ZF.Infrastructure.AlipayService.Domain;
using ZF.Infrastructure.AlipayService.Model;
using ZF.Infrastructure.Md5;
using System.Drawing;
using ThoughtWorks.QRCode.Codec;
using ZF.Infrastructure.QiniuYun;
using System.IO;
using System.Threading;
using ZF.Application.BaseDto;
using ZF.Infrastructure;
using System.Data;
using Dapper;
using ZF.Application.AppService;
using ZF.Core.IRepository;
using PurchaseDiscountAppService = ZF.Application.WebApiAppService.PurchaseDiscountModule.PurchaseDiscountAppService;
using UserDiscountCardAppService = ZF.Application.WebApiAppService.UserDiscountCardModule.UserDiscountCardAppService;

namespace ZF.Application.WebApiAppService.AlipayModule
{
    /// <summary>
    /// 支付宝服务调用
    /// </summary>
    public class AlipayModuleAppService : BaseAppServer<CourseResource, Guid>
    {

        private readonly UserDiscountCardAppService _discountCardAppService;

        private readonly OrderSheetAppService _orderSheetAppService;

        private readonly PurchaseDiscountAppService _purchaseDiscountAppService;

        //订单学习卡使用记录
        private readonly IOrderCardRepository _iOrderCardRepository;




        private SheetApiService sheetApiService;

        IAlipayTradeService serviceClient = F2FBiz.CreateClientInstance(
            AlipayConfig.URL,
            AlipayConfig.APP_ID,
            AlipayConfig.APP_PRIVATE_KEY,
            AlipayConfig.VERSION,
            AlipayConfig.SIGN_TYPE,
            AlipayConfig.ALIPAY_PUBLIC_KEY,
            AlipayConfig.CHARSET);
        /// <summary>
        /// 单例
        /// </summary>
        /// <param name="_sheetApiService"></param>
        public AlipayModuleAppService(SheetApiService _sheetApiService, UserDiscountCardAppService discountCardAppService, OrderSheetAppService orderSheetAppService, PurchaseDiscountAppService purchaseDiscountAppService, IOrderCardRepository iOrderCardRepository)
        {
            sheetApiService = _sheetApiService;
            _discountCardAppService = discountCardAppService;
            _orderSheetAppService = orderSheetAppService;
            _purchaseDiscountAppService = purchaseDiscountAppService;
            _iOrderCardRepository = iOrderCardRepository;
        }

        #region 当面付

        /// <summary>
        /// 验证SDK
        /// </summary>
        /// <returns></returns>
        public MessagesOutPut GetSDK(SheetModelInput input)
        {
            SDKByService sdk = new SDKByService();
            AlipayTradePrecreateContentBuilder builder = BuildPrecreateContent(input);
            string out_trade_no = builder.out_trade_no;

            //如果需要接收扫码支付异步通知，那么请把下面两行注释代替本行。
            //推荐使用轮询撤销机制，不推荐使用异步通知,避免单边账问题发生。
            AlipayF2FPrecreateResult precreateResult = serviceClient.tradePrecreate(builder);
            //string notify_url = "http://10.5.21.14/notify_url.aspx";  //商户接收异步通知的地址
            //AlipayF2FPrecreateResult precreateResult = serviceClient.tradePrecreate(builder, notify_url);

            //以下返回结果的处理供参考。
            //payResponse.QrCode即二维码对于的链接
            //将链接用二维码工具生成二维码打印出来，顾客可以用支付宝钱包扫码支付。
            string result = "";
            MessagesOutPut messagesoutput = new MessagesOutPut();
            switch (precreateResult.Status)
            {
                case ResultEnum.SUCCESS:
                    string imageName = DoWaitProcess(precreateResult);
                    messagesoutput.Message = imageName;
                    messagesoutput.Success = true;
                    break;
                case ResultEnum.FAILED:
                    result = precreateResult.response.Body;
                    messagesoutput.Message = result;
                    messagesoutput.Success = false;
                    break;

                case ResultEnum.UNKNOWN:
                    if (precreateResult.response == null)
                    {
                        result = "配置或网络异常，请检查后重试";
                    }
                    else
                    {
                        result = "系统异常，请更新外部订单后重新发起请求";
                    }
                    messagesoutput.Message = result;
                    messagesoutput.Success = false;
                    break;
            }
            return messagesoutput;
        }

        /// <summary>
        /// 当面付
        /// 构造支付请求数据
        /// </summary>
        /// <returns>请求数据集</returns>
        private AlipayTradePrecreateContentBuilder BuildPrecreateContent(SheetModelInput input)
        {
            var SheetInfo = sheetApiService.SheetList(input);

            //线上联调时，请输入真实的外部订单号。
            string out_trade_no = "";
            if (String.IsNullOrEmpty(input.OrderNo))
            {
                out_trade_no = System.DateTime.Now.ToString("yyyyMMddHHmmss") + "0000" + (new Random()).Next(1, 10000).ToString();
            }
            else
            {
                out_trade_no = input.OrderNo;
            }

            AlipayTradePrecreateContentBuilder builder = new AlipayTradePrecreateContentBuilder();
            //收款账号
            builder.seller_id = "qsulli4558@sandbox.com";
            //订单编号
            builder.out_trade_no = out_trade_no;
            //订单总金额
            builder.total_amount = input.OrderAmount.ToString();
            //参与优惠计算的金额
            //builder.discountable_amount = "";
            //不参与优惠计算的金额
            //builder.undiscountable_amount = "";
            //订单名称
            builder.subject = "卓帆科技";
            //自定义超时时间
            builder.timeout_express = "5m";
            //订单描述
            builder.body = "";
            //门店编号，很重要的参数，可以用作之后的营销
            builder.store_id = "test store id";
            //操作员编号，很重要的参数，可以用作之后的营销
            builder.operator_id = "test";

            //传入商品信息详情

            builder.goods_detail = GetDetailInfo(SheetInfo);

            //系统商接入可以填此参数用作返佣
            //ExtendParams exParam = new ExtendParams();
            //exParam.sysServiceProviderId = "20880000000000";
            //builder.extendParams = exParam;

            return builder;

        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="precreateResult">二维码串</param>
        private string DoWaitProcess(AlipayF2FPrecreateResult precreateResult)
        {
            //打印出 preResponse.QrCode 对应的条码
            Bitmap bt;
            string enCodeString = precreateResult.response.QrCode;
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
            qrCodeEncoder.QRCodeScale = 3;
            qrCodeEncoder.QRCodeVersion = 8;
            bt = qrCodeEncoder.Encode(enCodeString, Encoding.UTF8);
            string filename = System.DateTime.Now.ToString("yyyyMMddHHmmss") + "0000" + (new Random()).Next(1, 10000).ToString()
             + ".jpg";



            Stream stream = new MemoryStream(BitmapToBytes(bt));
            var isok = AliyunFileUpdata.ResumeUploader(stream, filename);
            //QiniuHelp.FormUploaderByByte(BitmapToBytes(bt), filename);

            //轮询订单结果
            //根据业务需要，选择是否新起线程进行轮询
            ParameterizedThreadStart ParStart = new ParameterizedThreadStart(LoopQuery);
            Thread myThread = new Thread(ParStart);
            object o = precreateResult.response.OutTradeNo;
            myThread.Start(o);

            return filename;
        }
        //图片转byte[]     
        private static byte[] BitmapToBytes(Bitmap Bitmap)
        {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                Bitmap.Save(ms, Bitmap.RawFormat);
                byte[] byteImage = new Byte[ms.Length];
                byteImage = ms.ToArray();
                return byteImage;
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            finally
            {
                ms.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="sheet"></param>
        /// <returns></returns>
        private string GetBizData(SheetModelInput input, List<SheetModelOutput> sheet)
        {
            StringBuilder sbJson = new StringBuilder();

            CartInfo cartInfoList = new CartInfo();
            cartInfoList.out_trade_no = input.OrderNo;
            cartInfoList.seller_id = "2088102175144191";
            cartInfoList.total_amount = Convert.ToDouble(input.OrderAmount);
            cartInfoList.subject = "卓帆科技";
            // cartInfoList.goods_detail = GetDetailInfo(sheet);
            return new JavaScriptSerializer().Serialize(cartInfoList);
        }

        /// <summary>
        /// 明细
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        private List<GoodsInfo> GetDetailInfo(List<SheetModelOutput> sheet)
        {
            List<GoodsInfo> cartdetailinfo = new List<GoodsInfo>();
            foreach (var item in sheet)
            {
                GoodsInfo info = new GoodsInfo()
                {
                    goods_id = Md5.GetMd5(item.CourseId),
                    goods_name = item.CourseName,
                    quantity = item.Num.ToString(),
                    price = item.FavourablePrice.ToString()
                };
                cartdetailinfo.Add(info);
            }
            return cartdetailinfo;
        }

        #endregion


        #region PC-支付

        /// <summary>
        /// PC-在线支付
        /// </summary>
        /// <param name="input"></param>
        /// <param name="userId"></param>
        public AlipayTradePagePayResult BuildPrePageContent(SheetModelInput input, string userId)
        {
            AlipayTradePageContentBuilder builder = BuildPageContent(input, userId);
            string out_trade_no = builder.out_trade_no;
            //如果需要接收扫码支付异步通知，那么请把下面两行注释代替本行。
            //推荐使用轮询撤销机制，不推荐使用异步通知,避免单边账问题发生。
            AlipayTradePagePayResult precreateResult = serviceClient.tradePage(builder);
            //AlipayTradePagePayResult precreateResult = serviceClient.tradePage(builder, AlipayConfig.NOTIFY_URL);

            //添加轮询，处理订单消息
            ParameterizedThreadStart ParStart = new ParameterizedThreadStart(LoopQuery);
            Thread myThread = new Thread(ParStart);
            object o = input.OrderNo;
            myThread.Start(o);

            return precreateResult;
        }

        /// <summary>
        /// PC-构造支付请求数据
        /// </summary>
        /// <returns></returns>
        private AlipayTradePageContentBuilder BuildPageContent(SheetModelInput input, string userId)
        {
            AlipayTradePageContentBuilder alipaytradepagecontentbuilder = new AlipayTradePageContentBuilder()
            {
                out_trade_no = input.OrderNo,
                product_code = "FAST_INSTANT_TRADE_PAY",
                total_amount = Convert.ToDouble(input.OrderAmount),//sheetApiService.GetAmount(input.OrderNo),
                body = "卓帆科技",
                subject = "经济师数字课程"
            };
            var orderSheet = _orderSheetAppService.GetModel(input.OrderNo);
            if (orderSheet == null)
            {
                alipaytradepagecontentbuilder.total_amount = -1;
                return alipaytradepagecontentbuilder;
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
                            var number = item1.Split(',');
                            if (item.CardCode == number[0])
                            {
                                if (int.Parse(number[1]) <= item.Count)
                                {
                                    orderAmount += item.dj * int.Parse(number[1]);
                                }
                                else
                                {
                                    alipaytradepagecontentbuilder.total_amount = -1;
                                    return alipaytradepagecontentbuilder;
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    alipaytradepagecontentbuilder.total_amount = -1;
                    return alipaytradepagecontentbuilder;
                }
                if (orderAmountold - orderAmount != input.OrderAmount)
                {
                    if (orderAmountold - orderAmount < input.OrderAmount)
                    {
                        var arr2 = input.CardNo.TrimEnd(';').Split(';');
                        #region  写入订单学习卡使用表
                        _orderSheetAppService.deleteCard(input.OrderNo);
                        foreach (var cardCode in arr2)
                        {
                            if (!string.IsNullOrEmpty(cardCode))
                            {
                                var number = cardCode.Split(',');
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
                        #endregion
                        alipaytradepagecontentbuilder.total_amount = Convert.ToDouble(input.OrderAmount);
                        return alipaytradepagecontentbuilder;
                    }
                    alipaytradepagecontentbuilder.total_amount = -1;
                    return alipaytradepagecontentbuilder;
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
            alipaytradepagecontentbuilder.total_amount = Convert.ToDouble(orderAmountold - orderAmount);
            return alipaytradepagecontentbuilder;
        }

        /// <summary>
        /// 轮询
        /// </summary>
        /// <param name="o">订单号</param>
        private void LoopQuery(object o)
        {
            AlipayF2FQueryResult queryResult = new AlipayF2FQueryResult();
            int count = 40;
            int interval = 5000;
            string out_trade_no = o.ToString();

            for (int i = 1; i <= count; i++)
            {
                Thread.Sleep(interval);
                queryResult = serviceClient.tradeQuery(out_trade_no);
                if (queryResult != null)
                {
                    if (queryResult.Status == ResultEnum.SUCCESS)
                    {
                        DoSuccessProcess(queryResult);
                        return;
                    }
                }
            }
            DoFailedProcess(queryResult);
        }

        /// <summary>
        /// 请添加支付成功后的处理
        /// </summary>
        private void DoSuccessProcess(AlipayF2FQueryResult queryResult)
        {
            string totalAmount = queryResult.response.TotalAmount;
            //支付成功，修改订单状态
            sheetApiService.EnditSheetState(queryResult.response.OutTradeNo, totalAmount, (int)OrderState.PaymentHasBeen);
        }

        /// <summary>
        /// 请添加支付失败后的处理
        /// </summary>
        private void DoFailedProcess(AlipayF2FQueryResult queryResult)
        {
            //支付失败，删除订单
            AlipayTradeCloseContentBuilder builder = BuildCloseContent(queryResult);
            AlipayTradeClosePayResult precreateResult = serviceClient.tradeClose(builder);

            switch (precreateResult.response.Code)
            {
                //修改订单状态
                case ResultCode.SUCCESS:
                    sheetApiService.EnditSheetState(queryResult.response.OutTradeNo, "0.0", (int)OrderState.PaymentFailure);
                    break;
            }

        }

        /// <summary>
        /// PC-订单失败 BizContent赋值
        /// </summary>
        /// <param name="queryResult"></param>
        /// <returns></returns>
        private AlipayTradeCloseContentBuilder BuildCloseContent(AlipayF2FQueryResult queryResult)
        {
            AlipayTradeCloseContentBuilder alipaytradepagecontentbuilder = new AlipayTradeCloseContentBuilder()
            {
                out_trade_no = queryResult.response.OutTradeNo
            };
            return alipaytradepagecontentbuilder;
        }

        #endregion

    }
}
