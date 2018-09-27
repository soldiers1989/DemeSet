using System.Configuration;
using System.IO;
using Qiniu.Http;
using Qiniu.Storage;
using Qiniu.Util;

namespace ZF.WebSite.App_Data
{
    public class QiniuHelp
    {
        //private static Config config;
        //public static string AccessKey = ConfigurationManager.AppSettings["AccessKey"];
        //public static string SecretKey = ConfigurationManager.AppSettings["SecretKey"];

        //public static string EncryptKey = ConfigurationManager.AppSettings["encryptKey"];
        //// 存储空间名
        //public static string Bucket = ConfigurationManager.AppSettings["Bucket"];

        ///// <summary>
        ///// 默认域名
        ///// </summary>
        //public static string DefuleDomain = ConfigurationManager.AppSettings["DefuleDomainQn"];



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
        //    putPolicy.SetExpires(3600);
        //    putPolicy.DeleteAfterDays = 1;
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

    }
}