using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using System;
using System.Configuration;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Sts.Model.V20150401;
using Aliyun.Acs.vod.Model.V20170321;
using ZF.Infrastructure.AlipayService.Model;

namespace ZF.Infrastructure.AlipayService
{
    public static class Aliyun
    {
        public static string AccessKey = ConfigurationManager.AppSettings["AccessKey"];
        public static string SecretKey = ConfigurationManager.AppSettings["SecretKey"];
        public static string REGIONID = ConfigurationManager.AppSettings["REGIONID"];
        public static string ENDPOINT = ConfigurationManager.AppSettings["ENDPOINT"];
        public static string RoleArn = ConfigurationManager.AppSettings["RoleArn"];
        public static string RoleSessionName = ConfigurationManager.AppSettings["RoleSessionName"];

        static IClientProfile clientProfile = DefaultProfile.GetProfile(REGIONID, AccessKey, SecretKey);
        static DefaultAcsClient client = new DefaultAcsClient(clientProfile);

        /// <summary>
        /// 通过视频编号VideoId  获取阿里云的视频签名
        /// </summary>
        public static VideoModel GetVideoPlayAuth(string videoId)
        {
            GetVideoPlayAuthRequest request = new GetVideoPlayAuthRequest();
            request.VideoId = videoId;
            try
            {
                try
                {
                    GetVideoPlayAuthResponse response = client.GetAcsResponse(request);
                    return new VideoModel
                    {
                        RequestId = response.RequestId,
                        PlayAuth = response.PlayAuth,
                        Title = response.VideoMeta.Title,
                        VideoId = response.VideoMeta.VideoId,
                        CoverURL = response.VideoMeta.CoverURL,
                        Duration = response.VideoMeta.Duration,
                        Status = response.VideoMeta.Status,
                    };
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (ServerException)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取上传视频地址和视频编号  实际上是创建一个空的视频对象到阿里云
        /// </summary>
        public static UploadVideo CreateUploadVideo()
        {
            CreateUploadVideoRequest request = new CreateUploadVideoRequest();
            request.Title = "Title";
            request.FileName = Guid.NewGuid() + ".MP4";
            request.Description = "Description";
            request.Tags = "Tags";
            request.CateId = 0;
            try
            {
                CreateUploadVideoResponse response = client.GetAcsResponse(request);
                return new UploadVideo
                {
                    RequestId = response.RequestId,
                    UploadAddress = response.UploadAddress,
                    UploadAuth = response.UploadAuth,
                    VideoId = response.VideoId
                };
            }
            catch (ServerException e)
            {
                return null;
            }
        }

        /// <summary>
        /// 删除视频
        /// </summary>
        public static string DeleteVideoRequest(string VideoId)
        {
            DeleteVideoRequest request = new DeleteVideoRequest();
            request.VideoIds = VideoId;
            try
            {
                DeleteVideoResponse response = client.GetAcsResponse(request);
                return response.RequestId;
            }
            catch (ClientException ex)
            {
                return ex.ToString();
            }
        }


        /// <summary>
        /// 刷新视频上传凭证
        /// </summary>
        public static UploadVideo RefreshUploadVideo(string VideoId)
        {
            RefreshUploadVideoRequest request = new RefreshUploadVideoRequest();
            request.VideoId = VideoId;
            try
            {
                RefreshUploadVideoResponse response = client.GetAcsResponse(request);
                return new UploadVideo
                {
                    RequestId = response.RequestId,
                    UploadAddress = response.UploadAddress,
                    UploadAuth = response.UploadAuth,
                };
            }
            catch (ServerException e)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取上传的SecurityToken
        /// </summary>
        public static SecurityTokenModel GetSecurityToken()
        {
            // 构建一个 Aliyun Client, 用于发起请求
            // 构建Aliyun Client时需要设置AccessKeyId和AccessKeySevcret
            DefaultProfile.AddEndpoint(REGIONID, REGIONID, "Sts", ENDPOINT);
            // 构造AssumeRole请求
            AssumeRoleRequest request1 = new AssumeRoleRequest();
            request1.AcceptFormat = FormatType.JSON;
            // 指定角色Arn
            request1.RoleArn = RoleArn;
            request1.RoleSessionName = RoleSessionName;
            // 可以设置Token有效期，可选参数，默认3600秒；
            request1.DurationSeconds = 3600;
            // 可以设置Token的附加Policy，可以在获取Token时，通过额外设置一个Policy进一步减小Token的权限；
            // request.Policy="<policy-content>"
            try
            {
                AssumeRoleResponse response = client.GetAcsResponse(request1);
                Console.WriteLine("AccessKeyId: " + response.Credentials.AccessKeyId);
                Console.WriteLine("AccessKeySecret: " + response.Credentials.AccessKeySecret);
                Console.WriteLine("SecurityToken: " + response.Credentials.SecurityToken);
                //Token过期时间；服务器返回UTC时间，这里转换成北京时间显示；
                Console.WriteLine("Expiration: " + DateTime.Parse(response.Credentials.Expiration).ToLocalTime());
                return new SecurityTokenModel
                {
                    AccessKeyId = response.Credentials.AccessKeyId,
                    AccessKeySecret = response.Credentials.AccessKeySecret,
                    SecurityToken = response.Credentials.SecurityToken,
                    Expiration = DateTime.Parse(response.Credentials.Expiration).ToLocalTime()
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}