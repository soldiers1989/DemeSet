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
    public class BaseTagController : BaseController
    {
        private readonly BaseTagAppService _baseTagAppService;
        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        public BaseTagController( BaseTagAppService baseTagAppService, OperatorLogAppService operatorLogAppService )
        {
            _baseTagAppService = baseTagAppService;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询列表实体
        /// </summary>
        [HttpPost]
        public List<BaseTagOutput> GetSelectList( BaseTagInput input )
        {
            var list = _baseTagAppService.GetList( input);
            return list;
        }

        [HttpPost]
        public JqGridOutPut<BaseTagOutput> GetList( BaseTagListInput input) {
            var count = 0;
            var list = _baseTagAppService.GetList( input, out count );
            return new JqGridOutPut<BaseTagOutput>( )
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
        public MessagesOutPut Delete( IdInputIds input )
        {
            var array = input.Ids.TrimEnd( ',' ).Split( ',' );
            foreach ( var item in array )
            {
                var model = _baseTagAppService.Get( item );
                if ( model != null )
                {
                    _operatorLogAppService.Add( new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = ( int )Model.AfficheHelp,
                        OperatorType = ( int )OperatorType.Delete,
                        Remark = "删除标签:"+model.TagName
                    } );
                }
                _baseTagAppService.Delete( model );
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
        /// <summary>
        /// 新增或修改实体：资讯,帮助管理表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit( BaseTagInput input )
        {
            var data = _baseTagAppService.AddOrEdit( input );
            return new MessagesOutPut { Id = data.Id, ModelId=data.ModelId, Message = data.Message, Success = data.Success };
        }

        /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public Base_Tag GetOne( IdInput input )
        {
            var model = _baseTagAppService.Get( input.Id );
            return model;
        }
    }
}
