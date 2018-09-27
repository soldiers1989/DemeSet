using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shell32;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleApplication1
{
    public class SoungPlayer2
    {
        public static void PlaySound( string FileName )
        {
            //要加载COM组件:Microsoft speech object Library           
            if ( !System.IO.File.Exists( FileName ) )
            {
                return;
            }
            SpeechLib.SpVoiceClass pp = new SpeechLib.SpVoiceClass( );
            SpeechLib.SpFileStreamClass spFs = new SpeechLib.SpFileStreamClass( );
            spFs.Open( FileName, SpeechLib.SpeechStreamFileMode.SSFMOpenForRead, true );
            SpeechLib.ISpeechBaseStream Istream = spFs as SpeechLib.ISpeechBaseStream;
            pp.SpeakStream( Istream, SpeechLib.SpeechVoiceSpeakFlags.SVSFIsFilename );
            spFs.Close( );
        }

        /// <summary>
        /// 获取音频时长
        /// </summary>
        /// <param name="SongPath"></param>
        /// <returns></returns>
        public static string GetSoundLongTime( string SongPath )
        {
            string dirName = Path.GetDirectoryName( SongPath );
            string SongName = Path.GetFileName( SongPath );//获得歌曲名称
            FileInfo fInfo = new FileInfo( SongPath );
            ShellClass sh = new ShellClass( );
            Folder dir = sh.NameSpace( dirName );
            FolderItem item = dir.ParseName( SongName );
            string SongTime = Regex.Match( dir.GetDetailsOf( item, -1 ), "\\d:\\d{2}:\\d{2}" ).Value;//获取歌曲时间
            return SongTime;
        }
    }
}
