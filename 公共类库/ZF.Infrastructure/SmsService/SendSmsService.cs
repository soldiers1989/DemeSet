using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ServiceStack.DataAnnotations;
using System.Reflection;

namespace ZF.Infrastructure.SmsService
{
    /// <summary>
    /// 发送短信
    /// </summary>
    public class SendSmsService
    {

        //短信API产品名称（短信产品名固定，无需修改）
        private static readonly string product = ConfigurationManager.AppSettings["Product"].ToString();
        //短信API产品域名（接口地址固定，无需修改）
        private static readonly string domain = ConfigurationManager.AppSettings["Domain"].ToString();
        //短信AK-id
        private static readonly string accessKeyId = ConfigurationManager.AppSettings["AccessKeyId"].ToString();
        //短信AK-value
        private static readonly string accessKeySecret = ConfigurationManager.AppSettings["AccessKeySecret"].ToString();
        //短信模版key
        private static readonly string smsModelKey = ConfigurationManager.AppSettings["SmsModelKey"].ToString();
        //短信模版key
        private static readonly string smsModelKey1 = ConfigurationManager.AppSettings["smsModelKey1"].ToString();
        //短信签名
        private static readonly string signName = ConfigurationManager.AppSettings["SignName"].ToString();


        public static SmsMessageOutPut SendSms1(string PhoneNumbers,string PassWord)
        {
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
            //初始化ascClient,暂时不支持多region（请勿修改）
            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();
            SmsMessageOutPut smsMessageOutPut = new SmsMessageOutPut();
            try
            {
                int VerificationCode = GetVerificationCode();
                //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为1000个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式
                request.PhoneNumbers = PhoneNumbers;
                //必填:短信签名-可在短信控制台中找到
                request.SignName = signName;
                //必填:短信模板-可在短信控制台中找到
                request.TemplateCode = smsModelKey1;
                //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
                request.TemplateParam = "{\"account\":\"" + PhoneNumbers + "\",\"password\":\"" + PassWord + "\"}";
                //可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                request.OutId = VerificationCode.ToString(); ;
                //请求失败这里会抛ClientException异常
                SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(request);
                smsMessageOutPut.Message = GetDescription(Msg.Success);
                smsMessageOutPut.Code = (int)Msg.Success;
                smsMessageOutPut.LogMessage = sendSmsResponse.Message;
                smsMessageOutPut.PhoneNumber = PhoneNumbers;
                smsMessageOutPut.VerificationCode = VerificationCode.ToString();
                return smsMessageOutPut;
            }
            catch (ServerException e)
            {
                smsMessageOutPut.Message = GetDescription(Msg.Failed);
                smsMessageOutPut.Code = (int)Msg.Failed;
                smsMessageOutPut.LogMessage = e.Message;
                smsMessageOutPut.PhoneNumber = PhoneNumbers;
                return smsMessageOutPut;
            }
            catch (ClientException e)
            {
                smsMessageOutPut.Message = GetDescription(Msg.Failed);
                smsMessageOutPut.Code = (int)Msg.Failed;
                smsMessageOutPut.LogMessage = e.Message;
                smsMessageOutPut.PhoneNumber = PhoneNumbers;
                return smsMessageOutPut;
            }
        }

        public static SmsMessageOutPut SendSms(SmsModel input)
        {
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
            //初始化ascClient,暂时不支持多region（请勿修改）
            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();
            SmsMessageOutPut smsMessageOutPut = new SmsMessageOutPut();
            try
            {
                int VerificationCode = GetVerificationCode();
                //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为1000个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式
                request.PhoneNumbers = input.PhoneNumbers;
                //必填:短信签名-可在短信控制台中找到
                request.SignName = signName;
                //必填:短信模板-可在短信控制台中找到
                request.TemplateCode = smsModelKey;
                //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
                request.TemplateParam = SetParam(VerificationCode);
                //可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                request.OutId = VerificationCode.ToString(); ;
                //请求失败这里会抛ClientException异常
                SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(request);
                smsMessageOutPut.Message = GetDescription(Msg.Success);
                smsMessageOutPut.Code = (int)Msg.Success;
                smsMessageOutPut.LogMessage = sendSmsResponse.Message;
                smsMessageOutPut.PhoneNumber = input.PhoneNumbers;
                smsMessageOutPut.VerificationCode = VerificationCode.ToString();
                return smsMessageOutPut;
            }
            catch (ServerException e)
            {
                smsMessageOutPut.Message = GetDescription(Msg.Failed);
                smsMessageOutPut.Code = (int)Msg.Failed;
                smsMessageOutPut.LogMessage = e.Message;
                smsMessageOutPut.PhoneNumber = input.PhoneNumbers;
                return smsMessageOutPut;
            }
            catch (ClientException e)
            {
                smsMessageOutPut.Message = GetDescription(Msg.Failed);
                smsMessageOutPut.Code = (int)Msg.Failed;
                smsMessageOutPut.LogMessage = e.Message;
                smsMessageOutPut.PhoneNumber = input.PhoneNumbers;
                return smsMessageOutPut;
            }
        }

        /// <summary>
        /// 获取枚举变量值的 Description 属性
        /// </summary>
        /// <param name="obj">枚举变量</param>
        /// <param name="isTop">是否改变为返回该类、枚举类型的头 Description 属性，而不是当前的属性或枚举变量值的 Description 属性</param>
        /// <returns>如果包含 Description 属性，则返回 Description 属性的值，否则返回枚举变量值的名称</returns>
        public static string GetDescription(object obj)
        {
            bool isTop = false;
            if (obj == null)
            {
                return string.Empty;
            }
            try
            {
                Type _enumType = obj.GetType();
                DescriptionAttribute dna = null;
                if (isTop)
                {
                    dna = (DescriptionAttribute)Attribute.GetCustomAttribute(_enumType, typeof(DescriptionAttribute));
                }
                else
                {
                    FieldInfo fi = _enumType.GetField(Msg.GetName(_enumType, obj));
                    dna = (DescriptionAttribute)Attribute.GetCustomAttribute(
                       fi, typeof(DescriptionAttribute));
                }
                if (dna != null && string.IsNullOrEmpty(dna.Description) == false)
                    return dna.Description;
            }
            catch
            {
            }
            return obj.ToString();
        }


        private static string SetParam(int verificationCode)
        {
            var smsParam = string.Empty;
            switch (smsModelKey)
            {
                case "SMS_127158896":
                    smsParam ="{\"code\":\""+ verificationCode.ToString() + "\"}";
                    break;
                default:
                    smsParam = "{\"code\":\"" + verificationCode.ToString() + "\"}";
                    break;
            }
            return smsParam;
        }
        /// <summary>
        /// 生成6位数的随机验证码
        /// </summary>
        /// <returns></returns>
        public static int GetVerificationCode()
        {
            Random random = new Random();
            return random.Next(100000, 1000000);
        }

        /// <summary>
        /// 生成随机用户名
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomString(int length)
        {
            string randStr = "";
            Random rd = new Random();
            byte[] str = new byte[length];
            int i;
            for (i = 0; i < length - 1; i++)
            {
                int a = 0;
                while (!((a >= 48 && a <= 57) || (a >= 97 && a <= 122)))
                {
                    a = rd.Next(48, 122);
                }
                str[i] = (byte)a;
            }
            string username = new string(UnicodeEncoding.ASCII.GetChars(str));
            Random r = new Random(unchecked((int)DateTime.Now.Ticks));
            string s1 = ((char)r.Next(97, 122)).ToString();
            username = username.Replace("/0", "");
            randStr = s1 + username;
            return randStr;
        }
    }
    
    /// <summary>
    /// 短信入参
    /// </summary>
    public class SmsModel
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNumbers { get; set; }
    }

    /// <summary>
    /// 短信出参
    /// </summary>
    public class SmsMessageOutPut
    {

        /// <summary>
        /// 返回状态
        /// </summary>
        public  bool Success { get; set; }

        /// <summary>
        /// 自定义返回码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 返回给用户的消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 写入日志的消息
        /// </summary>
        public string LogMessage { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string VerificationCode { get; set; }

        /// <summary>
        /// 短信失效时间
        /// </summary>
        public string FailureTime { get; set; }
    }


}
