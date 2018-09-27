using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：订单表 
    /// </summary>
    public sealed class OrderSheetMap : BaseClassMapper<OrderSheet, Guid>
    {
		public OrderSheetMap ()
		{
			Table("t_Order_Sheet");
				
			Map(x => x.OrderNo).Column("OrderNo");
			Map(x => x.RegisterUserId).Column("RegisterUserId");
			Map(x => x.AddTime).Column("AddTime");
			Map(x => x.OrderAmount).Column("OrderAmount");
			Map(x => x.State).Column("State");
			Map(x => x.FactPayAmount).Column("FactPayAmount");
			Map(x => x.OrderIp).Column("OrderIp");
			Map(x => x.PayType).Column("PayType");
			Map(x => x.PayTime).Column("PayTime");
			Map(x => x.OrderType).Column("OrderType");
			Map(x => x.TradeNo).Column("TradeNo");
			Map(x => x.Remark).Column("Remark");
            Map(x => x.InstitutionsId).Column("InstitutionsId");
            Map(x => x.HandOutId).Column("HandOutId");
			Map(x => x.ExpressCompanyId).Column("ExpressCompanyId");
			Map(x => x.ExpressNumber).Column("ExpressNumber");
            Map(x => x.InvoiceHeader).Column("InvoiceHeader");
            Map(x => x.InvoiceMailbox).Column("InvoiceMailbox");
            Map(x => x.TaxpayerIdentificationNumber).Column("TaxpayerIdentificationNumber");
            Map(x => x.InvoiceState).Column("InvoiceState");
            Map(x => x.InvoicePhone).Column("InvoicePhone");
            Map(x => x.InvoiceTime).Column("InvoiceTime");
            Map(x => x.SetOrderUser).Column("SetOrderUser");
            Map(x => x.SetOrderTime).Column("SetOrderTime");
            Map(x => x.PromotionCode).Column("PromotionCode");
            this.AutoMap();
		}
    }
}

