using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using ZF.Application.BaseDto;
using ZF.Infrastructure.QiniuYun;

namespace ZF.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class FileController : ApiController
    {
        private static string DefaultDomain = ConfigurationManager.AppSettings["DefuleDomain"];


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        [HttpPost]
        public void DownloadFile( string filepath ) {
            //var url = QiniuHelp.DownloadUrl( FID );
            //HttpWebRequest request = ( HttpWebRequest )WebRequest.Create( url );
            //var response = request.GetResponse( );
            //var stream = response.GetResponseStream( );
            var strpath = "/File";

            WebClient client = new WebClient( );
            client.DownloadFile( filepath,Path.GetFileName(filepath));
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut UploadFile( ) {
            var httpRequest = HttpContext.Current.Request;
            var files = httpRequest.Form["file"];
            foreach ( string key in httpRequest.Files )  // 文件键
            {
                var postedFile = httpRequest.Files[key];    // 获取文件键对应的文件对象
                if ( postedFile == null || postedFile.ContentLength <= 0 )
                {
                    return new MessagesOutPut { Success = false, Message = "请选择文件" };
                }
                var fileName = Path.GetFileName( postedFile.FileName );
                //int fileSize = postedFile.ContentLength;
                string fileEx = Path.GetExtension( fileName ); //获取上传文件的扩展名
                //int maxSize = 4000 * 1024; //定义上传文件的最大空间大小为4M
                //string fileType = ".xls,.xlsx"; //定义上传文件的类型字符串
                string ext = fileEx;
                fileName = DateTime.Now.ToString( "yyyyMMddhhmmss" )+ ext; // + fileEx;//noFileName +
                //if ( !fileType.ToUpper( ).Contains( fileEx.ToUpper( ) ) )
                //{
                //    return new MessagesOutPut { Message = "文件类型不对，只能导入xls和xlsx格式的文件", Success = false };
                //}
                //if ( fileSize >= maxSize )
                //{
                //    return new MessagesOutPut { Message = "上传文件超过4M，不能导入", Success = false };
                //}
                try
                {
                    string saveFolder = AppDomain.CurrentDomain.BaseDirectory + "Upload/";
                    if ( !Directory.Exists( saveFolder ) )
                    {
                        Directory.CreateDirectory( saveFolder );
                    }
                    var savePath = Path.Combine( saveFolder, fileName );
                    postedFile.SaveAs( savePath );
                    //FileStream fs = new FileStream( savePath, FileMode.Open, FileAccess.Read );
                    //fs.Close( );
                    //System.IO.File.Delete( savePath );
                } catch ( Exception e )
                {
                    return new MessagesOutPut { Message = "上传失败" + e.Message, Success = false };
                }
            }
            
            return new MessagesOutPut { Success = true, Message = "操作成功" };
        }
    }
}
