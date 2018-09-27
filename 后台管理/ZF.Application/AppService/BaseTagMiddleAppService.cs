using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure;

namespace ZF.Application.AppService
{
   public class BaseTagMiddleAppService:BaseAppService<Base_TagMiddle>
    {
        private  readonly OperatorLogAppService _operatorLogAppService;
        private readonly IBaseTagMiddleRepository _repository;
        public BaseTagMiddleAppService( IBaseTagMiddleRepository repository, OperatorLogAppService logService):base(repository)
        {
            _repository = repository;
            _operatorLogAppService = logService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut AddOrEdit( BaseTagMiddleInput input )
        {
            Base_TagMiddle model;
            if ( !string.IsNullOrEmpty( input.Id ) )
            {
                model = _repository.Get( input.Id );
                if ( model != null )
                {
                    model.Id = input.Id;
                    model.ModelId = input.ModelId;
                    model.TagId = input.TagId;
                    _repository.Update( model );
                    _operatorLogAppService.Add( new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = ( int )Model.BaseTagMiddle,
                        OperatorType = ( int )OperatorType.Edit,
                        Remark = "修改实体标签：" + model.ModelId+":"+model.TagId
                    } );
                    return new MessagesOutPut { Success = true, Message = "修改成功" };
                }
            }
            model = input.MapTo<Base_TagMiddle>( );
            model.Id = Guid.NewGuid( ).ToString( );
            model.ModelId = input.ModelId;
            model.TagId = input.TagId;
            var keyId = _repository.InsertGetId( model );
            _operatorLogAppService.Add( new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = ( int )Model.BaseTagMiddle,
                OperatorType = ( int )OperatorType.Add,
                Remark = "新增实体标签:" + model.ModelId + ":" + model.TagId
            } );
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut Delete( IdInputIds input )
        {
            var strIds = input.Ids.TrimEnd( ',' );
            var arr = strIds.Split( ',' );

            foreach ( var item in arr )
            {
                var model = _repository.Get( item );
                if ( model != null )
                {
                    _repository.Delete( model );
                    _operatorLogAppService.Add( new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = ( int )Model.BaseTagMiddle,
                        OperatorType = ( int )OperatorType.Delete,
                        Remark = "删除实体标签:" + model.ModelId + ":" + model.TagId
                    } );
                }
            }
            return new MessagesOutPut { Success = true, Message = "删除成功" };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<BaseTagMiddleOutput> GetList( BaseTagMiddleInput input )
        {
            var sql = new StringBuilder( " select  * from t_Base_TagMiddle where 1=1  " );
            var dynamicParameters = new DynamicParameters( );
            if ( input != null )
            {
                if ( !string.IsNullOrWhiteSpace( input.ModelId ) )
                {
                    sql.Append( " and ModelId=@ModelId " );
                    dynamicParameters.Add( ":ModelId", input.ModelId, DbType.String );
                }
            }
            var list = Db.QueryList<BaseTagMiddleOutput>( sql.ToString( ), dynamicParameters );
            return list;
        }
    }
}
