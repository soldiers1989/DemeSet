using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace ZF.Infrastructure.FileHandle
{
    /// <summary>
    /// 文件压缩与解压(支持rar与zip)
    /// 20161011
    /// </summary>
    public static class Compress
    {
        /// <summary>
        /// 是否安装了Winrar
        /// </summary>
        /// <returns></returns>
        static public bool Exists()
        {
            RegistryKey the_Reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe");
            return !string.IsNullOrEmpty(the_Reg.GetValue("").ToString());
        }

        /// <summary>
        /// 压缩成RAR
        /// </summary>
        /// <param name="patch">压缩源文件路径</param>
        /// <param name="rarPatch">压缩包目标路径</param>
        /// <param name="rarName">压缩文件名称</param>
        public static void CompressRAR(string patch, string rarPatch, string rarName)
        {
            string the_rar;
            RegistryKey the_Reg;
            object the_Obj;
            string the_Info;
            ProcessStartInfo the_StartInfo;
            Process the_Process;
            try
            {
                the_Reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe");
                the_Obj = the_Reg.GetValue("");
                the_rar = the_Obj.ToString();
                the_Reg.Close();
                the_rar = the_rar.Substring(1, the_rar.Length - 7);
                Directory.CreateDirectory(patch);
                //命令参数
                the_Info = " a    " + rarName + " " + patch + " -r"; ;
                the_StartInfo = new ProcessStartInfo();
                the_StartInfo.FileName = the_rar;
                the_StartInfo.Arguments = the_Info;
                the_StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                //打包文件存放目录
                the_StartInfo.WorkingDirectory = rarPatch;
                the_Process = new Process();
                the_Process.StartInfo = the_StartInfo;
                the_Process.Start();
                the_Process.WaitForExit();
                the_Process.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 解压RAR
        /// </summary>
        /// <param name="unRarPatch">解压目标路径</param>
        /// <param name="rarPatch">压缩包路径（绝对路径）</param>
        /// <param name="rarName">压缩包名称</param>
        /// <returns></returns>
        public static string UnCompressRAR(string unRarPatch, string rarPatch, string rarName)
        {
            string the_rar;
            RegistryKey the_Reg;
            object the_Obj;
            string the_Info;
            try
            {
                the_Reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe");
                the_Obj = the_Reg.GetValue("");
                the_rar = the_Obj.ToString();
                the_Reg.Close();

                if (Directory.Exists(unRarPatch) == false)
                {
                    Directory.CreateDirectory(unRarPatch);
                }
                the_Info = "x " + rarName + " " + unRarPatch + " -y";

                ProcessStartInfo the_StartInfo = new ProcessStartInfo();
                the_StartInfo.FileName = the_rar;
                the_StartInfo.Arguments = the_Info;
                the_StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                the_StartInfo.WorkingDirectory = rarPatch;//获取压缩包路径

                Process the_Process = new Process();
                the_Process.StartInfo = the_StartInfo;
                the_Process.Start();
                the_Process.WaitForExit();
                the_Process.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return unRarPatch;
        }
    }
}
