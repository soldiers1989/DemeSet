using System;

namespace ZF.Infrastructure.AlipayService.Model
{
    public class SecurityTokenModel
    {
        public string AccessKeyId { get; set; }
        public string AccessKeySecret { get; set; }
        public DateTime Expiration { get; set; }
        public string SecurityToken { get; set; }
    }
}