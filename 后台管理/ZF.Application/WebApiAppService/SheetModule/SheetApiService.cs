using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.WebApiDto.CartDtoModule;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using System.Data;
using ZF.Application.BaseDto;
using System.Collections.Generic;
using ZF.Application.WebApiDto.SheetDtoModule;
using Dapper;
using System;
using ZF.Infrastructure;
using ZF.Infrastructure.RandomHelper;
using ZF.Infrastructure.Enum;
using ZF.Application.WebApiDto.OrderCardModule;

namespace ZF.Application.WebApiAppService.SheetModule
{
    /// <summary>
    /// 订单服务
    /// </summary>
    public class SheetApiService : BaseAppService<OrderSheet>
    {
        private readonly ICourseInfoRepository _courseInfoRepository;
        private readonly IOrderSheetRepository _repository;

        //订单
        private readonly IOrderSheetRepository _iOrderSheetRepository;

        //我的课程
        private readonly IMyCourseRepository _iMyCourseRepository;
        //订单学习卡使用记录
        private readonly IOrderCardRepository _iOrderCardRepository;

        private readonly IUserDiscountCardRepository _userDiscountCardRepository;

        private readonly string defuleDomain = ConfigurationManager.AppSettings["DefuleDomain"];

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public SheetApiService(IOrderSheetRepository repository, IOrderSheetRepository iOrderSheetRepository, IMyCourseRepository iMyCourseRepository, IOrderCardRepository iOrderCardRepository, ICourseInfoRepository courseInfoRepository, IUserDiscountCardRepository userDiscountCardRepository) : base(repository)
        {
            _repository = repository;
            _iOrderSheetRepository = iOrderSheetRepository;
            _iMyCourseRepository = iMyCourseRepository;
            _iOrderCardRepository = iOrderCardRepository;
            _courseInfoRepository = courseInfoRepository;
            _userDiscountCardRepository = userDiscountCardRepository;
        }


        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut DelSheet(IdInput input)
        {
            try
            {
                var model = new OrderSheet();
                model.Id = input.Id;
                const string sql = " delete from t_Order_Sheet where OrderNo = @OrderNo; delete from t_Order_Card where OrderNo=@OrderNo ";
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add(":OrderNo", input.Id, DbType.String);
                Db.ExecuteNonQuery(sql, dynamicParameters);
                return new MessagesOutPut { Message = "删除成功", Success = true };
            }
            catch
            {
                return new MessagesOutPut { Message = "删除失败", Success = false };
            }
        }

        /// <summary>
        /// 卡卷使用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut EnditDiscountCard(SheetModelInput input)
        {
            #region  写入订单学习卡使用表
            if (!string.IsNullOrEmpty(input.DiscountCard))
            {
                var cardArr = input.DiscountCard.TrimEnd(',').Split(',');
                try
                {
                    foreach (var cardCode in cardArr)
                    {
                        _iOrderCardRepository.Insert(new OrderCard { Id = Guid.NewGuid().ToString(), OrderNo = input.OrderNo, CardCode = cardCode, State = 0 });
                    }
                    return new MessagesOutPut { Success = true };
                }
                catch (Exception)
                {
                    return new MessagesOutPut { Success = false };
                }
            }
            else
            {
                return new MessagesOutPut { Success = true };
            }
            #endregion
        }


        /// <summary>
        /// 订单写入后返回结果集
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<SheetModelOutput> SheetAddToList(SheetModelInput input)
        {
            var list = new List<SheetModelOutput>();
            string sql = string.Empty;

            if (string.IsNullOrEmpty(input.OrderNo))
            {


                #region 根据明细id查询购物车订单表
                sql = " select Id,OrderNo,RegisterUserId from t_Order_Cart a where exists(select 1 from t_Order_CartDetail where a.Id = OrderNo and Id in (" + input.Id + ") ) ";
                var cartList = Db.QueryFirstOrDefault<OrderCart>(sql, null);
                var sheetId = Guid.NewGuid().ToString();
                #endregion

                if (cartList == null)
                {
                    try
                    {
                        sql = " select Id,OrderNo,RegisterUserId,CourseId,FavourablePrice,Num,Amount,CourseName,'" + defuleDomain + "/'+CourseIamge as CourseIamge,OrderAmount,ValidityPeriod,EmailNotes,ValidityEndDate  from v_SheetInfo where RegisterUserId = @RegisterUserId and Remark=@Remark ";
                        var dynamicparameters = new DynamicParameters();
                        dynamicparameters.Add(":RegisterUserId", input.RegisterUserId, DbType.String);
                        dynamicparameters.Add(":Remark", input.Id, DbType.String);
                        list = Db.QueryList<SheetModelOutput>(sql, dynamicparameters);
                    }
                    catch
                    {

                        return list;
                    }
                }
                else
                {

                    #region 写入订单主表
                    try
                    {
                        #region 先删后写

                        sql = string.Format("select id, OrderNo,CourseId,Price,FavourablePrice,Num,Amount,CourseType from  t_Order_CartDetail  where Id in ({0})", input.Id);

                        var cartDetail = Db.QueryList<OrderCartDetail>(sql, null);

                        StringBuilder sqlList = new StringBuilder();
                        foreach (var item in input.CarDetailIdList.Split(','))
                        {
                            string[] parament = item.Split('+');
                            OrderCartDetail info = (from data in cartDetail where data.Id == parament[0].ToString() select data).LastOrDefault<OrderCartDetail>();
                            double sumPrice = Convert.ToDouble(Convert.ToDouble(info.FavourablePrice) * Convert.ToDouble(parament[1]));
                            sqlList.AppendFormat(@" insert into t_Order_Detail(id, OrderNo,CourseId,Price,FavourablePrice,Num,Amount,CourseType)
                                                values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}'); ", Guid.NewGuid().ToString(), sheetId, info.CourseId, info.Price, info.FavourablePrice, parament[1], sumPrice, info.CourseType);
                        }
                        sqlList.AppendFormat(" delete from t_Order_CartDetail where id in ({0}) ", input.Id);
                        var count = Db.ExecuteNonQuery(sqlList.ToString(), null);
                        #endregion

                        var orderAmount = new decimal();
                        var sum = cartDetail.Sum(x => x.FavourablePrice);
                        if (sum != null) orderAmount = sum.Value;

                        var orderSheet = new OrderSheet();
                        orderSheet.Id = sheetId;
                        orderSheet.OrderNo = DateTime.Now.ToString("yyyyMMddHHmmssfff") + RandomHelper.GetRandom(3, 1)[0].ToString();
                        orderSheet.RegisterUserId = cartList.RegisterUserId;
                        orderSheet.AddTime = DateTime.Now;
                        orderSheet.OrderAmount = input.OrderAmount;
                        orderSheet.State = (int)OrderState.GenerationPayment;
                        orderSheet.FactPayAmount = 0;
                        orderSheet.OrderIp = input.OrderIp.Contains(",") ? input.OrderIp.Split(',')[1] : input.OrderIp;
                        orderSheet.OrderType = input.OrderType;
                        orderSheet.PayType = "";
                        orderSheet.PromotionCode = input.PromotionCode;//新增推广码
                        orderSheet.InstitutionsId = input.InstitutionsId;//机构编号
                        orderSheet.Remark = input.Id;
                        orderSheet.InvoiceTime = Convert.ToDateTime("1900-01-01 00:00:00.000");
                        _iOrderSheetRepository.Insert(orderSheet);

                        //查询订单信息
                        sql = " select Id,OrderNo,RegisterUserId,CourseId,FavourablePrice,Num,Amount,CourseName,'" + defuleDomain + "/'+CourseIamge as CourseIamge,OrderAmount,ValidityPeriod,EmailNotes,ValidityEndDate  from v_SheetInfo where RegisterUserId = @RegisterUserId and CartId=@CartId ";
                        var dynamicparameters = new DynamicParameters();
                        dynamicparameters.Add(":RegisterUserId", cartList.RegisterUserId, DbType.String);
                        dynamicparameters.Add(":CartId", sheetId, DbType.String);
                        list = Db.QueryList<SheetModelOutput>(sql, dynamicparameters);

                        //#region  写入订单学习卡使用表
                        //if (!string.IsNullOrEmpty(input.DiscountCard))
                        //{
                        //    var cardArr = input.DiscountCard.TrimEnd(',').Split(',');
                        //    foreach (var cardCode in cardArr)
                        //    {
                        //        _iOrderCardRepository.Insert(new OrderCard { Id = Guid.NewGuid().ToString(), OrderNo = orderSheet.OrderNo, CardCode = cardCode, State = 0 });
                        //    }
                        //}
                        //#endregion

                    }
                    catch (Exception ex)
                    {
                        return list;
                    }
                }
            }
            else //从我的订单页过来
            {
                try
                {
                    sql = " select Id,OrderNo,RegisterUserId,CourseId,FavourablePrice,Num,Amount,CourseName,'" + defuleDomain + "/'+CourseIamge as CourseIamge,OrderAmount,ValidityPeriod,EmailNotes,ValidityEndDate  from v_SheetInfo where RegisterUserId = @RegisterUserId and OrderNo=@OrderNo ";
                    var dynamicparameters = new DynamicParameters();
                    dynamicparameters.Add(":RegisterUserId", input.RegisterUserId, DbType.String);
                    dynamicparameters.Add(":OrderNo", input.OrderNo, DbType.String);
                    list = Db.QueryList<SheetModelOutput>(sql, dynamicparameters);
                }
                catch
                {
                    return list;
                }
            }
            #endregion

            return list;
        }


        /// <summary>
        /// 订单写入后返回结果集
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<SheetModelOutput> SheetAddToListByWiki(SheetModelInput input)
        {
            var list = new List<SheetModelOutput>();
            string sql = string.Empty;

            if (string.IsNullOrEmpty(input.OrderNo))
            {


                #region 根据明细id查询购物车订单表
                sql = " select Id,OrderNo,RegisterUserId from t_Order_Cart a where exists(select 1 from t_Order_CartDetail where a.Id = OrderNo and Id in (" + input.Id + ") ) ";
                var cartList = Db.QueryFirstOrDefault<OrderCart>(sql, null);
                var sheetId = Guid.NewGuid().ToString();
                #endregion

                if (cartList == null)
                {
                    try
                    {
                        sql = " select Id,OrderNo,RegisterUserId,CourseId,FavourablePrice,Num,Amount,CourseName,'" + defuleDomain + "/'+CourseIamge as CourseIamge,OrderAmount,ValidityPeriod,EmailNotes,ValidityEndDate  from v_SheetInfo where RegisterUserId = @RegisterUserId and Remark=@Remark ";
                        var dynamicparameters = new DynamicParameters();
                        dynamicparameters.Add(":RegisterUserId", input.RegisterUserId, DbType.String);
                        dynamicparameters.Add(":Remark", input.Id, DbType.String);
                        list = Db.QueryList<SheetModelOutput>(sql, dynamicparameters);
                    }
                    catch
                    {

                        return list;
                    }
                }
                else
                {

                    #region 写入订单主表
                    try
                    {
                        #region 先删后写

                        sql = string.Format("select id, OrderNo,CourseId,Price,FavourablePrice,Num,Amount,CourseType from  t_Order_CartDetail  where Id in ({0})", input.Id);

                        var cartDetail = Db.QueryList<OrderCartDetail>(sql, null);

                        StringBuilder sqlList = new StringBuilder();
                        foreach (var item in input.CarDetailIdList.Split(','))
                        {
                            string[] parament = item.Split('+');
                            OrderCartDetail info = (from data in cartDetail where data.Id == parament[0].ToString() select data).LastOrDefault<OrderCartDetail>();
                            double sumPrice = Convert.ToDouble(Convert.ToDouble(info.FavourablePrice) * Convert.ToDouble(parament[1]));
                            sqlList.AppendFormat(@" insert into t_Order_Detail(id, OrderNo,CourseId,Price,FavourablePrice,Num,Amount,CourseType)
                                                values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}'); ", Guid.NewGuid().ToString(), sheetId, info.CourseId, info.Price, info.FavourablePrice, parament[1], sumPrice, info.CourseType);
                        }
                        sqlList.AppendFormat(" delete from t_Order_CartDetail where id in ({0}) ", input.Id);
                        var count = Db.ExecuteNonQuery(sqlList.ToString(), null);
                        #endregion

                        var orderAmount = input.OrderAmount;

                        var orderSheet = new OrderSheet();
                        orderSheet.Id = sheetId;
                        orderSheet.OrderNo = DateTime.Now.ToString("yyyyMMddHHmmssfff") + RandomHelper.GetRandom(3, 1)[0].ToString();
                        orderSheet.RegisterUserId = cartList.RegisterUserId;
                        orderSheet.AddTime = DateTime.Now;
                        orderSheet.OrderAmount = orderAmount;
                        orderSheet.State = (int)OrderState.GenerationPayment;
                        orderSheet.FactPayAmount = 0;
                        orderSheet.OrderIp = input.OrderIp.Contains(",") ? input.OrderIp.Split(',')[1] : input.OrderIp;
                        orderSheet.OrderType = input.OrderType;
                        orderSheet.PayType = "";
                        orderSheet.PromotionCode = input.PromotionCode;//新增推广码
                        orderSheet.InstitutionsId = input.InstitutionsId;//机构编号
                        orderSheet.Remark = input.Id;
                        orderSheet.InvoiceTime = Convert.ToDateTime("1900-01-01 00:00:00.000");
                        _iOrderSheetRepository.Insert(orderSheet);

                        //查询订单信息
                        sql = " select Id,OrderNo,RegisterUserId,CourseId,FavourablePrice,Num,Amount,CourseName,'" + defuleDomain + "/'+CourseIamge as CourseIamge,OrderAmount,ValidityPeriod,EmailNotes,ValidityEndDate  from v_SheetInfo where RegisterUserId = @RegisterUserId and CartId=@CartId ";
                        var dynamicparameters = new DynamicParameters();
                        dynamicparameters.Add(":RegisterUserId", cartList.RegisterUserId, DbType.String);
                        dynamicparameters.Add(":CartId", sheetId, DbType.String);
                        list = Db.QueryList<SheetModelOutput>(sql, dynamicparameters);

                        //#region  写入订单学习卡使用表
                        //if (!string.IsNullOrEmpty(input.DiscountCard))
                        //{
                        //    var cardArr = input.DiscountCard.TrimEnd(',').Split(',');
                        //    foreach (var cardCode in cardArr)
                        //    {
                        //        _iOrderCardRepository.Insert(new OrderCard { Id = Guid.NewGuid().ToString(), OrderNo = orderSheet.OrderNo, CardCode = cardCode, State = 0 });
                        //    }
                        //}
                        //#endregion

                    }
                    catch (Exception ex)
                    {
                        return list;
                    }
                }
            }
            else //从我的订单页过来
            {
                try
                {
                    sql = " select Id,OrderNo,RegisterUserId,CourseId,FavourablePrice,Num,Amount,CourseName,'" + defuleDomain + "/'+CourseIamge as CourseIamge,OrderAmount,ValidityPeriod,EmailNotes,ValidityEndDate  from v_SheetInfo where RegisterUserId = @RegisterUserId and OrderNo=@OrderNo ";
                    var dynamicparameters = new DynamicParameters();
                    dynamicparameters.Add(":RegisterUserId", input.RegisterUserId, DbType.String);
                    dynamicparameters.Add(":OrderNo", input.OrderNo, DbType.String);
                    list = Db.QueryList<SheetModelOutput>(sql, dynamicparameters);
                }
                catch
                {
                    return list;
                }
            }
            #endregion

            return list;
        }

        /// <summary>
        /// 订单写入后返回结果集
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cartId"></param>
        /// <returns></returns>
        public List<SheetModelOutput> SheetAddToListNoLogin(SheetModelInput input)
        {
            var list = new List<SheetModelOutput>();
            string sql = string.Empty;

            if (string.IsNullOrEmpty(input.OrderNo))
            {
                #region 根据明细id查询购物车订单表
                sql = " select Id,OrderNo,RegisterUserId from t_Order_Cart a where exists(select 1 from t_Order_CartDetail where a.Id = OrderNo and NologinGroup in ('" + input.Id + "') ) ";
                var cartList = Db.QueryFirstOrDefault<OrderCart>(sql, null);
                var sheetId = Guid.NewGuid().ToString();
                #endregion

                if (cartList == null)
                {
                    try
                    {
                        sql = " select Id,OrderNo,RegisterUserId,CourseId,FavourablePrice,Num,Amount,CourseName,'" + defuleDomain + "/'+CourseIamge as CourseIamge,OrderAmount,ValidityPeriod,EmailNotes,ValidityEndDate  from v_SheetInfo where RegisterUserId = @RegisterUserId and Remark=@Remark ";
                        var dynamicparameters = new DynamicParameters();
                        dynamicparameters.Add(":RegisterUserId", input.RegisterUserId, DbType.String);
                        dynamicparameters.Add(":Remark", input.Id, DbType.String);
                        list = Db.QueryList<SheetModelOutput>(sql, dynamicparameters);
                    }
                    catch
                    {
                        return list;
                    }
                }
                else
                {
                    #region 写入订单主表
                    try
                    {
                        #region 先删后写

                        sql = string.Format("select id, OrderNo,CourseId,Price,FavourablePrice,Num,Amount,CourseType from  t_Order_CartDetail  where NologinGroup in ('{0}')", input.Id);

                        var cartDetail = Db.QueryList<OrderCartDetail>(sql, null);

                        StringBuilder sqlList = new StringBuilder();
                        foreach (var item in cartDetail)
                        {
                            double sumPrice = Convert.ToDouble(item.FavourablePrice);
                            sqlList.AppendFormat(@" insert into t_Order_Detail(id, OrderNo,CourseId,Price,FavourablePrice,Num,Amount,CourseType)
                                                values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}'); ", Guid.NewGuid().ToString(), sheetId, item.CourseId, item.Price, item.FavourablePrice, 1, sumPrice, item.CourseType);
                        }
                        sqlList.AppendFormat(" delete from t_Order_CartDetail where NologinGroup in ('{0}') ", input.Id);
                        var count = Db.ExecuteNonQuery(sqlList.ToString(), null);
                        #endregion

                        var orderAmount = new decimal();
                        var sum = cartDetail.Sum(x => x.FavourablePrice);
                        if (sum != null) orderAmount = sum.Value;

                        var orderSheet = new OrderSheet();
                        orderSheet.Id = sheetId;
                        orderSheet.OrderNo = DateTime.Now.ToString("yyyyMMddHHmmssfff") + RandomHelper.GetRandom(3, 1)[0].ToString();
                        orderSheet.RegisterUserId = cartList.RegisterUserId;
                        orderSheet.AddTime = DateTime.Now;
                        orderSheet.OrderAmount = input.OrderAmount;
                        orderSheet.State = (int)OrderState.GenerationPayment;
                        orderSheet.PromotionCode = input.PromotionCode;//新增推广码
                        orderSheet.InstitutionsId = input.InstitutionsId;//机构编号
                        orderSheet.FactPayAmount = 0;
                        orderSheet.OrderIp = input.OrderIp.Contains(",") ? input.OrderIp.Split(',')[1] : input.OrderIp;
                        orderSheet.OrderType = input.OrderType;
                        orderSheet.PayType = "";
                        orderSheet.Remark = input.Id;
                        orderSheet.InvoiceTime = Convert.ToDateTime("1900-01-01 00:00:00.000");
                        _iOrderSheetRepository.Insert(orderSheet);

                        //查询订单信息
                        sql = " select Id,OrderNo,RegisterUserId,CourseId,FavourablePrice,Num,Amount,CourseName,'" + defuleDomain + "/'+CourseIamge as CourseIamge,OrderAmount,ValidityPeriod,EmailNotes,ValidityEndDate  from v_SheetInfo where RegisterUserId = @RegisterUserId and CartId=@CartId ";
                        var dynamicparameters = new DynamicParameters();
                        dynamicparameters.Add(":RegisterUserId", cartList.RegisterUserId, DbType.String);
                        dynamicparameters.Add(":CartId", sheetId, DbType.String);
                        list = Db.QueryList<SheetModelOutput>(sql, dynamicparameters);



                    }
                    catch (Exception ex)
                    {
                        return list;
                    }
                }
            }
            else //从我的订单页过来
            {
                try
                {
                    sql = " select Id,OrderNo,RegisterUserId,CourseId,FavourablePrice,Num,Amount,CourseName,'" + defuleDomain + "/'+CourseIamge as CourseIamge,OrderAmount,ValidityPeriod,EmailNotes,ValidityEndDate  from v_SheetInfo where RegisterUserId = @RegisterUserId and OrderNo=@OrderNo ";
                    var dynamicparameters = new DynamicParameters();
                    dynamicparameters.Add(":RegisterUserId", input.RegisterUserId, DbType.String);
                    dynamicparameters.Add(":OrderNo", input.OrderNo, DbType.String);
                    list = Db.QueryList<SheetModelOutput>(sql, dynamicparameters);
                }
                catch
                {
                    return list;
                }
            }
            #endregion

            return list;
        }

        /// <summary>
        /// 修改讲义收货地址
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut EnditiSheetDeliveryAddRess(SheetModelInput input)
        {
            const string sql = " select * from t_Order_Sheet where OrderNo=@OrderNo ";
            var parament = new DynamicParameters();
            parament.Add(":OrderNo", input.OrderNo, DbType.String);
            OrderSheet model = Db.QueryFirstOrDefault<OrderSheet>(sql, parament);
            model.HandOutId = input.HandOutId;
            try
            {
                _repository.Update(model);
                return new MessagesOutPut { Success = true, Message = "讲义地址修改成功" };
            }
            catch (Exception)
            {
                return new MessagesOutPut { Success = false, Message = "讲义地址修改失败" };
            }
        }

        /// <summary>
        /// 获取用户最近一次写入的电子发票信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public SheetModelOutput GetElectronicInvoiceByUserId(string userId)
        {
            const string sql = @" select top 1 InvoiceHeader,InvoiceMailbox,TaxpayerIdentificationNumber from t_Order_Sheet 
                                  where RegisterUserid = @RegisterUserid
                                   and(isnull(InvoiceHeader, '') <> '' or isnull(InvoiceMailbox, '') <> '' or isnull(TaxpayerIdentificationNumber, '') <> '')
                                  order by addtime desc ";
            var parament = new DynamicParameters();
            parament.Add(":RegisterUserid", userId, DbType.String);
            SheetModelOutput model = Db.QueryFirstOrDefault<SheetModelOutput>(sql, parament);
            return model;
        }

        /// <summary>
        /// 写入电子发票信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut EnditiSheetElectronicInvoice(SheetModelInput input)
        {
            const string sql = "  select * from t_Order_Sheet where OrderNo = @OrderNo ";
            var parament = new DynamicParameters();
            parament.Add(":OrderNo", input.OrderNo, DbType.String);
            var model = Db.QueryFirstOrDefault<OrderSheet>(sql, parament);
            model.InvoiceHeader = input.InvoiceHeader;
            model.InvoiceMailbox = input.InvoiceMailbox;
            model.TaxpayerIdentificationNumber = input.TaxpayerIdentificationNumber;
            model.InvoicePhone = input.InvoicePhone;
            model.InvoiceTime = DateTime.Now;
            try
            {
                _repository.Update(model);
                return new MessagesOutPut { Success = true };
            }
            catch (Exception ex)
            {
                return new MessagesOutPut { Success = false, Message = "电子发票写入失败" };
            }
        }


        /// <summary>
        /// 用户重新下单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut ReOrder(SheetModelInput input)
        {
            string sql = string.Empty;
            sql = " select Id,OrderNo,CourseId,Price,FavourablePrice,Num,Amount,CourseType from t_Order_Detail a where  exists(select 1 from t_Order_Sheet where a.OrderNo = Id and OrderNo=@OrderNo) ";
            var dynamicparameters = new DynamicParameters();
            dynamicparameters.Add(":OrderNo", input.OrderNo, DbType.String);
            var list = Db.QueryList<SheetModelOutput>(sql, dynamicparameters);
            StringBuilder sqlList = new StringBuilder();
            var sheetId = Guid.NewGuid().ToString();
            var orderNo = DateTime.Now.ToString("yyyyMMddHHmmssfff") + RandomHelper.GetRandom(3, 1)[0].ToString();

            //获取商品金额
            sql = " select Id,CoursePrice,CourseAmout from v_SheetInfoByOrder where OrderNo=@OrderNo ";
            var courseList = Db.QueryList<SheetModelOutput>(sql, dynamicparameters);

            var sumAmount = courseList.Sum(sumdata => sumdata.CourseAmout);
            foreach (var item in list)
            {
                SheetModelOutput courseInfo = (from info in courseList where info.Id == item.Id select info).FirstOrDefault<SheetModelOutput>();
                sqlList.AppendFormat(" insert into t_Order_Detail select '{0}','{1}',CourseId,Price,{3},Num,{4},CourseType from t_Order_Detail where Id='{2}' "
                    , Guid.NewGuid().ToString(), sheetId, item.Id, courseInfo.CoursePrice, courseInfo.CourseAmout);
            }

            sqlList.AppendFormat(@" insert into t_Order_Sheet select '{0}','{1}',RegisterUserId,'{2}',{5},{3},FactPayAmount,OrderIp,PayType,PayTime,OrderType,TradeNo,Remark,HandOutId,ExpressCompanyId,ExpressNumber,InvoiceHeader,InvoiceMailbox,TaxpayerIdentificationNumber from t_Order_Sheet where OrderNo='{4}'; "
                                , sheetId, orderNo, DateTime.Now, 1, input.OrderNo, sumAmount);

            var count = Db.ExecuteNonQuery(sqlList.ToString(), null);
            if (count > 0)
            {
                return new MessagesOutPut { Success = true, Message = "重新下单成功" };
            }
            else
            {
                return new MessagesOutPut { Success = true, Message = "重新下单失败" };
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
        /// 我的订单
        /// </summary>
        /// <param name="input"></param>
        /// <param name="count"></param>
        /// <returns></returns>

        public List<SheetModelPageOutput> PageSheetList(SheetModelInput input, out int count)
        {
            const string sql = " select Id,OrderNo,RegisterUserId,CourseId,FavourablePrice,Num,Amount,CourseName,CourseIamge,OrderAmount,ValidityPeriod,ValidityEndDate,AddTime,State,CourseType from v_SheetInfoByOrder ";
            var strSql = @"  where 1=1 and RegisterUserId=@RegisterUserId ";
            const string sqlCount = " select count(*) from t_Order_Sheet ";

            const string sqlSheet = " select * from t_Order_Sheet ";

            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add(":RegisterUserId", input.RegisterUserId);
            if (!string.IsNullOrEmpty(input.State))
            {
                strSql += " and State in (" + input.State + ") ";
                //dynamicParameters.Add(":State",  input.State, DbType.String);
            }

            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<SheetModelOutput>(GetPageSql(sqlSheet + strSql,
                dynamicParameters,
                input.Page,
                input.Rows,
                input.Sidx,
                input.Sord), dynamicParameters);


            var sheetList = Db.QueryList<SheetModelOutput>(sql + strSql, dynamicParameters);
            List<SheetModelPageOutput> sheetmodelpageoutputList = new List<SheetModelPageOutput>();
            List<string> orderNoList = new List<string>();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (!orderNoList.Contains(item.OrderNo))
                    {
                        decimal? sumprice = 0;
                        var detail = sheetList.Where(data => data.OrderNo == item.OrderNo).ToList();
                        SheetModelPageOutput sheetmodelpageoutput = new SheetModelPageOutput()
                        {
                            OrderNo = item.OrderNo,
                            RegisterUserId = item.RegisterUserId,
                            AddTime = Convert.ToDateTime(item.AddTime).ToString("yyyy-MM-dd HH:mm"),
                            OrderAmount = item.OrderAmount,
                            State = item.State,
                            FactPayAmount = item.FactPayAmount,
                            StateVal = GetStateVal(item.State),
                            sheetmodelpagedetailoutput = OutSheetPageDetailList(detail,out sumprice)
                        };
                        sheetmodelpageoutput.FavourablePrice = sumprice;
                        sheetmodelpageoutputList.Add(sheetmodelpageoutput);
                        orderNoList.Add(item.OrderNo);
                    }
                }
            }
            return sheetmodelpageoutputList;
        }

        /// <summary>
        /// 订单-wechat
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<SheetModelPageOutput> WcChatSheetList(SheetModelInput input)
        {
            const string sql = " select Id,OrderNo,RegisterUserId,CourseId,FavourablePrice,Num,Amount,CourseName,CourseIamge,OrderAmount,ValidityPeriod,ValidityEndDate,AddTime,State,CourseType from v_SheetInfoByOrder ";
            var strSql = @"  where 1=1 and RegisterUserId=@RegisterUserId ";
            const string sqlSheet = " select * from t_Order_Sheet ";
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add(":RegisterUserId", input.RegisterUserId);
            if (!string.IsNullOrEmpty(input.State))
            {
                strSql += " and State in (" + input.State + ") ";
            }

            strSql += " ORDER BY AddTime desc ";
            var list = Db.QueryList<SheetModelOutput>(sqlSheet + strSql, dynamicParameters);
            var sheetList = Db.QueryList<SheetModelOutput>(sql + strSql, dynamicParameters);
            List<SheetModelPageOutput> sheetmodelpageoutputList = new List<SheetModelPageOutput>();
            List<string> orderNoList = new List<string>();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (!orderNoList.Contains(item.OrderNo))
                    {
                        var detail = sheetList.Where(data => data.OrderNo == item.OrderNo).ToList();
                        decimal? sumprice = 0;
                        SheetModelPageOutput sheetmodelpageoutput = new SheetModelPageOutput()
                        {
                            OrderNo = item.OrderNo,
                            RegisterUserId = item.RegisterUserId,
                            AddTime = Convert.ToDateTime(item.AddTime).ToString("yyyy-MM-dd HH:mm"),
                            OrderAmount = item.OrderAmount,
                            State = item.State,
                            FactPayAmount = item.FactPayAmount,
                            StateVal = GetStateVal(item.State),
                            sheetmodelpagedetailoutput = OutSheetPageDetailList(detail,out sumprice)
                        };
                        sheetmodelpageoutputList.Add(sheetmodelpageoutput);
                        orderNoList.Add(item.OrderNo);
                    }
                }
            }
            return sheetmodelpageoutputList;
        }

        private string GetStateVal(int state)
        {
            var stateVal = string.Empty;
            switch (state)
            {
                case (int)OrderState.PaymentHasBeen:
                    stateVal = EnumHelper.GetDescription(OrderMessage.TradingSuccess);
                    break;
                case (int)OrderState.GenerationPayment:
                    stateVal = EnumHelper.GetDescription(OrderState.GenerationPayment);
                    break;
                case (int)OrderState.PaymentFailure:
                    stateVal = EnumHelper.GetDescription(OrderMessage.TradingFailure);
                    break;
                case (int)OrderState.Canceled:
                    stateVal = EnumHelper.GetDescription(OrderState.Canceled);
                    break;
                case (int)OrderState.HaveARefund:
                    stateVal = EnumHelper.GetDescription(OrderState.HaveARefund);
                    break;
                case (int)OrderState.Invalid:
                    stateVal = EnumHelper.GetDescription(OrderState.Invalid);
                    break;
            }
            return stateVal;
        }

        private List<SheetModelPageDetailOutput> OutSheetPageDetailList(List<SheetModelOutput> input,out decimal? sumprice)
        {
            List<SheetModelPageDetailOutput> sheetmodelpagedetailoutputList = new List<SheetModelPageDetailOutput>();
            sumprice = 0;
            foreach (var item in input)
            {
                try
                {
                    sumprice += item.FavourablePrice;
                }
                catch (Exception)
                {

                    sumprice += 0;
                }
                SheetModelPageDetailOutput sheetmodelpagedetailoutput = new SheetModelPageDetailOutput()
                {
                    Id = item.Id,
                    DetailOrderNo = item.DetailOrderNo,
                    CourseId = item.CourseId,
                    Price = item.Price,
                    FavourablePrice = item.FavourablePrice,
                    Num = item.Num,
                    Amount = item.Amount,
                    CourseType = item.CourseType,
                    CourseName = item.CourseName,
                    ValidityPeriod = item.ValidityPeriod,
                    ValidityEndDate = item.ValidityEndDate,
                    CourseIamge = defuleDomain + "/" + item.CourseIamge
                };
                sheetmodelpagedetailoutputList.Add(sheetmodelpagedetailoutput);
            }
            return sheetmodelpagedetailoutputList;
        }

        /// <summary>
        /// 根据支付宝支付状态修改订单状态
        /// </summary>
        /// <param name="orderno"></param>
        /// <param name="state"></param>
        /// <param name="totalamount"></param>
        public void EnditSheetState(string orderno, string totalamount, int state)
        {
            string sql = string.Empty;
            var dy = new DynamicParameters();


            //当订单状态为已支付，写入我的课程
            try
            {
                dy = new DynamicParameters();
                dy.Add(":OrderNo", orderno, DbType.String);
                OrderSheet model = Db.QueryFirstOrDefault<OrderSheet>("select  * from t_Order_Sheet where OrderNo = @OrderNo ", dy);
                if (state == (int)OrderState.PaymentHasBeen)
                {
                    sql = "select CourseId,RegisterUserId,ValidityPeriod,Num,CourseType,ValidityEndDate from v_SheetInfoByOrder where OrderNo = @OrderNo ";
                    dy = new DynamicParameters();
                    dy.Add(":OrderNo", orderno, DbType.String);
                    var list = Db.QueryList<SheetModelOutput>(sql, dy);
                    StringBuilder sqlList = new StringBuilder();
                    var gid = string.Empty;
                    foreach (var item in list)
                    {
                        //添加到我的课程的课程
                        if (item.Num != null)
                        {
                            #region 写入赠送题库到我的课程
                            //获取单个课程下的赠送题库集合
                            var linkCourseSql = $"select * from t_Course_Info where 1=1 and IsDelete=0  and Type=1 and LinkCourse='{item.CourseId}'";
                            var linkCourseList = Db.QueryList<CourseInfo>(linkCourseSql);
                            foreach (var itemLinkCourse in linkCourseList)
                            {
                                var courseMode = _courseInfoRepository.Get(itemLinkCourse.Id);
                                var endTime1 = courseMode.ValidityEndDate.HasValue
                                    ? courseMode.ValidityEndDate
                                    : DateTime.Now.AddYears(100);
                                //判断是否购买过该课程
                                var strSq2 = @" SELECT * FROM dbo.t_My_Course WHERE CourseId=@CourseId AND UserId =@UserId ";
                                var dy2 = new DynamicParameters();
                                dy2.Add(":CourseId", itemLinkCourse.Id, DbType.String);
                                dy2.Add(":UserId", item.RegisterUserId, DbType.String);
                                var model2 = Db.QueryFirstOrDefault<MyCourse>(strSq2, dy2);
                                if (model2 == null)
                                {
                                    sqlList.AppendFormat(" insert into t_My_Course(Id,UserId,CourseId,AddTime,BeginTime,EndTime,OrderId,CourseType)values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                                        Guid.NewGuid(), item.RegisterUserId, itemLinkCourse.Id, DateTime.Now, DateTime.Now, endTime1, model.Id, 0);
                                }
                                else
                                {
                                    if (model2.EndTime != null)
                                        model2.EndTime = endTime1;
                                    _iMyCourseRepository.Update(model2);
                                }
                            }
                            #endregion
                            #region 写入购买课程到我的课程
                            //  var courseMode1 = _courseInfoRepository.Get(item.CourseId);
                            var endTime = item.ValidityEndDate.HasValue
                                    ? item.ValidityEndDate
                                    : DateTime.Now.AddYears(100);
                            gid = Guid.NewGuid().ToString();

                            //判断是否购买过该课程
                            var strSql = @" SELECT * FROM dbo.t_My_Course WHERE CourseId=@CourseId AND UserId =@UserId ";
                            var dy1 = new DynamicParameters();
                            dy1.Add(":CourseId", item.CourseId, DbType.String);
                            dy1.Add(":UserId", item.RegisterUserId, DbType.String);
                            var model1 = Db.QueryFirstOrDefault<MyCourse>(strSql, dy1);
                            if (model1 == null)
                            {
                                sqlList.AppendFormat(" insert into t_My_Course(Id,UserId,CourseId,AddTime,BeginTime,EndTime,OrderId,CourseType)values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                                    gid, item.RegisterUserId, item.CourseId, DateTime.Now, DateTime.Now, endTime, model.Id, item.CourseType);
                            }
                            else
                            {
                                if (model1.EndTime != null)
                                    model1.EndTime = endTime;
                                _iMyCourseRepository.Update(model1);
                            }
                            #endregion
                            #region 判断是否是套餐课程  如果是套餐课程则需要把子课程相关的题库写到我的课程
                            if (item.CourseType == 1)
                            {
                                //获取单个套餐课程下的子课程集合
                                var courseListSql = $"SELECT b.* FROM dbo.t_Course_SuitDetail  a LEFT JOIN dbo.t_Course_Info b ON a.CourseId=b.Id  WHERE 1=1 AND b.IsDelete=0 AND a.PackCourseId='{item.CourseId}'";
                                var courseList = Db.QueryList<CourseInfo>(courseListSql);
                                foreach (var courseModel in courseList)
                                {
                                    var linkCourseSql1 = $"select * from t_Course_Info where 1=1 and IsDelete=0  and Type=1 and LinkCourse='{courseModel.Id}'";
                                    var linkCourseList1 = Db.QueryList<CourseInfo>(linkCourseSql1);
                                    foreach (var itemLinkCourse in linkCourseList1)
                                    {
                                        var courseMode = _courseInfoRepository.Get(itemLinkCourse.Id);
                                        //获取课程的有效日期  如果设置了则有效日期到设置的日期   若没有则有效日期为永久  如果后台有效日期为必填 则该判断没有任何意义
                                        var endTime1 = courseMode.ValidityEndDate.HasValue
                                            ? courseMode.ValidityEndDate
                                            : DateTime.Now.AddYears(100);
                                        //判断是否购买过该课程
                                        var strSq2 = @" SELECT * FROM dbo.t_My_Course WHERE CourseId=@CourseId AND UserId =@UserId ";
                                        var dy2 = new DynamicParameters();
                                        dy2.Add(":CourseId", itemLinkCourse.Id, DbType.String);
                                        dy2.Add(":UserId", item.RegisterUserId, DbType.String);
                                        var model2 = Db.QueryFirstOrDefault<MyCourse>(strSq2, dy2);
                                        //判断该课程是否有购买过  有购买则更新有效日期 无则插入我的课程
                                        if (model2 == null)
                                        {
                                            sqlList.AppendFormat(" insert into t_My_Course(Id,UserId,CourseId,AddTime,BeginTime,EndTime,OrderId,CourseType)values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                                                Guid.NewGuid(), item.RegisterUserId, itemLinkCourse.Id, DateTime.Now, DateTime.Now, endTime1, model.Id, 0);
                                        }
                                        else
                                        {
                                            if (model2.EndTime != null)
                                                model2.EndTime = endTime1;
                                            _iMyCourseRepository.Update(model2);
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                    if (sqlList.Length > 0)
                    {
                        Db.ExecuteNonQuery(sqlList.ToString(), null);
                    }
                    #region 已付款，根据订单号找到学习卡，并更新用户学习卡状态
                    var strsql = " SELECT * FROM dbo.t_Order_Card WHERE OrderNo=@OrderNo  ";
                    var pa = new DynamicParameters();
                    pa.Add(":OrderNo", orderno, DbType.String);
                    var cardList = Db.QueryList<OrderCardOutput>(strsql, pa);
                    //更改状态
                    Db.ExecuteNonQuery(" update dbo.t_Order_Card set [State]=1 WHERE OrderNo=@OrderNo", pa);
                    foreach (var item in cardList)
                    {
                        var cardModel =
                            Db.QueryFirstOrDefault<User_Discount_Card>(
                                $"select * from t_User_Discount_Card where UserId='{model.RegisterUserId}' and Id='{item.CardId}' and IfUse=0", null);
                        cardModel.IfUse = 1;
                        _userDiscountCardRepository.Update(cardModel);
                        //var updateSql = "   update t_User_Discount_Card  set IfUse=1 where UserId=( select RegisterUserId from  t_Order_Sheet where OrderNo=@OrderNo) and CardId=@CardId ";
                        //var p = new DynamicParameters();
                        //p.Add(":OrderNo", orderno, DbType.String);
                        //p.Add(":CardId", item.CardCode, DbType.String);
                        //Db.ExecuteNonQuery(updateSql, p);
                    }
                    #endregion
                }


                model.State = state;
                model.FactPayAmount = Convert.ToDecimal(totalamount);
                model.PayType = "c7a3fec3-a886-4805-ab05-35080f2c3185";
                model.PayTime = DateTime.Now;
                _iOrderSheetRepository.Update(model);
            }
            catch (Exception)
            {

                throw;
            }


        }


        /// <summary>
        /// 根据微信支付状态修改订单状态
        /// </summary>
        /// <param name="input"></param>
        public MessagesOutPut EnditSheetStates(SheetModelInput input)
        {
            string sql = string.Empty;
            var dy = new DynamicParameters();
            dy.Add(":OrderNo", input.OrderNo, DbType.String);
            OrderSheet model = Db.QueryFirstOrDefault<OrderSheet>("select  * from t_Order_Sheet where OrderNo = @OrderNo ", dy);
            model.State = Convert.ToInt32(input.State);
            model.FactPayAmount = input.OrderAmount;
            model.PayType = input.PayType;
            model.PayTime = DateTime.Now;
            _iOrderSheetRepository.Update(model);

            //当订单状态为已支付，写入我的课程
            if (Convert.ToInt32(input.State) == (int)OrderState.PaymentHasBeen)
            {
                try
                {
                    sql = "select CourseId,RegisterUserId,ValidityPeriod,Num,CourseType,ValidityEndDate from v_SheetInfoByOrder where OrderNo = @OrderNo ";
                    dy = new DynamicParameters();
                    dy.Add(":OrderNo", input.OrderNo, DbType.String);
                    var list = Db.QueryList<SheetModelOutput>(sql, dy);
                    StringBuilder sqlList = new StringBuilder();
                    var gid = string.Empty;
                    foreach (var item in list)
                    {
                        //添加到我的课程的课程
                        if (item.Num != null)
                        {
                            var endTime = item.ValidityEndDate.HasValue
                                ? item.ValidityEndDate
                                : DateTime.Now.AddYears(100);
                            gid = Guid.NewGuid().ToString();

                            //判断是否购买过该课程
                            var strSql = @" SELECT * FROM dbo.t_My_Course WHERE CourseId=@CourseId AND UserId =@UserId ";
                            var dy1 = new DynamicParameters();
                            dy1.Add(":CourseId", item.CourseId, DbType.String);
                            dy1.Add(":UserId", item.RegisterUserId, DbType.String);
                            var model1 = Db.QueryFirstOrDefault<MyCourse>(strSql, dy1);
                            if (model1 == null)
                            {
                                sqlList.AppendFormat(
                                    " insert into t_My_Course(Id,UserId,CourseId,AddTime,BeginTime,EndTime,OrderId,CourseType)values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                                    gid, item.RegisterUserId, item.CourseId, DateTime.Now, DateTime.Now, endTime,
                                    model.Id, item.CourseType);
                            }
                            else
                            {
                                if (model1.EndTime != null)
                                    model1.EndTime = endTime;
                                _iMyCourseRepository.Update(model1);
                            }
                        }
                        #region 判断是否是套餐课程  如果是套餐课程则需要把子课程相关的题库写到我的课程
                        if (item.CourseType == 1)
                        {
                            var courseListSql =
                                $"SELECT b.* FROM dbo.t_Course_SuitDetail  a LEFT JOIN dbo.t_Course_Info b ON a.CourseId=b.Id  WHERE 1=1 AND b.IsDelete=0 AND a.PackCourseId='{item.CourseId}'";
                            var courseList = Db.QueryList<CourseInfo>(courseListSql);
                            foreach (var courseModel in courseList)
                            {
                                var linkCourseSql1 =
                                    $"select * from t_Course_Info where 1=1 and IsDelete=0  and Type=1 and LinkCourse='{courseModel.Id}'";
                                var linkCourseList1 = Db.QueryList<CourseInfo>(linkCourseSql1);
                                foreach (var itemLinkCourse in linkCourseList1)
                                {
                                    var courseMode = _courseInfoRepository.Get(itemLinkCourse.Id);
                                    var endTime1 = courseMode.ValidityEndDate.HasValue
                                        ? courseMode.ValidityEndDate
                                        : DateTime.Now.AddYears(100);
                                    //判断是否购买过该课程
                                    var strSq2 =
                                        @" SELECT * FROM dbo.t_My_Course WHERE CourseId=@CourseId AND UserId =@UserId ";
                                    var dy2 = new DynamicParameters();
                                    dy2.Add(":CourseId", itemLinkCourse.Id, DbType.String);
                                    dy2.Add(":UserId", item.RegisterUserId, DbType.String);
                                    var model2 = Db.QueryFirstOrDefault<MyCourse>(strSq2, dy2);
                                    if (model2 == null)
                                    {
                                        sqlList.AppendFormat(
                                            " insert into t_My_Course(Id,UserId,CourseId,AddTime,BeginTime,EndTime,OrderId,CourseType)values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                                            Guid.NewGuid(), item.RegisterUserId, itemLinkCourse.Id, DateTime.Now,
                                            DateTime.Now, endTime1, model.Id, 0);
                                    }
                                    else
                                    {
                                        if (model2.EndTime != null)
                                            model2.EndTime = endTime1;
                                        _iMyCourseRepository.Update(model2);
                                    }
                                }

                            }
                        }
                    }
                    #endregion
                    if (sqlList.Length > 0)
                    {
                        Db.ExecuteNonQuery(sqlList.ToString(), null);
                    }
                    #region 已付款，根据订单号找到学习卡，并更新用户学习卡状态
                    var strsql = " SELECT * FROM dbo.t_Order_Card WHERE OrderNo=@OrderNo  ";
                    var pa = new DynamicParameters();
                    pa.Add(":OrderNo", input.OrderNo, DbType.String);
                    var cardList = Db.QueryList<OrderCardOutput>(strsql, pa);
                    //更改状态
                    Db.ExecuteNonQuery(" update dbo.t_Order_Card set [State]=1 WHERE OrderNo=@OrderNo", pa);
                    foreach (var item in cardList)
                    {
                        //var updateSql = "   update t_User_Discount_Card  set IfUse=1 where UserId=( select RegisterUserId from  t_Order_Sheet where OrderNo=@OrderNo) and CardId=@CardId ";
                        //var p = new DynamicParameters();
                        //p.Add(":OrderNo", input.OrderNo, DbType.String);
                        //p.Add(":CardId", item.CardCode, DbType.String);
                        //Db.ExecuteNonQuery(updateSql, p);
                        var cardModel =
                            Db.QueryFirstOrDefault<User_Discount_Card>(
                                $"select * from t_User_Discount_Card where UserId='{model.RegisterUserId}' and Id='{item.CardId}' and IfUse=0", null);
                        cardModel.IfUse = 1;
                        _userDiscountCardRepository.Update(cardModel);
                    }
                    return new MessagesOutPut { Success = true, Message = "订单支付成功" };
                }
                catch (Exception)
                {
                    return new MessagesOutPut { Success = false, Message = "订单支付失败" };
                }
                #endregion
            }
            else
            {
                return new MessagesOutPut { Success = false, Message = "订单支付失败" };
            }

        }

        /// <summary>
        /// 根据订单获取订单金额
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public double GetAmount(string orderNo)
        {
            const string sql = " select OrderAmount from t_Order_Sheet where OrderNo=@OrderNo  ";
            var dynamicparameters = new DynamicParameters();
            dynamicparameters.Add(":OrderNo", orderNo, DbType.String);
            return Db.ExecuteScalar<double>(sql, dynamicparameters);
        }

        /// <summary>
        /// 平台用户撤销订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut EnditSheetState(SheetModelInput input)
        {
            const string sql = " update t_Order_Sheet set State=@State where OrderNo = @OrderNo; delete from t_Order_Card where OrderNo=@OrderNo ";
            var dynamicparameters = new DynamicParameters();
            dynamicparameters.Add(":OrderNo", input.OrderNo, DbType.String);
            dynamicparameters.Add(":State", input.State, DbType.Int32);
            var rowcount = Db.ExecuteNonQuery(sql, dynamicparameters);
            if (rowcount > 0)
            {
                return new MessagesOutPut { Success = true, Message = "撤销订单成功" };
            }
            else
            {
                return new MessagesOutPut { Success = false, Message = "撤销订单失败" };
            }
        }

        /// <summary>
        /// 支付宝-查询订单信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<SheetModelOutput> SheetList(SheetModelInput input)
        {
            const string sql = " select Id,OrderNo,RegisterUserId,CourseId,FavourablePrice,Num,Amount,CourseName,CourseIamge,OrderAmount  from v_SheetInfo where RegisterUserId = @RegisterUserId and OrderNo = @OrderNo ";
            var dynamicparameters = new DynamicParameters();
            dynamicparameters.Add(":RegisterUserId", input.RegisterUserId, DbType.String);
            dynamicparameters.Add(":OrderNo", input.OrderNo, DbType.String);
            var list = Db.QueryList<SheetModelOutput>(sql, dynamicparameters);
            return list;
        }

        /// <summary>
        /// 查询订单状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public SheetModelOutput SheetSdate(SheetModelInput input)
        {
            const string sql = "select [State] from t_Order_Sheet where RegisterUserId = @RegisterUserId and OrderNo = @OrderNo ";
            var dynamicparameters = new DynamicParameters();
            dynamicparameters.Add(":RegisterUserId", input.RegisterUserId, DbType.String);
            dynamicparameters.Add(":OrderNo", input.OrderNo, DbType.String);
            var list = Db.QueryFirstOrDefault<SheetModelOutput>(sql, dynamicparameters);
            return list;
        }

        /// <summary>
        /// 查询是否已购买
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IfAreadyPay(SheetDetailModelInput input)
        {
            var sql = "";
            if (input.CourseType == 0)
            {
                sql = "SELECT COUNT(1) FROM  (  SELECT CourseId FROM t_My_Course  WHERE CourseType=0 AND UserId=@UserId and EndTime >= GETDATE() UNION (SELECT b.CourseId FROM dbo.t_My_Course a LEFT JOIN dbo.t_Course_SuitDetail b ON a.CourseId=b.PackCourseId  WHERE  a.CourseType=1 and UserId=@UserId AND EndTime >= GETDATE()))c WHERE c.CourseId=@CourseId ";
            }
            else if (input.CourseType == 1)
            {
                sql = "select count(1) from t_My_Course where courseType=1 and  UserId=@UserId and EndTime >= GETDATE() and CourseId=@CourseId ";
            }
            else if (input.CourseType == 2)
            {
                sql = "select count(1) from t_My_Course where courseType=2 and  UserId=@UserId and EndTime >= GETDATE() and CourseId=@CourseId ";
            }
            var parameters = new DynamicParameters();
            parameters.Add(":UserId", input.UserId, DbType.String);
            parameters.Add(":CourseId", input.CourseId, DbType.String);
            return Db.ExecuteScalar<int>(sql, parameters) > 0;
        }

        /// <summary>
        /// 查询订单是否已经被支付或支付失败
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut OrderSheetIsPay(SheetModelInput input)
        {
            const string sql = " SELECT COUNT(1) FROM dbo.t_Order_Sheet WHERE OrderNo=@OrderNo AND RegisterUserId=@RegisterUserId AND [State]=1  ";
            var parament = new DynamicParameters();
            parament.Add(":OrderNo", input.OrderNo, DbType.String);
            parament.Add(":RegisterUserId", input.RegisterUserId, DbType.String);
            var count = Db.ExecuteScalar<int>(sql, parament);
            if (count > 0)
            {
                return new MessagesOutPut { Success = true };
            }
            else
            {
                return new MessagesOutPut { Success = false };
            }
        }

    }
}
