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
   public class PurchaseDiscountAppService: BaseAppService<PurchaseDiscount>
    {
        private readonly IPurchaseDiscountRepository _repository;
        private readonly OperatorLogAppService _operatorLogAppService;
        private readonly CourseInfoAppService _courseInfoAppService;

        public PurchaseDiscountAppService( IPurchaseDiscountRepository repository, OperatorLogAppService operatorLogAppService, CourseInfoAppService courseInfoAppService ) :base(repository) {
            _repository = repository;
            _operatorLogAppService = operatorLogAppService;
            _courseInfoAppService = courseInfoAppService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut AddOrEdit( PurchaseDiscountInput input )
        {
            PurchaseDiscount model;
            if ( !string.IsNullOrEmpty( input.Id ) )
            {
                model = _repository.Get( input.Id );
                if ( model != null )
                {
                    model.Id = input.Id;
                    model.TopNum = input.TopNum;
                    model.MinusNum = input.MinusNum;
                    model.BeginDate = input.BeginDate;
                    model.EndDate = input.EndDate;
                    model.TargetCourse = input.TargetCourse;

                    _repository.Update( model );
                    _operatorLogAppService.Add( new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = ( int )Model.PurchaseDiscount,
                        OperatorType = ( int )OperatorType.Edit,
                        Remark = "修改满减券：" + model.Id
                    } );
                    return new MessagesOutPut { Success = true, Message = "修改成功" };
                }
            }
            model = input.MapTo<PurchaseDiscount>( );
            model.Id = Guid.NewGuid( ).ToString( );
            model.TopNum = input.TopNum;
            model.MinusNum = input.MinusNum;
            model.BeginDate = input.BeginDate;
            model.EndDate = input.EndDate;
            model.TargetCourse = input.TargetCourse;
            var keyId = _repository.InsertGetId( model );
            _operatorLogAppService.Add( new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = ( int )Model.Project,
                OperatorType = ( int )OperatorType.Add,
                Remark = "新增满减券:" + model.Id
            } );
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

        /// <summary>
        /// 获取数据字典集合
        /// </summary>
        public List<PurchaseDiscountOutput> GetList( PurchaseDiscountListInput input, out int count )
        {
            const string sql = " select a.*  ";
            var strSql = new StringBuilder( " from t_purchase_discount a where 1=1 " );
            var dynamicParameters = new DynamicParameters( );
            const string sqlCount = "select count(*) ";
            if ( input != null )
            {
                if ( !string.IsNullOrWhiteSpace( input.TargetCourse ) )
                {
                    strSql.Append( " and a.TargetCourse like  @TargetCourse or isnull(a.TargetCourse,'')=''" );
                    dynamicParameters.Add( ":TargetCourse", "%" + input.TargetCourse + "%", DbType.String );
                }
                if ( input.BeginDate.HasValue )
                {
                    strSql.Append( " and isnull(a.EndDate,'9999-12-31') >=  @BeginDate" );
                    dynamicParameters.Add( ":BeginDate", input.BeginDate, DbType.String );
                }
                if ( input.EndDate.HasValue )
                {
                    strSql.Append( " and isnull(a.BeginDate,'0001-01-01') <=  @EndDate " );
                    dynamicParameters.Add( ":EndDate", input.EndDate, DbType.String );
                }
            }
            count = Db.ExecuteScalar<int>( sqlCount + strSql, dynamicParameters );
            var list = Db.QueryList<PurchaseDiscountOutput>( GetPageSql( sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, "EndDate", "Desc" ), dynamicParameters );

            foreach ( var item in list )
            {
                var targetCourseName = "";
                var target = item.TargetCourse;
                if ( !string.IsNullOrEmpty( target ) )
                {
                    var arr = target.Split( ',' );
                    for ( var i = 0; i < arr.Length; i++ )
                    {
                        var model = _courseInfoAppService.Get( arr[i] );
                        if ( model != null )
                        {
                            targetCourseName += model.CourseName + "，";
                        }
                    }
                    item.TargetCourse = targetCourseName.TrimEnd( '，' );
                }
            }

            return list;
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
                        ModuleId = ( int )Model.BaseData,
                        OperatorType = ( int )OperatorType.Delete,
                        Remark = "删除满减:" + model.Id
                    } );
                }
            }
            return new MessagesOutPut { Success = true, Message = "删除成功" };
        }

    }
}
