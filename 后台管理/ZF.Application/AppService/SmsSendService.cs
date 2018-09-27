using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using qcloudsms_csharp;
using qcloudsms_csharp.json;
using qcloudsms_csharp.httpclient;
using System.Configuration;

namespace ZF.Application.AppService
{
    public  class SmsSendService
    {
        //private static int SMS_APPID =int.Parse(ConfigurationManager.AppSettings["Sms_AppId"].ToString( ));
        //private static string SMS_APPKEY = ConfigurationManager.AppSettings["Sms_AppKey"].ToString( );
        //private static int SMS_TEMPLATEDID = int.Parse( ConfigurationManager.AppSettings["Sms_TemplateId"].ToString( ) );

        private static int SMS_APPID = 1400118282;
        private static string SMS_APPKEY = "988d0ce6de1001ba0b14ab6c22fe1038";
        private static int SMS_TEMPLATEDID = 167225;

        /// <summary>
        /// 模板单发
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static SmsSendOutput SingleSendSms( SmsSendInput input) {
            try
            {
                SmsSingleSender ssender = new SmsSingleSender( SMS_APPID, SMS_APPKEY );
                var result = ssender.sendWithParam( "86", input.PhoneNumber,
                    SMS_TEMPLATEDID, new[] { GetVerificationCode().ToString() }, "", "", "" );
                return new SmsSendOutput { Success = true, Message = "短信发送成功" };
            } catch ( Exception e ) {
                return new SmsSendOutput { Success = false, Message = "短信发送失败" };
            }
        }

        /// <summary>
        /// 模板群发
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static SmsSendOutput MultiSendSms( SmsSendInput input ) {
            try
            {
                SmsMultiSender msender = new SmsMultiSender( SMS_APPID, SMS_APPKEY );
                var list = new List<string> { GetVerificationCode().ToString() };
                var sresult = msender.sendWithParam( "86", input.ListPhoneNumbers, SMS_TEMPLATEDID,
                    list, "", "", "" );
                return new SmsSendOutput { Success = true, Message = "短信发送成功" };
            } catch ( JSONException e )
            {
                return new SmsSendOutput { Success = false, Message = "短信发送失败" };
            }
        }

        /// <summary>
        /// 发送语音验证码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static SmsSendOutput VoiceSendSms( SmsSendInput input ) {
            try
            {
                
                SmsVoiceVerifyCodeSender vvcsender = new SmsVoiceVerifyCodeSender( SMS_APPID, SMS_APPKEY );
                var result = vvcsender.send( "86", input.PhoneNumber, GetVerificationCode( ).ToString( ), 2, "" );
                return new SmsSendOutput { Success = true, Message = "短信发送成功" };
            } catch ( JSONException e )
            {
                return new SmsSendOutput { Success = false, Message = "短信发送失败" };
            }
        }

        /// <summary>
        ///发送语音通知
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static SmsSendOutput VoiceNotificationSendSms( SmsSendInput input ) {
            try
            {
                SmsVoicePromptSender vvcsender = new SmsVoicePromptSender( SMS_APPID, SMS_APPKEY );
                var list = new List<string> { "67890" };
                var result = vvcsender.send( "86", input.PhoneNumber,2, GetVerificationCode( ).ToString( ), 2, "" );
                return new SmsSendOutput { Success = true, Message = "短信发送成功" };
            } catch ( JSONException e )
            {
                return new SmsSendOutput { Success = false, Message = "短信发送失败" };
            }
        }

        /// <summary>
        /// 生成6位数的随机验证码
        /// </summary>
        /// <returns></returns>
        public static int GetVerificationCode( )
        {
            Random random = new Random( );
            return random.Next( 100000, 1000000 );
        }
    }


    /// <summary>
    /// 短信发送入参
    /// </summary>
    public class SmsSendInput {
        public string PhoneNumber { get; set; }

        public List<string> ListPhoneNumbers { get; set; }
    }


    /// <summary>
    ///短信发送出参
    /// </summary>
    public class SmsSendOutput {
        public string Message { get; set; }

        public bool Success { get; set; }
    }
}
