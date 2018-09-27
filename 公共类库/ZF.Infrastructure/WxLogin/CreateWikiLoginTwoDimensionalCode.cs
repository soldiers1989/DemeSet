using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZF.Infrastructure.AlipayService;
using ZF.Infrastructure.TwoDimensionalCode;

namespace ZF.Infrastructure.WxLogin
{
    public class CreateWikiLoginTwoDimensionalCode
    {
        private static readonly string redirectRri = ConfigurationManager.AppSettings["WxLoginRedirectUri"];
        private static readonly string appId = ConfigurationManager.AppSettings["WxLoginAppId"];
        private static readonly string returnUrl = ConfigurationManager.AppSettings["WxLoginTeturnUrl"];
        private static readonly string scpoe = ConfigurationManager.AppSettings["WxLoginScpoe"];
        private static readonly string DefuleDomain = ConfigurationManager.AppSettings["DefuleDomain"];

        /// <summary>
        /// 生成登录的二维码
        /// </summary>
        /// <returns></returns>
        public static string WikiLoginTwoDimensionalCode()
        {
            string strGuid = string.Empty;
            string strImgPath = string.Empty;
            string url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri=http%3A%2F%2F{1}%2F{2}&response_type=code&scope={3}&state=STATE#wechat_redirect"
                                 , appId, HttpUtility.UrlEncode(redirectRri), HttpUtility.UrlEncode(returnUrl), scpoe);
            Dictionary<string, string> imgList = new Dictionary<string, string>();
            strGuid = Guid.NewGuid().ToString();
            strImgPath= DefuleDomain + "/" + strGuid + ".jpg";
            Bitmap bt;
            bt = QRCodeHelper.Create(url,20);
            if (AliyunFileUpdata.GetFileDate(strImgPath) > 0)
            {
                AliyunFileUpdata.DeleteFile(strImgPath);
            }
            Stream stream = new MemoryStream(CreateTwoDimensionalCode.Bitmap2Byte(bt));
            var isok = AliyunFileUpdata.ResumeUploader(stream, strGuid + ".jpg");
            if (isok)
            {
                return strImgPath;
            }
            else
            {
                return null;
            }

        }

    }
}
