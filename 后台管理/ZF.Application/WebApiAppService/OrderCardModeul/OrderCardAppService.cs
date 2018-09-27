using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;
using ZF.Application.WebApiDto.OrderCardModule;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;

namespace ZF.Application.WebApiAppService.OrderCardModeul
{
    public class OrderCardAppService : BaseAppService<OrderCard>
    {
        private readonly IOrderCardRepository _repository;
        public OrderCardAppService( IOrderCardRepository repository):base(repository) {
            _repository = repository;
        }

        /// <summary>
        /// 维护订单学习卡使用记录
        /// </summary>
        /// <returns></returns>
        public MessagesOutPut AddRecourd( OrderCardInput input) {
            OrderCard  model = input.MapTo<OrderCard>( );
            model.Id = Guid.NewGuid( ).ToString( );
            model.OrderNo = input.OrderNo;
            model.CardCode = input.CardCode;
            _repository.InsertGetId( model );
            return new MessagesOutPut { Success = true, Message = "操作成功" };
        }


    }
}
