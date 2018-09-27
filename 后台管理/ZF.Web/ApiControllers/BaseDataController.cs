using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    public class BaseDataController : BaseController
    {
        private readonly BaseDataAppService _baseDataAppService;

        private readonly OperatorLogAppService _operatorLogAppService;

        public BaseDataController( BaseDataAppService baseDataAppService, OperatorLogAppService operatorLogAppService )
        {
            _baseDataAppService = baseDataAppService;
            _operatorLogAppService = operatorLogAppService;
        }

        [HttpPost]
        public MessagesOutPut AddOrEdit(BasedataInput input) {
            return _baseDataAppService.AddOrEdit( input );
        }

        [HttpPost]
        public JqGridOutPut<BasedataOutput> GetList( BasedataListInput input ) {
            var count = 0;
            var list = _baseDataAppService.GetList( input ,out count);
            return new JqGridOutPut<BasedataOutput>( ) {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records=count,
                Rows=list

            };
        }

        [HttpPost]
        public Basedata GetOne( IdInput input ) {
            return _baseDataAppService.Get( input.Id);
        }

        [HttpPost]
        public MessagesOutPut Delete( IdInputIds input )
        {
            var array = input.Ids.TrimEnd( ',' ).Split( ',' );
            foreach ( var item in array )
            {
                var model = _baseDataAppService.Get( item );
                if ( model != null )
                {
                    _operatorLogAppService.Add( new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = ( int )Model.DataType,
                        OperatorType = ( int )OperatorType.Delete,
                        Remark = "删除数据字典:" + model.Name
                    } );
                    _baseDataAppService.LogicDelete( model.Id );
                }
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
    }
}