using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ThoughtWorks.QRCode.Codec;

namespace ZF.Infrastructure.TwoDimensionalCode
{
    public class QRCodeHelper
    {
        /// <summary>
        /// 会产生graphics异常的PixelFormat
        /// </summary>
        private static PixelFormat[] indexedPixelFormats = { PixelFormat.Undefined, PixelFormat.DontCare,
PixelFormat.Format16bppArgb1555, PixelFormat.Format1bppIndexed, PixelFormat.Format4bppIndexed,
PixelFormat.Format8bppIndexed
        };

        /// <summary>
        /// 生成文字图片
        /// </summary>
        /// <param name="text"></param>
        /// <param name="isBold"></param>
        /// <param name="fontSize"></param>
        public static Bitmap CreateImage(string text, bool isBold, int fontSize)
        {
            int wid = 400;
            int high = 200;
            Font font;

            if (isBold)
            {
                font = new Font("宋体", fontSize, FontStyle.Bold);

            }
            else
            {
                font = new Font("宋体", fontSize, FontStyle.Regular);

            }
            //绘笔颜色
            SolidBrush brush = new SolidBrush(Color.Black);
            StringFormat format = new StringFormat(StringFormatFlags.NoClip);
            format.Alignment = StringAlignment.Far; //右对齐
            format.LineAlignment = StringAlignment.Center;
            Bitmap image = new Bitmap(wid, high);
            Graphics g = Graphics.FromImage(image);

            SizeF sizef = g.MeasureString(text, font, PointF.Empty, format);//得到文本的宽高

            int width = (int)(sizef.Width + 1);
            int height = (int)(sizef.Height + 12);
            image.Dispose();
            image = new Bitmap(width, height);
            g = Graphics.FromImage(image);
            g.Clear(Color.White);//透明

            RectangleF rect = new RectangleF(0, 6, width, height);
            //绘制图片
            g.DrawString(text, font, brush, rect);
            //释放对象
            g.Dispose();
            return image;
        }


        /// <summary>  
        /// 将图片按一定的比例压缩  
        /// </summary>  
        /// <param name="b">资源图片</param>  
        /// <param name="destHeight">高度</param>  
        /// <param name="destWidth">宽度</param>  
        /// <returns></returns>  
        public static Bitmap GetThumbnail(Bitmap b, int destHeight, int destWidth)
        {
            System.Drawing.Image imgSource = b;
            System.Drawing.Imaging.ImageFormat thisFormat = imgSource.RawFormat;
            int sW = 0, sH = 0;
            // 按比例缩放      
            int sWidth = imgSource.Width;
            int sHeight = imgSource.Height;
            if (sHeight > destHeight || sWidth > destWidth)
            {
                if ((sWidth * destHeight) > (sHeight * destWidth))
                {
                    sW = destWidth;
                    sH = (destWidth * sHeight) / sWidth;
                }
                else
                {
                    sH = destHeight;
                    sW = (sWidth * destHeight) / sHeight;
                }
            }
            else
            {
                sW = sWidth;
                sH = sHeight;
            }
            Bitmap outBmp = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage(outBmp);
            g.Clear(Color.Transparent);
            // 设置画布的描绘质量      
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgSource, new Rectangle((destWidth - sW) / 2, (destHeight - sH) / 2, sW, sH), 0, 0, imgSource.Width, imgSource.Height, GraphicsUnit.Pixel);
            g.Dispose();
            // 以下代码为保存图片时，设置压缩质量      
            EncoderParameters encoderParams = new EncoderParameters();
            long[] quality = new long[1];
            quality[0] = 100;
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParams.Param[0] = encoderParam;
            imgSource.Dispose();
            return outBmp;
        }


        /// <summary>  
        /// 生成二维码  
        /// </summary>  
        /// <param name="content">需要生成二维码的内容</param>  
        /// <param name="size">二维码图片长宽大小</param>  
        /// <returns></returns>  
        public static Bitmap Create(string content,int vodeVersion)
        {
            try
            {
                QRCodeEncoder qRCodeEncoder = new QRCodeEncoder();
                qRCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;//设置二维码编码格式 
                qRCodeEncoder.QRCodeScale = 3;//设置编码测量度             
                qRCodeEncoder.QRCodeVersion = vodeVersion;//设置编码版本   
                qRCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;//设置错误校验 

                Bitmap image = qRCodeEncoder.Encode(content,Encoding.UTF8);
                return image;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region 合并用户QR图片和用户头像  

        /// <summary>     
        /// 合并用户QR图片和用户头像     
        /// </summary>     
        /// <param name="qrImg">QR图片（二维码图片）</param>     
        /// <param name="headerImg">用户头像</param>     
        /// <param name="n">缩放比例</param>     
        /// <returns></returns>     
        public static Bitmap MergeQrImg(Bitmap qrImg, Bitmap headerImg, double n = 0.23)
        {
            int margin = 0;
            float dpix = qrImg.HorizontalResolution;
            float dpiy = qrImg.VerticalResolution;
            var _newWidth = (10 * qrImg.Width - 46 * margin) * 1.0f / 46;
            var _headerImg = ZoomPic(headerImg, _newWidth / headerImg.Width);
            //处理头像     
            int newImgWidth = _headerImg.Width + margin;
            Bitmap headerBgImg = new Bitmap(newImgWidth, newImgWidth);
            headerBgImg.MakeTransparent();
            Graphics g = Graphics.FromImage(headerBgImg);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.Clear(Color.Transparent);
            Pen p = new Pen(new SolidBrush(Color.White));
            Rectangle rect = new Rectangle(0, 0, newImgWidth - 1, newImgWidth - 1);
            using (GraphicsPath path = CreateRoundedRectanglePath(rect, 7))
            {
                g.DrawPath(p, path);
                g.FillPath(new SolidBrush(Color.White), path);
            }
            //画头像     
            Bitmap img1 = new Bitmap(_headerImg.Width, _headerImg.Width);
            Graphics g1 = Graphics.FromImage(img1);
            g1.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g1.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g1.Clear(Color.Transparent);
            Pen p1 = new Pen(new SolidBrush(Color.Gray));
            Rectangle rect1 = new Rectangle(0, 0, _headerImg.Width - 1, _headerImg.Width - 1);
            using (GraphicsPath path1 = CreateRoundedRectanglePath(rect1, 7))
            {
                g1.DrawPath(p1, path1);
                TextureBrush brush = new TextureBrush(_headerImg);
                g1.FillPath(brush, path1);
            }
            g1.Dispose();
            PointF center = new PointF((newImgWidth - _headerImg.Width) / 2, (newImgWidth - _headerImg.Height) / 2);
            g.DrawImage(img1, center.X, center.Y, _headerImg.Width, _headerImg.Height);
            g.Dispose();
            Bitmap backgroudImg = new Bitmap(qrImg.Width, qrImg.Height);
            backgroudImg.MakeTransparent();
            backgroudImg.SetResolution(dpix, dpiy);
            headerBgImg.SetResolution(dpix, dpiy);
            Graphics g2 = Graphics.FromImage(backgroudImg);
            g2.Clear(Color.Transparent);
            g2.DrawImage(qrImg, 0, 0);
            PointF center2 = new PointF((qrImg.Width - headerBgImg.Width) / 2, (qrImg.Height - headerBgImg.Height) / 2);
            g2.DrawImage(headerBgImg, center2);
            g2.Dispose();
            return backgroudImg;
        }

        #endregion

        #region 图形处理  

        /// <summary>     
        /// 创建圆角矩形     
        /// </summary>     
        /// <param name="rect">区域</param>     
        /// <param name="cornerRadius">圆角角度</param>     
        /// <returns></returns>     
        private static GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int cornerRadius)
        {
            //下午重新整理下，圆角矩形     
            GraphicsPath roundedRect = new GraphicsPath();
            roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
            roundedRect.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
            roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
            roundedRect.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
            roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
            roundedRect.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2);
            roundedRect.CloseFigure();
            return roundedRect;
        }

        /// <summary>     
        /// 图片按比例缩放     
        /// </summary>     
        private static Image ZoomPic(Image initImage, double n)
        {
            //缩略图宽、高计算     
            double newWidth = initImage.Width;
            double newHeight = initImage.Height;
            newWidth = n * initImage.Width;
            newHeight = n * initImage.Height;
            //生成新图     
            //新建一个bmp图片     
            System.Drawing.Image newImage = new System.Drawing.Bitmap((int)newWidth, (int)newHeight);
            //新建一个画板     
            System.Drawing.Graphics newG = System.Drawing.Graphics.FromImage(newImage);
            //设置质量     
            newG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            newG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //置背景色     
            newG.Clear(Color.Transparent);
            //画图     
            newG.DrawImage(initImage, new System.Drawing.Rectangle(0, 0, newImage.Width, newImage.Height), new System.Drawing.Rectangle(0, 0, initImage.Width, initImage.Height), System.Drawing.GraphicsUnit.Pixel);
            newG.Dispose();
            return newImage;
        }

        #endregion


        /// <summary>  
        /// 合并图片(生成文字图片二维码)  
        /// </summary>  
        /// <param name="imgBack"></param>  
        /// <param name="img"></param>  
        /// <returns></returns>  
        public static Bitmap CombinImage(Bitmap imgBack, Bitmap img)
        {
            if (img.Height != 50 || img.Width != 50)
            {
                img = ResizeImage(img, 60, 30, 0);
            }
            Graphics g = Graphics.FromImage(imgBack);

            g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height); //g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);     

            g.FillRectangle(System.Drawing.Brushes.White, imgBack.Width / 2 - img.Width / 2 - 1, imgBack.Width / 2 - img.Width / 2 - 1, 1, 1);//相片四周刷一层黑色边框    

            //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);    
            PointF center = new PointF((imgBack.Width - img.Width) / 2, (imgBack.Width - img.Height) / 2);
            g.DrawImage(img, center.X + 2, center.Y + 30, img.Width, img.Height);
            GC.Collect();
            return imgBack;
        }


        /// <summary>  
        ///     Resize图片  
        /// </summary>  
        /// <param name="bmp">原始Bitmap</param>  
        /// <param name="newW">新的宽度</param>  
        /// <param name="newH">新的高度</param>  
        /// <param name="mode">保留着，暂时未用</param>  
        /// <returns>处理以后的图片</returns>  
        public static Bitmap ResizeImage(Image bmp, int newW, int newH, int mode)
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);

                // 插值算法的质量    
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //置背景色     
                g.Clear(Color.Transparent);
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height),
                            GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }




        #region 生成带图片Logo
        /// <summary>
        /// 获取本地图片
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Bitmap GetLocalLog(string fileName)
        {
            fileName = HttpContext.Current.Server.MapPath(fileName);
            using (Image img = Image.FromFile(fileName))
            {
                //如果原图片是索引像素格式之列的，则需要转换
                if (IsPixelFormatIndexed(img.PixelFormat))
                {
                    Bitmap bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        g.DrawImage(img, 0, 0);
                    }
                    //下面的水印操作，就直接对 bmp 进行了
                    return bmp;
                }
                else //否则直接操作
                {
                    Bitmap newBmp = new Bitmap(fileName);
                    //Bitmap bmp = new Bitmap(newBmp);
                    return newBmp;
                }
            }
        }

        /// <summary>
        /// 判断图片的PixelFormat 是否在 引发异常的 PixelFormat 之中
        /// </summary>
        /// <param name="imgPixelFormat">原图片的PixelFormat</param>
        /// <returns></returns>
        private static bool IsPixelFormatIndexed(PixelFormat imgPixelFormat)
        {
            foreach (PixelFormat pf in indexedPixelFormats)
            {
                if (pf.Equals(imgPixelFormat)) return true;
            }

            return false;
        }
        #endregion
    }
}
