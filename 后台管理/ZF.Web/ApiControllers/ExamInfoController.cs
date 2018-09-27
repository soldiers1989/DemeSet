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
    public class ExamInfoController : ApiController
    {
        private readonly ExamInfoAppService _service;
        private readonly OperatorLogAppService _operatorLogAppService;
        public ExamInfoController( ExamInfoAppService service , OperatorLogAppService operatorLogAppService )
        {
            _service = service;
            _operatorLogAppService = operatorLogAppService; 
        }


        /// <summary>
        /// 备考信息列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<ExamInfoOutput> GetList( ExamInfoListInput input )
        {
            var count = 0;
            var list = _service.GetList( input, out count );
            return new JqGridOutPut<ExamInfoOutput>( )
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
                var model = _service.Get( item );
                if ( model != null )
                {
                    _operatorLogAppService.Add( new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = ( int )Model.ExamInfo,
                        OperatorType = ( int )OperatorType.Delete,
                        Remark = "删除备考信息:"
                    } );
                }
                _service.Delete( model );
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
        /// <summary>
        /// 新增或修改实体：资讯,帮助管理表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit( ExamInfoInput input )
        {
            var data = _service.AddOrEdit( input );
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

        /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public ExamInfo GetOne( IdInput input )
        {
            var model = _service.Get( input.Id );
            return model;
        }

        /// <summary>
        /// 启用
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public bool UpdateState( IdInput input ) {
            return _service.UpdateState( input.Id );
        }
    }
}
