using System;
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;
using ZF.Infrastructure.RedisCache;

namespace ZF.Web.ApiControllers
{
    public class MenuController : BaseController
    {

        private readonly MenuAppService _menuAppService;

        private readonly OperatorLogAppService _operatorLogAppService;

        public MenuController(MenuAppService menuAppService, OperatorLogAppService operatorLogAppService)
        {
            _menuAppService = menuAppService;
            _operatorLogAppService = operatorLogAppService;
        }

        [HttpPost]
        public JqGridOutPut<MenuOutput> GetList(ListMenuInput input)
        {
            var count = 0;
            var list = _menuAppService.GetList(input, out count);
            return new JqGridOutPut<MenuOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }

        /// <summary>
        /// 根据id 删除实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut Delete(IdInputIds input)
        {
            RedisCacheHelper.Remove("MenuModuleTree");
            var array = input.Ids.TrimEnd(',').Split(',');
            foreach (var item in array)
            {
                var model = _menuAppService.Get(item);
                if (model!=null)
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = (int)Model.Menu,
                        OperatorType = (int)OperatorType.Delete,
                        Remark = "删除菜单:" + model.MenuName
                    });
                _menuAppService.Delete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }


        /// <summary>
        /// 新增或修改实体：Menu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(MenuInput input)
        {
            var data = _menuAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

        /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public Menu GetOne(IdInput input)
        {
            var model = _menuAppService.Get(input.Id);
            return model;
        }
    }
}
