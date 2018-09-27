
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using Dapper;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Application.Dto.SalesReports;
using ZF.Application.WebApiDto.SheetDtoModule;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure;
using ZF.Infrastructure.Enum;
using ZF.Infrastructure.RandomHelper;

namespace ZF.Application.AppService
{
    /// <summary>
    /// 数据表实体应用服务现实：订单表 
    /// </summary>
    public class OrderSheetAppService : BaseAppService<OrderSheet>
    {
        private readonly IOrderSheetRepository _iOrderSheetRepository;

        private readonly OrderDetailAppService _orderDetailAppService;
        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        private readonly IMyCourseRepository _iMyCourseRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iOrderSheetRepository"></param>
        /// <param name="operatorLogAppService"></param>
        /// <param name="orderDetailAppService"></param>
        /// <param name="iMyCourseRepository"></param>
        public OrderSheetAppService(IOrderSheetRepository iOrderSheetRepository, OperatorLogAppService operatorLogAppService, OrderDetailAppService orderDetailAppService, IMyCourseRepository iMyCourseRepository) : base(iOrderSheetRepository)
        {
            _iOrderSheetRepository = iOrderSheetRepository;
            _operatorLogAppService = operatorLogAppService;
            _orderDetailAppService = orderDetailAppService;
            _iMyCourseRepository = iMyCourseRepository;
        }

        /// <summary>
        /// 查询已支付订单中的电子发票信息
        /// </summary>
        /// <returns></returns>
        public List<OrderSheetOutput> GetElectronicInvoiceInfoByOrderSheet(OrderSheetListInput input)
        {
            string sql = @" select Id,OrderNo,AddTime,InvoiceHeader,InvoiceMailbox,TaxpayerIdentificationNumber,isnull(InvoiceState,0)InvoiceState,InvoicePhone
                                  from t_Order_Sheet where [State] = 0
                                  and(isnull(InvoiceHeader, '') <> '' or isnull(InvoiceMailbox, '') <> '' or isnull(TaxpayerIdentificationNumber, '') <> '')
                                   ";
            if (input.InvoiceState == 0)
            {
                sql += " and isnull(InvoiceState,0)=0  order by AddTime";
            }
            else if (input.InvoiceState == 1)
            {
                sql += " and InvoiceState = 1  order by AddTime";
            }
            var listInfo = Db.QueryList<OrderSheetOutput>(sql, null);
            return listInfo;
        }

        /// <summary>
        /// 处理需要电子发票的订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut EnditiElectronicInvoice(IdInput input)
        {
            OrderSheet model = _iOrderSheetRepository.Get(input.Id);
            model.InvoiceState = 1;
            try
            {
                _iOrderSheetRepository.Update(model);
                return new MessagesOutPut { Success = true, Message = "处理完成" };
            }
            catch (Exception)
            {
                return new MessagesOutPut { Success = false, Message = "处理失败" };
            }
        }

        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public SheetModelPageOutput OrderSheetDetail(SheetModelInput input)
        {
            string DefuleDomain = ConfigurationManager.AppSettings["DefuleDomain"];
            const string sql = @" SELECT a.*,b.Amount CommodityPrice FROM v_order_sheet_detail a INNER JOIN
                                 (SELECT OrderNo, SUM(Amount)Amount FROM t_Order_Detail
                                 WHERE OrderNo = (SELECT Id FROM t_Order_Sheet WHERE OrderNo = @OrderNo) GROUP BY OrderNo) b
                                     ON a.Id = b.OrderNo
                                WHERE a.OrderNo = @OrderNo ";
            var parament = new DynamicParameters();
            parament.Add(":OrderNo", input.OrderNo, DbType.String);
            //订单信息
            var orderSheet = Db.QueryFirstOrDefault<SheetModelPageOutput>(sql, parament);
            //订单详情
            string detailSql = " select Id,OrderNo,RegisterUserId,CourseId,FavourablePrice,Num,Amount,CourseName,'" + DefuleDomain + "/'+CourseIamge CourseIamge,OrderAmount,ValidityPeriod,AddTime,State,CourseType from v_SheetInfoByOrder where OrderNo=@OrderNo ";
            List<SheetModelPageDetailOutput> orderSheetDetail = Db.QueryList<SheetModelPageDetailOutput>(detailSql, parament);
            orderSheet.sheetmodelpagedetailoutput = orderSheetDetail;
            return orderSheet;

        }

        /// <summary>
        /// 查询列表实体：订单表 
        /// </summary>
        public List<OrderSheetOutput> GetList(OrderSheetListInput input, out int count)
        {
            const string sql = "select  a.*,b.NickNamw as RegisterUserName,c.Name as PayTypeName,d.Name as InstitutionsName ";
            var strSql = new StringBuilder(@" from v_SheetInfoByService  a 
                                              left join t_Base_RegisterUser b on a.RegisterUserId=b.Id 
                                              left join t_Order_Institutions d on a.InstitutionsId=d.Id 
                                              left join t_Base_Basedata c on a.PayType=c.Id where 1=1 ");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.OrderNo))
            {
                strSql.Append(" and a.OrderNo like @OrderNo ");
                dynamicParameters.Add(":OrderNo", '%' + input.OrderNo + '%', DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.InstitutionsId))
            {
                strSql.Append(" and a.InstitutionsId like @InstitutionsId ");
                dynamicParameters.Add(":InstitutionsId", '%' + input.InstitutionsId + '%', DbType.String);
            }
            if (input.OrderType.HasValue)
            {
                strSql.Append(" and a.OrderType = @OrderType ");
                dynamicParameters.Add(":OrderType", input.OrderType, DbType.String);
            }

            if (!string.IsNullOrWhiteSpace(input.PayType))
            {
                strSql.Append(" and a.PayType = @PayType ");
                dynamicParameters.Add(":PayType", input.PayType, DbType.String);
            }
            if (input.State.HasValue)
            {
                strSql.Append(" and a.State = @State ");
                dynamicParameters.Add(":State", input.State, DbType.Int32);
            }
            if (!string.IsNullOrWhiteSpace(input.RegisterUserName))
            {
                strSql.Append(" and b.NickNamw like @RegisterUserName ");
                dynamicParameters.Add(":RegisterUserName", '%' + input.RegisterUserName + '%', DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.TradeNo))
            {
                strSql.Append(" and a.TradeNo like @TradeNo ");
                dynamicParameters.Add(":TradeNo", '%' + input.TradeNo + '%', DbType.String);
            }
            if (input.AddTimeBegin.HasValue)
            {
                strSql.Append(" and a.AddTime >= @AddTimeBegin ");
                dynamicParameters.Add(":AddTimeBegin", input.AddTimeBegin, DbType.DateTime);
            }
            if (input.AddTimeEnd.HasValue)
            {
                strSql.Append(" and a.AddTime <= @AddTimeEnd ");
                dynamicParameters.Add(":AddTimeEnd", input.AddTimeEnd.Value.AddDays(1), DbType.DateTime);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<OrderSheetOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);

            foreach (var item in list)
            {
                item.State = EnumHelper.GetEnumName<OrderState>((int)(OrderState)Enum.Parse(typeof(OrderState), item.State));
                item.OrderType = EnumHelper.GetEnumName<OrderType>((int)(OrderType)Enum.Parse(typeof(OrderType), item.OrderType));
            }
            return list;
        }

        /// <summary>
        /// 新增实体  订单表
        /// </summary>
        public MessagesOutPut AddOrEdit(OrderSheetInput input)
        {
            var model = input.MapTo<OrderSheet>();
            model.PayTime = DateTime.ParseExact(input.PayTime1, "yyyy-MM-dd HH-mm-ss", null);
            model.Id = Guid.NewGuid().ToString();
            model.AddTime = DateTime.Now;
            model.OrderIp = GetAddressIp();
            model.OrderType = (int)OrderType.Artificial;
            //  model.SetOrderTime = null;
            model.PayType = "be0feaf4-22e7-41a5-93c4-885683539ff2";//后台支付编码
            model.State = (int)OrderState.PaymentHasBeen;
            model.OrderNo = DateTime.Now.ToString("yyyyMMddHHmmssfff") + RandomHelper.GetRandom(3, 1)[0].ToString();
            foreach (var item in input.Data)
            {
                _orderDetailAppService.AddOrEdit(new OrderDetailInput
                {
                    Amount = item.Total,
                    CourseId = item.Id,
                    CourseType = item.CourseType,
                    FavourablePrice = item.FavourablePrice,
                    Num = item.Number,
                    OrderNo = model.Id,
                    Price = item.Price
                });
                var data = item.ValidityEndDate ?? DateTime.Now.AddYears(100);
                _iMyCourseRepository.Insert(new MyCourse
                {
                    AddTime = DateTime.Now,
                    BeginTime = DateTime.Now,
                    CourseId = item.Id,
                    CourseType = item.CourseType,
                    Id = Guid.NewGuid().ToString(),
                    UserId = model.RegisterUserId,
                    EndTime = data,
                    OrderId = model.Id
                });
            }
            var keyId = _iOrderSheetRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.OrderSheet,
                OperatorType = (int)OperatorType.Add,
                Remark = "新增订单表:订单编号" + model.OrderNo
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }


        /// <summary>
        /// 更新订单为撤销状态  且删除订单明细表中数据
        /// </summary>
        /// <param name="model"></param>
        public void UpdateModel(OrderSheet model)
        {
            var dyCourse = new DynamicParameters();
            dyCourse.Add(":OrderId", model.Id, DbType.String);
            var myCourse =
                Db.QueryFirstOrDefault<MyCourse>(
                    "select * from t_My_Course where OrderId=@OrderId ", dyCourse);
            _iMyCourseRepository.Delete(myCourse);
            _iOrderSheetRepository.Update(model);
        }

        /// <summary>
        /// 修改订单价格
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut SetOrderPrice(OrderSheet input)
        {
            try
            {
                var model =
                    Db.QueryFirstOrDefault<OrderSheet>($"select * from t_Order_Sheet where OrderNo='{input.OrderNo}'",
                        null);

                if (model != null)
                {
                    _operatorLogAppService.Add(new OperatorLogInput
                    {

                        KeyId = model.Id,
                        ModuleId = (int)Model.OrderSheet,
                        OperatorType = (int)OperatorType.Edit,
                        Remark = "修改订单金额:订单编号" + model.OrderNo + ",修改前金额:" + model.OrderAmount + ",修改后金额:" + input.OrderAmount
                    });
                }

                const string sql = " update t_Order_Sheet set SetOrderUser=@SetOrderUser,SetOrderTime=@SetOrderTime,OrderAmount=@OrderAmount where OrderNo=@OrderNo ";
                var parament = new DynamicParameters(); ;
                parament.Add("SetOrderUser", UserObject.LoginName, DbType.String);
                parament.Add("SetOrderTime", DateTime.Now, DbType.DateTime);
                parament.Add("OrderNo", input.OrderNo);
                parament.Add("OrderAmount", input.OrderAmount);
                Db.ExecuteNonQuery(sql, parament);
                return new MessagesOutPut { Success = true, Message = "修改成功" };
            }
            catch (Exception ex)
            {
                return new MessagesOutPut { Success = false, Message = "修改失败" };
            }

        }

        /// <summary>
        /// 按日获取统计报表
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>

        public List<SalesReportsOutPut> GetSalesReportsDayList(SalesReportsInput Input)
        {
            var strSql = new StringBuilder(
                @"SELECT  CONVERT(CHAR(10), AddTime, 120) AS 'Date' ,
                          SUM(FactPayAmount) AS 'TotalMoney' ,
                          Max(Id) AS 'Id' ,
                          COUNT(1) AS 'tradecount'
                FROM    dbo.t_Order_Sheet Where 1=1 and  State=0
               ");
            if (!string.IsNullOrWhiteSpace(Input.StartTime))
            {
                strSql.Append(" AND addtime >= '" + Input.StartTime + " 00:00:00' ");
            }
            if (!string.IsNullOrWhiteSpace(Input.EndTime))
            {
                strSql.Append(" AND addtime <= '" + Input.EndTime + " 23:59:59' ");
            }
            var Group = "  GROUP BY CONVERT(CHAR(10), AddTime, 120) ";
            var Order = "  ORDER BY CONVERT(char(10), addtime,120) ASC ";
            return Db.QueryList<SalesReportsOutPut>(strSql + Group + Order, null);
        }

        /// <summary>
        /// 按月获取统计报表
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public List<SalesReportsOutPut> GetSalesReportsMonthList(SalesReportsInput Input)
        {
            var strSql = new StringBuilder(
                @"SELECT  CONVERT(CHAR(7), AddTime, 120) AS 'Date' ,
                          SUM(FactPayAmount) AS 'TotalMoney' ,
                          Max(Id) AS 'Id' ,
                          COUNT(1) AS 'tradecount'
                FROM    dbo.t_Order_Sheet Where 1=1  and  State=0
               ");
            if (!string.IsNullOrWhiteSpace(Input.StartTime))
            {
                strSql.Append(" AND addtime >= '" + Input.StartTime + "-01 00:00:00' ");
            }
            if (!string.IsNullOrWhiteSpace(Input.EndTime))
            {
                DateTime dtEndTime = Convert.ToDateTime(Input.EndTime + "-01");
                strSql.Append(" AND addtime <= '" + dtEndTime.AddMonths(1).ToString("yyyy-MM-dd") + " 00:00:00'");
            }
            var Group = "  GROUP BY CONVERT(CHAR(7), AddTime,120) ";
            var Order = "  ORDER BY CONVERT(char(7), AddTime,120) ASC ";
            return Db.QueryList<SalesReportsOutPut>(strSql + Group + Order, null);
        }
        /// <summary>
        /// 按年获取统计报表
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public List<SalesReportsOutPut> GetSalesReportsYearList(SalesReportsInput Input)
        {
            var strSql = new StringBuilder(
                @"SELECT  CONVERT(CHAR(4), AddTime, 120) AS 'Date' ,
                          SUM(FactPayAmount) AS 'TotalMoney' ,
                          Max(Id) AS 'Id' ,
                          COUNT(1) AS 'tradecount'
                FROM    dbo.t_Order_Sheet Where 1=1  and  State=0
               ");
            if (!string.IsNullOrWhiteSpace(Input.StartTime))
            {
                strSql.Append(" AND addtime >= '" + Input.StartTime + "-01-01 00:00:00'");
            }
            if (!string.IsNullOrWhiteSpace(Input.EndTime))
            {
                DateTime dtEndTime = Convert.ToDateTime(Input.EndTime + "-01-01");
                strSql.Append(" AND addtime <= '" + dtEndTime.AddYears(1).ToString("yyyy-MM-dd") + " 00:00:00'");
            }
            var Group = "  GROUP BY CONVERT(CHAR(4), AddTime,120) ";
            var Order = "  ORDER BY CONVERT(char(4), AddTime,120) ASC ";
            return Db.QueryList<SalesReportsOutPut>(strSql + Group + Order, null);
        }

        /// <summary>
        /// 修改订单快递公司
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut UpdateSheetExpressCompany(OrderSheetListInput input)
        {
            string msg = string.Empty;
            try
            {
                var model = Db.QueryFirstOrDefault<OrderSheet>(" select * from t_Order_Sheet where OrderNo='" + input.OrderNo + "' ", null);
                model.ExpressCompanyId = input.ExpressCompanyId;
                model.ExpressNumber = input.ExpressNumber;
                _iOrderSheetRepository.Update(model);
                msg = string.IsNullOrEmpty(input.ExpressCompanyId) ? "撤销成功" : "邮寄成功";
                return new MessagesOutPut { Success = true, Message = msg };
            }
            catch (Exception ex)
            {
                msg = string.IsNullOrEmpty(input.ExpressCompanyId) ? "撤销失败" : "邮寄失败";
                return new MessagesOutPut { Success = false, Message = msg };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <returns></returns>
        public OrderSheet GetModel(string OrderNo)
        {
            return Db.QueryFirstOrDefault<OrderSheet>($" SELECT * FROM dbo.t_Order_Sheet WHERE OrderNo='{OrderNo}'", null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <returns></returns>
        public void deleteCard(string OrderNo)
        {
            Db.ExecuteNonQuery($" DELETE dbo.t_Order_Card WHERE OrderNo ='{OrderNo}'", null);
        }

        /// <summary>
        /// 统计课程销量
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<CourseSaleReport> GetSaleReportByCourse(OrderSaleInput input)
        {
            var sql = new StringBuilder();
            if (input.CourseType == 0)
            {
                sql.Append(@"SELECT c.CourseName ,COUNT(1) AS CourseCount FROM dbo.t_Order_Detail a
                LEFT JOIN dbo.t_Order_Sheet b ON a.OrderNo=b.Id
                LEFT JOIN dbo.t_Course_Info c ON a.CourseId=c.Id
                WHERE b.State =0 AND  a.CourseType=0 AND c.IsValueAdded=0 ");
            }
            else if (input.CourseType == 1)
            {
                sql.Append(@"SELECT c.CourseName ,COUNT(1) AS CourseCount FROM dbo.t_Order_Detail a
                LEFT JOIN dbo.t_Order_Sheet b ON a.OrderNo=b.Id
                LEFT JOIN dbo.t_Course_PackcourseInfo c ON a.CourseId=c.Id
                WHERE b.State =0 AND  a.CourseType=1 ");
            }

            if (input.BeginDate.HasValue)
            {
                sql.Append(" AND b.PayTime >= '" + input.BeginDate + "' ");
            }
            if (input.EndDate.HasValue)
            {
                sql.Append(" AND b.PayTime <= '" + input.EndDate + "' ");
            }

            sql.Append(" GROUP BY a.CourseId,c.CourseName");



            return Db.QueryList<CourseSaleReport>(sql.ToString(), null);
        }

        /// <summary>
        /// 统计课程销量-分页
        /// </summary>
        /// <param name="input"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<CourseSaleReport> GetSaleReportByCourseWithPage(OrderSaleInput input, out int count)
        {
            var sql = new StringBuilder();
            if (input.CourseType == 0)
            {
                sql.Append(@"SELECT c.CourseName ,COUNT(1) AS CourseCount FROM dbo.t_Order_Detail a
                LEFT JOIN dbo.t_Order_Sheet b ON a.OrderNo=b.Id
                LEFT JOIN dbo.t_Course_Info c ON a.CourseId=c.Id
                WHERE b.State =0 AND  a.CourseType=0 AND c.IsValueAdded=0 ");
            }
            else if (input.CourseType == 1)
            {
                sql.Append(@"SELECT c.CourseName ,COUNT(1) AS CourseCount FROM dbo.t_Order_Detail a
                LEFT JOIN dbo.t_Order_Sheet b ON a.OrderNo=b.Id
                LEFT JOIN dbo.t_Course_PackcourseInfo c ON a.CourseId=c.Id
                WHERE b.State =0 AND  a.CourseType=1 ");
            }

            if (input.BeginDate.HasValue)
            {
                sql.Append(" AND b.PayTime >= '" + input.BeginDate + "' ");
            }
            if (input.EndDate.HasValue)
            {
                sql.Append(" AND b.PayTime <= '" + input.EndDate + "' ");
            }
            sql.Append(" GROUP BY a.CourseId,c.CourseName");
            count = Db.ExecuteScalar<int>(GetPageSql(sql.ToString(), input.Page, input.Rows, "CourseName"), null);



            return Db.QueryList<CourseSaleReport>(sql.ToString(), null);
        }
    }
}

