using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Infrastructure.AlipayService.Model
{
    public class ResultCode
    {
        /// <summary>
        /// 接口调用成功，调用结果请参考具体的API文档所对应的业务返回参数
        /// </summary>
        public const string SUCCESS = "10000";
        public const string INRROCESS = "10003";
        //业务处理失败   
        public const string FAIL = "40004";
        //服务不可用
        public const string ERROR = "20000";
    }
}
