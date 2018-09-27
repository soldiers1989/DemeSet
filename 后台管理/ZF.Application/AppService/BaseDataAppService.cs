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
   public class BaseDataAppService : BaseAppService<Basedata>
    {
        private readonly OperatorLogAppService _operatorLogAppService;
        private readonly IBasedataRepository _repository;

        public BaseDataAppService(IBasedataRepository respository, OperatorLogAppService logService):base( respository ) {
            _repository = respository;
            _operatorLogAppService = logService;
        }


        /// <summary>
        /// 编辑或新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut AddOrEdit( BasedataInput input) {
            Basedata model;
            if ( !string.IsNullOrEmpty( input.Id ) ) {
                 model = _repository.Get( input.Id);
                if ( model != null ) {
                    model.Id = input.Id;
                    model.Name = input.Name;
                    model.Code = input.Code;
                    model.DataTypeId = input.DataTypeId;
                    model.UpdateUserId = UserObject.Id;
                    model.UpdateTime = DateTime.Now;
                    model.Sort = input.Sort;
                    model.Desc = input.Desc;
                    _repository.Update( model);
                    _operatorLogAppService.Add( new OperatorLogInput {
                        KeyId=model.Id,
                        ModuleId=(int)Model.BaseData,
                        OperatorType=(int)OperatorType.Edit,
                        Remark="修改数据字典："+model.Name
                    } );
                    return new MessagesOutPut { Success=true,Message="修改成功"};
                }
            }
            model = input.MapTo<Basedata>( );
            model.Id = Guid.NewGuid( ).ToString( );
            model.Name = input.Name;
            model.Code = input.Code;
            model.DataTypeId = input.DataTypeId;
            model.Sort = input.Sort;
            model.Desc = input.Desc;
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            var keyId = _repository.InsertGetId( model );
            _operatorLogAppService.Add( new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = ( int )Model.Project,
                OperatorType = ( int )OperatorType.Add,
                Remark = "新增数据字典:" + model.Name
            } );
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

        /// <summary>
        /// 获取数据字典集合
        /// </summary>
        public List<BasedataOutput> GetList( BasedataListInput input, out int count )
        {
            const string sql = " select distinct a.Id,a.Name,b.DataTypeName,a.DataTypeId,a.Code,a.AddTime,a.Sort,a.[Desc] ";
            var strSql = new StringBuilder( " from t_Base_Basedata a left join t_Base_DataType b ON a.DataTypeId=b.Id   where 1=1 and a.IsDelete=0 " );
            var dynamicParameters = new DynamicParameters( );
            const string sqlCount = "select count(*) ";
            if ( input != null )
            {
                if ( !string.IsNullOrWhiteSpace( input.Name ) )
                {
                    strSql.Append( " and a.Name like  @Name " );
                    dynamicParameters.Add( ":Name", '%' + input.Name + '%', DbType.String );
                }
                if ( !string.IsNullOrWhiteSpace( input.DataTypeId ) )
                {
                    strSql.Append( " and a.DataTypeId =  @DataTypeId " );
                    dynamicParameters.Add( ":DataTypeId", input.DataTypeId, DbType.String );
                }
                if ( !string.IsNullOrEmpty( input.Code ) ) {
                    strSql.Append( " and a.Code =  @Code " );
                    dynamicParameters.Add( ":Code", input.Code, DbType.String );
                }
            }
            count = Db.ExecuteScalar<int>( sqlCount + strSql, dynamicParameters );
            var list = Db.QueryList<BasedataOutput>( GetPageSql( sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord ), dynamicParameters );
            return list;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut Delete(IdInputIds input) {
            var strIds = input.Ids.TrimEnd( ',' );
            var arr = strIds.Split( ',');
            
            foreach ( var item in arr ) {
                var model = _repository.Get( item);
                if ( model != null ) {
                    _repository.LogicDelete( model);
                    _operatorLogAppService.Add( new OperatorLogInput {
                        KeyId = model.Id,
                        ModuleId = ( int )Model.BaseData,
                        OperatorType = ( int )OperatorType.Delete,
                        Remark = "删除数据字典:" + model.Name
                    } );
                }
            }
            return new MessagesOutPut { Success=true,Message="删除成功" } ;
        }

    }
}
