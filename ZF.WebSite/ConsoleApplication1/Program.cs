using Chatbot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Timers;
using TTS;

namespace ConsoleApplication1
{
    internal class Program
    {
        //Timer不要声明成局部变量，否则会被GC回收
        private static System.Threading.Timer timer1;
        public static string TimestampSince1970=> Convert.ToInt64( ( DateTime.UtcNow - new DateTime( 1970, 1, 1, 0, 0, 0, 0 ) ).TotalSeconds ).ToString( );
        private static ChatRobot bot;
        public static void Main( string[] args )
        {
            //timer1 = new Timer( );
            //timer1.Interval = 2000;
            //timer1.Elapsed += new ElapsedEventHandler( ExecuteAction );
            //timer1.Enabled = true;
            //timer1.AutoReset = true;
            //Console.WriteLine( "按任意键退出程序。" );
            //Console.ReadLine( );

            //var answer = "a";
            //var rightAnswer = "a,b,c";
            //var score= Test.GetScore( answer, rightAnswer );

            //var stu = A.Fake<Student>( x=>x.WithArgumentsForConstructor(()=>new Student()));
            //var stus = A.CollectionOfFake<Student>(10 );

            bot = new ChatRobot( );
            var output = "";

            while ( true )
            {
                var messge = Console.ReadLine( );
                output = bot.getOutput( messge );
                Console.WriteLine( output );
                var soundfile = TTSUtil.XFHecheng( output, AppDomain.CurrentDomain.BaseDirectory );

                SoungPlayer2.PlaySound( soundfile );
                //System.Media.SoundPlayer sp = new System.Media.SoundPlayer( );
                //sp.SoundLocation = soundfile;
                //sp.Play( );

                //科大讯飞语音SDK
                //SpeechSynthesizer synthesizer = new SpeechSynthesizer( );
                //synthesizer.Volume = 100;  // 0...100
                //synthesizer.Rate = -2;     // -10...10

                //// Synchronous
                //synthesizer.Speak( output );
            }
            //var soundfile = AppDomain.CurrentDomain.BaseDirectory + "Vioce\\15380267431427968.mp3";
            //////var soundfile = "D:\\eb661c1f-3072-4ae1-b278-534fd9f0186f.mp3";
            //var longtime = SoungPlayer2.GetSoundLongTime( soundfile);
            //SoungPlayer2.PlaySound( soundfile );

            //SoundPlayer 只能播放wav格式文件
            //System.Media.SoundPlayer sp = new System.Media.SoundPlayer( );
            //sp.SoundLocation = soundfile;
            //sp.Play( );

        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="targetText">待合成的文本</param>
        public static void TTS( string targetText )
        {
            var result = "";
            var url = "http://api.xfyun.cn/v1/service/v1/tts";  
            var appid = "5baae1b5";
            var appkey = "fd5c59864922be9c070fd851b54696a2";
            HttpWebRequest req = ( HttpWebRequest )WebRequest.Create( url );
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
            var param = "{\"aue\":\"raw\",\"auf\":\"audio/L16;rate=16000\",\"voice_name\":\"xiaoyan\",\"speed\":\"50\",\"engine_type\":\"intp65_en\"}";
            var paramBase64 = Convert.ToBase64String( Encoding.UTF8.GetBytes( param));
            
            var checkSum =  Md5.GetMd5( appkey + TimestampSince1970 + paramBase64 ) ;

            req.Headers.Add( "X-CurTime", TimestampSince1970 );
            req.Headers.Add( "X-Param",paramBase64 );
            req.Headers.Add( "X-Appid", appid );
            req.Headers.Add( "X-CheckSum",checkSum );
            req.Headers.Add( "X-Real-Ip", "192.168.2.124" );
            //req.Headers.Add( "Content-Type", "application/x-www-form-urlencoded;" );
            req.Headers.Add( "charset", "utf-8" );
            byte[] data = Encoding.UTF8.GetBytes( "text="+targetText );//需要保留"text="
            req.ContentLength = data.Length;
            using ( Stream reqStream = req.GetRequestStream( ) )
            {
                reqStream.Write( data, 0, data.Length );
                reqStream.Close( );
            }
            
            HttpWebResponse resp;
            try
            {
                resp = ( HttpWebResponse )req.GetResponse( );
            } catch ( WebException ex )
            {
                resp = ( HttpWebResponse )ex.Response;
                throw;
            }

            if ( resp.Headers["Content-Type"] == "audio/mpeg" )
            {
                Console.WriteLine( "合成成功" );
            } else {
                Stream stream = resp.GetResponseStream( );
                //获取响应内容  
                if ( stream != null )
                {
                    using ( var reader = new StreamReader( stream, Encoding.UTF8 ) )
                    {
                        result = reader.ReadToEnd( );
                    }
                }
            }
        }

        public static string StrToHex( string mStr ) //返回处理后的十六进制字符串
        {
            return BitConverter.ToString(
            ASCIIEncoding.Default.GetBytes( mStr ) ).Replace( "-", " " );
        }

        private static void ExecuteAction( object sender, ElapsedEventArgs e )
        {
            Console.WriteLine( "触发的事件发生在： {0}", e.SignalTime );
            var filepath = "D:\\serviceLog.log";
            if ( !File.Exists( filepath ) )
            {
                File.Create( filepath );
            }
            //定时任务执行
            FileStream fs = new FileStream( filepath, FileMode.Append, FileAccess.Write, FileShare.Write );
            StreamWriter sw = new StreamWriter( fs ); // 创建写入流
            sw.WriteLine( DateTime.Now + "：记录日志信息" );
            sw.Close( ); //关闭文件
            fs.Close( );
        }
    }
}