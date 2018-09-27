using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Core.Entity;
using ZF.Core.IRepository;

namespace ZF.Application.WebApiAppService.PurchaseDiscountModule
{
   public class PurchaseDiscountAppService: BaseAppService<PurchaseDiscount>
    {
        private readonly IPurchaseDiscountRepository _repository;
        public PurchaseDiscountAppService( IPurchaseDiscountRepository repository ) : base( repository ) {
            _repository = repository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="priceSum"></param>
        /// <returns></returns>
        public PurchaseDiscount GetGetBestDiscountNum( decimal priceSum )
        {
            var sql = " SELECT * FROM dbo.t_purchase_discount WHERE TopNum<=@priceSum AND CONVERT(DATE,BeginDate)<=GETDATE() AND EndDate>=CONVERT(DATE,GETDATE())  order by TopNum desc, MinusNum desc ";
            var dy = new DynamicParameters( );
            dy.Add( ":priceSum",priceSum,DbType.Decimal);
            return Db.QueryFirstOrDefault<PurchaseDiscount>( sql,dy);
        }
    }
}
