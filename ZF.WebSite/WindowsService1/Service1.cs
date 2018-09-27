using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        System.Timers.Timer timer1;//计时器
        public Service1( )
        {
            InitializeComponent( );
        }

        protected override void OnStart( string[] args )
        {
            timer1 = new System.Timers.Timer( );
            timer1.Interval = 10000;
            timer1.Elapsed += new System.Timers.ElapsedEventHandler( ExecuteAction );
            timer1.Enabled = true;
        }

        private void ExecuteAction( object sender, ElapsedEventArgs e )
        {
            var filepath = "D:\\serviceLog.log";
            //if ( !File.Exists( filepath )){
            //    File.Create( filepath);
            //}
            //定时任务执行
            FileStream fs = new FileStream( filepath, FileMode.Append, FileAccess.Write,FileShare.Write ); 
            StreamWriter sw = new StreamWriter( fs ); // 创建写入流
            sw.WriteLine( DateTime.Now+"：记录日志信息" );
            sw.Close( ); //关闭文件
            fs.Close( );
        }

        protected override void OnStop( )
        {
            timer1.Enabled = false;
        }
    }
}
