using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.WebApiAppService.CourseModule;
using ZF.Application.WebApiDto.UserDiscountCardModule;
using ZF.Core.Entity;
using ZF.Core.IRepository;

namespace ZF.Application.WebApiAppService.UserDiscountCardModule
{
   public class DiscountCardAppService: BaseAppService<Discount_Card>
    {
        private readonly IDiscount_CardRepository _repository;
        private readonly CourseInfoAppService _courseInfoAppService;

        public DiscountCardAppService( IDiscount_CardRepository repository ) : base( repository ) {
            _repository = repository;
        }


        /// <summary>
        /// 获取学习卡信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public DiscountCardModelOutput GetOne( DiscountCardModelInput input ) {
            var sql = " select * from t_Discount_Card where CardCode=@CardCode ";
            var dy = new DynamicParameters( );
            dy.Add( ":CardCode",input.CardCode,DbType.String);
            var model = Db.QueryFirstOrDefault<DiscountCardModelOutput>( sql, dy );
            var targetCourseName = "";
            var target = model.TargetCourse;
            if ( !string.IsNullOrEmpty( target ) )
            {
                var arr = target.Split( ',' );
                for ( var i = 0; i < arr.Length; i++ )
                {
                    var obj = _courseInfoAppService.Get( arr[i] );
                    targetCourseName +="["+ obj.CourseName + "]，";
                }
                model.TargetCourse = targetCourseName.TrimEnd( '，' );
            }
            return model;
        }
    }
}
