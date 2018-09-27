using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TTS
{
   public class TTSUtil
    {

        public static String Md5( string s )
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider( );
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes( s );
            bytes = md5.ComputeHash( bytes );
            md5.Clear( );
            string ret = "";
            for ( int i = 0; i < bytes.Length; i++ )
            {
                ret += Convert.ToString( bytes[i], 16 ).PadLeft( 2, '0' );
            }
            return ret.PadLeft( 32, '0' );
        }

        static string appID = "5baae1b5";
        static string APIKey = "fd5c59864922be9c070fd851b54696a2";
        static String url = "http://api.xfyun.cn/v1/service/v1/tts";



        public  static  string XFHecheng( string text, string pathUrl )
        {
            String voiceLinkUrl = "";

            String method = "POST";

            //String querys = "";
            String bodys;
            //string text = "成功了成功了成功了成功了成功了成功了成功了";
            //对要合成语音的文字先用utf-8然后进行URL加密
            byte[] textData = Encoding.UTF8.GetBytes( text );
            text = HttpUtility.UrlEncode( textData );
            bodys = string.Format( "text={0}", text );

            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;
            //aue = raw, 音频文件保存类型为 wav
            //aue = lame, 音频文件保存类型为 mp3
            string AUE = "raw";
            string param = "{\"aue\":\"" + AUE + "\",\"auf\":\"audio/L16;rate=16000\",\"voice_name\":\"xiaoyan\",\"engine_type\":\"intp65\"}";
            //获取十位的时间戳
            string curTime = ConvertDateTimeToInt( DateTime.Now ).ToString( );
            //对参数先utf-8然后用base64编码
            byte[] paramData = Encoding.UTF8.GetBytes( param );
            string paraBase64 = Convert.ToBase64String( paramData );
            //形成签名
            string checkSum = Md5( APIKey + curTime + paraBase64 );
            httpRequest = ( HttpWebRequest )WebRequest.Create( url );
            httpRequest.Method = method;
            httpRequest.Headers.Add( "X-Param", paraBase64 );
            httpRequest.Headers.Add( "X-CurTime", curTime );
            httpRequest.Headers.Add( "X-Appid", appID );
            httpRequest.Headers.Add( "X-CheckSum", checkSum );
            string ip = "127.0.0.1";//设置白名单中的一致此处默认为127.0.0.1
            httpRequest.Headers.Add( "X-Real-Ip", "127.0.0.1" );
            //根据API的要求，定义相对应的Content-Type
            httpRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            if ( 0 < bodys.Length )
            {
                byte[] data = Encoding.UTF8.GetBytes( bodys );
                using ( Stream stream = httpRequest.GetRequestStream( ) )
                {
                    stream.Write( data, 0, data.Length );
                }
            }
            try
            {
                httpResponse = ( HttpWebResponse )httpRequest.GetResponse( );
            } catch ( WebException ex )
            {
                httpResponse = ( HttpWebResponse )ex.Response;
            }

            if ( httpResponse.StatusCode != HttpStatusCode.OK )
            {
                Stream st = httpResponse.GetResponseStream( );
                StreamReader reader = new StreamReader( st, Encoding.GetEncoding( "utf-8" ) );
            } else
            {
                Stream st = httpResponse.GetResponseStream( );

                //StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                //string strValue = reader.ReadToEnd();   //从头读到尾，如果生成文件无效的话把这句代码放开看下原因
                String voiceUrl = AppDomain.CurrentDomain.BaseDirectory + "/Vioce";

                if ( !Directory.Exists( voiceUrl ) )
                {
                    Directory.CreateDirectory( voiceUrl );
                }
                //用13位时间戳形成文件名
                string voice = string.Format( "{0}.wav", ConvertDateTimeToInt( DateTime.Now, 1 ).ToString( ) );

                //合成网络路径
                voiceLinkUrl = pathUrl + "Vioce\\" + voice;
                //合成本地路径
                voice = voiceUrl + "/" + voice;
                //数据流转换成缓存流
                MemoryStream memoryStream = StreamToMemoryStream( st );
                //写入文件
                File.WriteAllBytes( voice, streamTobyte( memoryStream ) );

            }
            return voiceLinkUrl;
        }


        #region 把流转换成缓存流
        static MemoryStream StreamToMemoryStream( Stream instream )
        {
            MemoryStream outstream = new MemoryStream( );
            const int bufferLen = 4096;
            byte[] buffer = new byte[bufferLen];
            int count = 0;
            while ( ( count = instream.Read( buffer, 0, bufferLen ) ) > 0 )
            {
                outstream.Write( buffer, 0, count );
            }
            return outstream;
        }
        #endregion

        #region 把缓存流转换成字节组
        public static byte[] streamTobyte( MemoryStream memoryStream )
        {
            byte[] buffer = new byte[memoryStream.Length];
            memoryStream.Seek( 0, SeekOrigin.Begin );
            memoryStream.Read( buffer, 0, buffer.Length );
            return buffer;
        }
        #endregion

        #region 将c# DateTime时间格式转换为Unix时间戳格式
        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <param name="mark">0：生成10位的时间戳 1:生成13位的时间戳</param>  
        /// <returns>long</returns>  
        public static long ConvertDateTimeToInt( DateTime time, int mark = 0 )
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime( new DateTime( 1970, 1, 1, 0, 0, 0, 0 ) );
            long t = 0;
            if ( mark != 0 )
            {
                t = ( time.Ticks - startTime.Ticks );
            } else
            {
                t = ( time.Ticks - startTime.Ticks ) / 10000000; //除10000调整为13位  ，除10000000调整为10位    
            }
            return t;
        }

        #endregion
    }
}
