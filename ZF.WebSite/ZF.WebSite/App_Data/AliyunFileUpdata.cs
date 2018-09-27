using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;
using Aliyun.OSS;

namespace ZF.WebSite.App_Data
{
    public class AliyunFileUpdata
    {
        static AutoResetEvent _event = new AutoResetEvent(false);
        private static int partSize = 2 * 1024 * 1024;

        public static string AccessKey = ConfigurationManager.AppSettings["AccessKey"];
        public static string SecretKey = ConfigurationManager.AppSettings["SecretKey"];
        public static string Bucket = ConfigurationManager.AppSettings["Bucket"];
        public static string ossendpoint = ConfigurationManager.AppSettings["ossendpoint"];
        public static string DefuleDomain = ConfigurationManager.AppSettings["DefuleDomain"];


        private static OssClient client = new OssClient(ossendpoint, AccessKey, SecretKey);


        private static MemoryStream StreamToMemoryStream(Stream instream)
        {
            MemoryStream outstream = new MemoryStream();
            const int bufferLen = 4096;
            byte[] buffer = new byte[bufferLen];
            int count = 0;
            while ((count = instream.Read(buffer, 0, bufferLen)) > 0)
            {
                outstream.Write(buffer, 0, count);
            }
            return outstream;
        }

        /// <summary>
        /// 分片上传
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool ResumeUploader(Stream fi, string fileName)
        {
            var filename = fileName;
            try
            {
                string bucketName = Bucket;
                string key = filename;

                // 开始Multipart Upload
                var request = new InitiateMultipartUploadRequest(bucketName, key);
                var result = client.InitiateMultipartUpload(request);

                var ms = StreamToMemoryStream(fi);
                ms.Seek(0, SeekOrigin.Begin);
                // 计算片个数
                var fileSize = ms.Length;
                var partCount = fileSize / partSize;
                if (fileSize % partSize != 0)
                {
                    partCount++;
                }

                // 开始分片上传
                try
                {
                    var partETags = new List<PartETag>();
                    for (var i = 0; i < partCount; i++)
                    {
                        var skipBytes = (long)partSize * i;

                        //定位到本次上传片应该开始的位置
                        ms.Seek(skipBytes, 0);

                        //计算本次上传的片大小，最后一片为剩余的数据大小，其余片都是part size大小。
                        var size = (partSize < fileSize - skipBytes) ? partSize : (fileSize - skipBytes);
                        var req = new UploadPartRequest(bucketName, filename, result.UploadId)
                        {
                            InputStream = ms,
                            PartSize = size,
                            PartNumber = i + 1
                        };

                        //调用UploadPart接口执行上传功能，返回结果中包含了这个数据片的ETag值
                        var ret = client.UploadPart(req);
                        partETags.Add(ret.PartETag);
                    }
                    Console.WriteLine("Put multi part upload succeeded : {0}", result.UploadId);
                    try
                    {
                        var completeMultipartUploadRequest = new CompleteMultipartUploadRequest(bucketName, key,
                            result.UploadId);
                        foreach (var partETag in partETags)
                        {
                            completeMultipartUploadRequest.PartETags.Add(partETag);
                        }
                        var ret2 = client.CompleteMultipartUpload(completeMultipartUploadRequest);
                        Console.WriteLine("complete multi part succeeded");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("complete multi part failed, {0}", ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Put multi part upload failed, {0}", ex.Message);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Put multi part upload failed, {0}", ex.Message);
            }
            return false;
        }


        /// <summary>
        /// 通过key  获取文件信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long GetFileDate(string key)
        {
            try
            {
                if (client.GetObject(Bucket, key) != null)
                {
                    return client.GetObject(Bucket, key).Metadata.ContentLength;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return 1;
            }
        }

        /// <summary>
        /// 通过key删除文件
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool Delete(string name)
        {
            try
            {
                client.DeleteObject(Bucket, name);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}