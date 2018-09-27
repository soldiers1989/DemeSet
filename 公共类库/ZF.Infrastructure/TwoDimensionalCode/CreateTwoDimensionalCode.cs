using System;
using System.Text;
using System.IO;
using System.Drawing;
using ThoughtWorks.QRCode.Codec;
using ZF.Infrastructure.QiniuYun;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Imaging;
using ZF.Infrastructure.AlipayService;
using System.Web;

namespace ZF.Infrastructure.TwoDimensionalCode
{
    /// <summary>
    /// 默认域名
    /// </summary>
    public class CreateTwoDimensionalCode
    {
        private static string DefuleDomain = ConfigurationManager.AppSettings["DefuleDomain"];

        /// <summary>
        /// 生成二维码并展示到页面上
        /// </summary>
        /// <param name="precreateResult">二维码串</param>
        public static Dictionary<string, string> DoWaitProcess(Dictionary<string, string> precreateResult)
        {
            Bitmap bt;
            Bitmap imgBt;
            Bitmap mergeBt;
            Bitmap logoBt;
            Bitmap hbBt;
            string enCodeString = string.Empty;
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            Dictionary<string, string> returnInfo = null;
            try
            {
                foreach (var item in precreateResult)
                {
                    enCodeString = item.Value;

                    #region 先将文字嵌入到logo
                    //获取logo图片
                    logoBt = QRCodeHelper.GetLocalLog("../../Img/erwemalogoheibai.png");

                    //文字图片
                    imgBt = item.Key.Contains("@") ? QRCodeHelper.CreateImage(item.Key.Split('@')[0], false, 10) : QRCodeHelper.CreateImage(item.Key, false, 10);

                    //将文字图片嵌入到logo图片
                    mergeBt = QRCodeHelper.CombinImage(logoBt, imgBt);
                    #endregion

                    //二维码
                    bt = QRCodeHelper.Create(enCodeString,20);

                    //再将嵌入文字的图片嵌入到二维码图片
                    hbBt = QRCodeHelper.MergeQrImg(bt, mergeBt);

                    string filename = item.Key.Contains("@") ? item.Key.Split('@')[0] + "-" + item.Key.Split('@')[1] + ".jpg" : item.Key + ".jpg";
                    if (AliyunFileUpdata.GetFileDate(filename) > 0)
                    {
                        AliyunFileUpdata.DeleteFile(filename);
                    }

                    Stream stream = new MemoryStream(Bitmap2Byte(hbBt));
                    var isok = AliyunFileUpdata.ResumeUploader(stream, filename);
                    //HttpResult result = QiniuHelp.FormUploaderByByte(Bitmap2Byte(bt), filename);
                    if (isok)
                    {
                        if (returnInfo == null)
                        {
                            returnInfo = new Dictionary<string, string>();
                            returnInfo.Add(item.Key.Contains("@") ? item.Key.Split('@')[0] + "-" + item.Key.Split('@')[1]: item.Key, DefuleDomain + "/" + filename);
                        }
                        else
                        {
                            returnInfo.Add(item.Key.Contains("@") ? item.Key.Split('@')[0] + "-" + item.Key.Split('@')[1]: item.Key, DefuleDomain + "/" + filename);
                        }
                    }
                }
                return returnInfo;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static byte[] Bitmap2Byte(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Jpeg);
                byte[] data = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(data, 0, Convert.ToInt32(stream.Length));
                return data;
            }
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

     

    }

    /// <summary>
    /// 课程二维码属性
    /// </summary>
    public class CourseInfoCode
    {
        public string ImgName { get; set; }

        public string ImgUrl { get; set; }

        public List<VideoCode> videoCode { get; set; }
    }

    /// <summary>
    /// 视频二维码属性
    /// </summary>
    public class VideoCode
    {
        public string VideoCodeName { get; set; }

        public string VideoCodeUrl { get; set; }
    }

    public class ChapterCode
    {
        public string CourseId { get; set; }
        public string ChapterId { get; set; }
        public string VideoName { get; set; }
        public string VideoUrl { get; set; }
        public string Id { get; set; }
        public string CapterName { get; set; }
        public string QcodeTitle { get; set; }
        public string Code { get; set; }
    }
}


   

