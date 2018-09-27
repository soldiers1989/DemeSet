using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using ZF.Infrastructure.AlipayService;

namespace ZF.Infrastructure.QiniuYun
{
    public class QiniuHelp
    {
        //private static Config config;
        //public static string AccessKey = ConfigurationManager.AppSettings["AccessKey"];
        //public static string SecretKey = ConfigurationManager.AppSettings["SecretKey"];

        //public static string EncryptKey = ConfigurationManager.AppSettings["encryptKey"];
        //// 存储空间名
        //public static string Bucket = ConfigurationManager.AppSettings["Bucket"];

        //// 存储空间名（私有）
        //public static string BucketPrivate = ConfigurationManager.AppSettings["BucketPrivate"];

        ///// <summary>
        ///// 默认域名
        ///// </summary>
        //public static string DefuleDomain = ConfigurationManager.AppSettings["DefuleDomain"];

        ///// <summary>
        ///// 私有空间域名
        ///// </summary>
        //public static string DefuleDomainPrivate = ConfigurationManager.AppSettings["DefuleDomainPrivate"];

        ///// <summary>
        ///// 表单上传文件
        ///// </summary>
        ///// <param name="stream">文件流</param>
        ///// <param name="fileName">存储文件名</param>
        ///// <returns></returns>
        //public static HttpResult FormUploader(Stream stream, string fileName)
        //{
        //    Mac mac = new Mac(AccessKey, SecretKey);
        //    // 设置上传策略，详见：https://developer.qiniu.com/kodo/manual/1206/put-policy
        //    PutPolicy putPolicy = new PutPolicy();
        //    putPolicy.Scope = Bucket;
        //    //putPolicy.SetExpires(3600);
        //    //putPolicy.DeleteAfterDays = 1;
        //    string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
        //    Config config = new Config();
        //    // 设置上传区域
        //    config.Zone = Zone.ZONE_CN_East;
        //    // 设置 http 或者 https 上传
        //    config.UseHttps = true;
        //    config.UseCdnDomains = true;
        //    config.ChunkSize = ChunkUnit.U512K;
        //    // 表单上传
        //    FormUploader target = new FormUploader(config);
        //    HttpResult result = target.UploadStream(stream, fileName, token, null);
        //    return result;
        //}


        ///// <summary>
        ///// 分片上传
        ///// </summary>
        ///// <param name="stream"></param>
        ///// <param name="fileName"></param>
        ///// <returns></returns>
        //public static HttpResult ResumeUploader(Stream stream, string fileName ) {

        //    AliyunFileUpdata.FileUpdate(stream,fileName);
        //    Mac mac = new Mac( AccessKey,SecretKey);
        //    PutPolicy putPolicy = new PutPolicy( );
        //    putPolicy.Scope = Bucket;
        //    string token = Auth.CreateUploadToken( mac,putPolicy.ToJsonString());
        //    //putPolicy.SetExpires(3600);
        //    Config config = new Config( );
        //    config.UseHttps = true;
        //    config.Zone = Zone.ZONE_CN_East;
        //    config.UseCdnDomains = true;
        //    config.ChunkSize = ChunkUnit.U4096K;
        //    //Stream stream = File.OpenRead( fileName );
        //    ResumableUploader target = new ResumableUploader( config );
        //    PutExtra extra = new PutExtra( );
        //    //块并发上传线程数量
        //    extra.BlockUploadThreads = 4;
        //    extra.ResumeRecordFile = ResumeHelper.GetDefaultRecordKey(fileName,EncryptKey );
        //    HttpResult result = target.UploadStream( stream,fileName,token,extra);
        //    return result;
        //}


        ///// <summary>
        ///// 表单上传文件
        ///// </summary>
        ///// <param name="stream">文件流</param>
        ///// <param name="fileName">存储文件名</param>
        ///// <returns></returns>
        //public static HttpResult FormUploaderByByte(byte[] stream, string fileName)
        //{
        //    Mac mac = new Mac(AccessKey, SecretKey);
        //    // 设置上传策略，详见：https://developer.qiniu.com/kodo/manual/1206/put-policy
        //    PutPolicy putPolicy = new PutPolicy();
        //    putPolicy.Scope = Bucket;
        //    //putPolicy.SetExpires(3600);
        //    //putPolicy.DeleteAfterDays = 1;
        //    string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
        //    Config config = new Config();
        //    // 设置上传区域
        //    config.Zone = Zone.ZONE_CN_East;
        //    // 设置 http 或者 https 上传
        //    config.UseHttps = true;
        //    config.UseCdnDomains = true;
        //    config.ChunkSize = ChunkUnit.U512K;
        //    // 表单上传
        //    FormUploader target = new FormUploader(config);
        //    HttpResult result = target.UploadData(stream, fileName, token, null);
        //    return result;
        //}



        ///// <summary>
        ///// 表单上传文件(私有空间)
        ///// </summary>
        ///// <param name="stream">文件流</param>
        ///// <param name="fileName">存储文件名</param>
        ///// <returns></returns>
        //public static HttpResult PrivateFormUploader(Stream stream, string fileName)
        //{
        //    Mac mac = new Mac(AccessKey, SecretKey);
        //    // 设置上传策略，详见：https://developer.qiniu.com/kodo/manual/1206/put-policy
        //    PutPolicy putPolicy = new PutPolicy();
        //    putPolicy.Scope = BucketPrivate;
        //    //putPolicy.SetExpires(3600);
        //    //putPolicy.DeleteAfterDays = 1;
        //    string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
        //    Config config = new Config();
        //    // 设置上传区域
        //    config.Zone = Zone.ZONE_CN_East;
        //    // 设置 http 或者 https 上传
        //    config.UseHttps = true;
        //    config.UseCdnDomains = true;
        //    config.ChunkSize = ChunkUnit.U512K;
        //    // 表单上传
        //    FormUploader target = new FormUploader(config);
        //    HttpResult result = target.UploadStream(stream, fileName, token, null);
        //    return result;
        //}


        ///// <summary>
        ///// 分片上传(私有空间)
        ///// </summary>
        ///// <param name="stream"></param>
        ///// <param name="fileName"></param>
        ///// <returns></returns>
        //public static HttpResult PrivateResumeUploader(Stream stream, string fileName)
        //{
        //    Mac mac = new Mac(AccessKey, SecretKey);
        //    PutPolicy putPolicy = new PutPolicy();
        //    putPolicy.Scope = BucketPrivate;
        //    string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
        //    putPolicy.SetExpires(3600);
        //    Config config = new Config();
        //    config.UseHttps = true;
        //    config.Zone = Zone.ZONE_CN_East;
        //    config.UseCdnDomains = true;
        //    config.ChunkSize = ChunkUnit.U4096K;
        //    //Stream stream = File.OpenRead( fileName );
        //    ResumableUploader target = new ResumableUploader(config);
        //    PutExtra extra = new PutExtra();
        //    //块并发上传线程数量
        //    extra.BlockUploadThreads = 4;
        //    extra.ResumeRecordFile = ResumeHelper.GetDefaultRecordKey(fileName, EncryptKey);
        //    HttpResult result = target.UploadStream(stream, fileName, token, extra);
        //    return result;
        //}


        ///// <summary>
        ///// 表单上传文件(私有空间)
        ///// </summary>
        ///// <param name="stream">文件流</param>
        ///// <param name="fileName">存储文件名</param>
        ///// <returns></returns>
        //public static HttpResult PrivateFormUploaderByByte(byte[] stream, string fileName)
        //{
        //    Mac mac = new Mac(AccessKey, SecretKey);
        //    // 设置上传策略，详见：https://developer.qiniu.com/kodo/manual/1206/put-policy
        //    PutPolicy putPolicy = new PutPolicy();
        //    putPolicy.Scope = BucketPrivate;
        //    //putPolicy.SetExpires(3600);
        //    //putPolicy.DeleteAfterDays = 1;
        //    string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
        //    Config config = new Config();
        //    // 设置上传区域
        //    config.Zone = Zone.ZONE_CN_East;
        //    // 设置 http 或者 https 上传
        //    config.UseHttps = true;
        //    config.UseCdnDomains = true;
        //    config.ChunkSize = ChunkUnit.U512K;
        //    // 表单上传
        //    FormUploader target = new FormUploader(config);
        //    HttpResult result = target.UploadData(stream, fileName, token, null);
        //    return result;
        //}

        ///// <summary>
        ///// 防盗链下载文件链接
        ///// </summary>
        ///// <param name="name">文件名称</param>
        ///// <returns></returns>
        //public static string AntiDaolianGetUrl(string name)
        //{
        //    string host = DefuleDomain;
        //    string fileName = name;
        //    string query = "v=34";
        //    int expireInSeconds = 3600;
        //    //加密密钥
        //    string encryptKey = EncryptKey;
        //    string finalUrl = CdnManager.CreateTimestampAntiLeechUrl(host, fileName, query, encryptKey, expireInSeconds);
        //    return finalUrl;
        //}

        ///// <summary>
        ///// 下载文件链接  公开空间
        ///// </summary>
        ///// <param name="name">文件名称</param>
        ///// <returns></returns>
        //public static string DownloadUrl(string name)
        //{
        //    return DownloadManager.CreatePublishUrl(DefuleDomain, name);
        //}


        ///// <summary>
        ///// 下载文件链接  私有空间
        ///// </summary>
        ///// <param name="name">文件名称</param>
        ///// <returns></returns>
        //public static string DownloadPrivateUrl(string name)
        //{
        //    Mac mac = new Mac(AccessKey, SecretKey);
        //    string privateUrl = DownloadManager.CreatePrivateUrl(mac, DefuleDomainPrivate, name, 60);
        //    return privateUrl;
        //}

        ///// <summary>
        ///// 通过key删除文件
        ///// </summary>
        ///// <param name="name"></param>
        ///// <returns></returns>
        //public static bool Delete(string name)
        //{
        //    Mac mac = new Mac(AccessKey, SecretKey);
        //    Config config = new Config();
        //    config.Zone = Zone.ZONE_CN_East;
        //    BucketManager bucketManager = new BucketManager(mac, config);
        //    HttpResult deleteRet = bucketManager.Delete(Bucket, name);
        //    return deleteRet.Code == (int)HttpCode.OK;
        //}

        ///// <summary>
        ///// 通过key  获取文件信息
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //public static Qiniu.Storage.FileInfo GetFileDate(string key)
        //{
        //    Mac mac = new Mac(AccessKey, SecretKey);
        //    Config config = new Config();
        //    config.Zone = Zone.ZONE_CN_East;
        //    BucketManager bucketManager = new BucketManager(mac, config);
        //    StatResult statRet = bucketManager.Stat(Bucket, key);
        //    if (statRet.Code == (int)HttpCode.OK)
        //    {
        //        return statRet.Result;
        //    }
        //    return null;
        //}


        ///// <summary>
        ///// 批量删除文件
        ///// </summary>
        ///// <param name="keys"></param>
        ///// <returns></returns>
        //public static bool BatchDelete(string[] keys)
        //{
        //    // string[] keys = {
        //    //"00_wo_ts_a.mp4",
        //    //"0327-02.png",
        //    //"0715589972949ea3a.mp4"
        //    //};
        //    Mac mac = new Mac(AccessKey, SecretKey);
        //    Config config = new Config();
        //    config.Zone = Zone.ZONE_CN_East;


        //    List<string> ops = new List<string>();
        //    BucketManager bucketManager = new BucketManager(mac, config);

        //    foreach (string key in keys)
        //    {
        //        string op = bucketManager.DeleteOp(Bucket, key);
        //        ops.Add(op);
        //    }
        //    BatchResult ret = bucketManager.Batch(ops);
        //    if (ret.Code / 100 != 2)
        //    {
        //        return false;
        //    }
        //    foreach (BatchInfo info in ret.Result)
        //    {
        //        return info.Code == (int)HttpCode.OK;
        //    }
        //    return true;
        //}
    }
}