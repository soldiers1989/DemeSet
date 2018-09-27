using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.BaseDto;
using ZF.Application.WebApiDto.SystemModule;
using ZF.Application.WebApiAppService.SystemModule;
using ZF.Application.WebApiAppService.UserDiscountCardModule;
using ZF.Application.WebApiDto.UserDiscountCardModule;

namespace ZF.API.Controllers
{
    public class UserDiscountCardController : BaseApiController
    {
        private readonly UserDiscountCardAppService _service;


        public UserDiscountCardController(UserDiscountCardAppService service)
        {
            _service = service;
        }


        /// <summary>
        /// 添加学习卡
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddCard(UserDiscountCardModelInput input)
        {
            input.UserId = UserObject.Id;
            return _service.AddCard(input);
        }

        /// <summary>
        /// 是否存在该学习卡
        /// </summary>
        /// <param name="CardId"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut IfExistAndExpair(string CardId)
        {
            return _service.IfExistAndExpair(CardId);
        }

        /// <summary>
        /// 获取用户学习卡列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<UserDiscountCardModelOutput> GetList(UserDiscountCardModelInput input)
        {
            input.UserId = UserObject.Id;
            return _service.GetList(input);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public List<UserDiscountCardModelOutput> GetMyCard(UserDiscountCardModelInput input)
        {
            input.UserId = UserObject.Id;
            return _service.GetMyCard(input);
        }

        /// <summary>
        /// 获取可使用的学习卡
        /// </summary>
        [HttpPost]
        public List<UserDiscountCardModelOutput> GetUseCard(string courseId)
        {

            return _service.GetUseCard(UserObject.Id, courseId, null);
        }

        /// <summary>
        /// 获取可使用的学习卡
        /// </summary>
        [HttpPost]
        public List<UserDiscountCardModelOutput> GetUseCard(string courseId, string orderNo)
        {

            return _service.GetUseCard(UserObject.Id, courseId, orderNo);
        }
    }
}
