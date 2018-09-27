using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.AutoMapper.AutoMapper;
using ZF.Core;
using ZF.Core.Entity;

namespace ZF.Application.WebApiDto.OrderCardModule
{

    [AutoMap( typeof( OrderCard ) )]
    public class OrderCardInput
    {
        public string OrderNo { get; set; }

        public string CardCode { get; set; }
    }
}
