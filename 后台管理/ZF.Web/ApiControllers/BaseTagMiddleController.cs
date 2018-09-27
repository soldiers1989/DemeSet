using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    public class BaseTagMiddleController : BaseController
    {
        private readonly BaseTagMiddleAppService _baseTagMiddleAppService;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        public BaseTagMiddleController( BaseTagMiddleAppService baseTagMiddleAppService, OperatorLogAppService operatorLogAppService )
        {
            _baseTagMiddleAppService = baseTagMiddleAppService;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询列表实体：资讯,帮助管理表 
        /// </summary>
        [HttpPost]
        public List<BaseTagMiddleOutput> GetList( BaseTagMiddleInput input )
        {
            var list = _baseTagMiddleAppService.GetList( input );
            return list;
        }


        /// <summary>
        /// 根据id 删除实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut Delete( IdInputIds input )
        {
            var array = input.Ids.TrimEnd( ',' ).Split( ',' );
            foreach ( var item in array )
            {
                var model = _baseTagMiddleAppService.Get( item );
                if ( model != null )
                {
                    _operatorLogAppService.Add( new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = ( int )Model.BaseTagMiddle,
                        OperatorType = ( int )OperatorType.Delete,
                        Remark = "删除实体标签:" + model.ModelId+":"+model.TagId
                    } );
                }
                _baseTagMiddleAppService.Delete( model );
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
        /// <summary>
        /// 新增或修改实体：资讯,帮助管理表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit( BaseTagMiddleInput input )
        {
            var data = _baseTagMiddleAppService.AddOrEdit( input );
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

        /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public Base_TagMiddle GetOne( IdInput input )
        {
            var model = _baseTagMiddleAppService.Get( input.Id );
            return model;
        }
    }
}
