using Aop.Api.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Infrastructure.AlipayService.Model;

namespace ZF.Infrastructure.AlipayService.Business
{
    public class AlipayTradePagePayResult
    {
        public AlipayTradePagePayResult()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public AlipayTradePagePayResponse response { get; set; }

        public ResultEnum Status
        {
            get
            {
                if (response != null)
                {
                    if (response.Code == ResultCode.SUCCESS)
                    {
                        return ResultEnum.SUCCESS;
                    }
                    if (response.Code == ResultCode.ERROR)
                    {
                        return ResultEnum.UNKNOWN;
                    }
                    else
                        return ResultEnum.FAILED;
                }
                else
                {
                    return ResultEnum.UNKNOWN;
                }

            }

        }
    }
}
