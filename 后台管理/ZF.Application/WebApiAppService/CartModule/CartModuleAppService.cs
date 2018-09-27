using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.WebApiDto.CartDtoModule;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using System.Data;
using ZF.Application.BaseDto;
using ZF.Infrastructure;
using ZF.Infrastructure.RandomHelper;
using ZF.Infrastructure.AppWikiService;
using ZF.Application.WebApiDto.SheetDtoModule;
using ZF.Infrastructure.Md5;
using System.Web.Script.Serialization;

namespace ZF.Application.WebApiAppService.CartModule
{
    /// <summary>
    /// 购物车服务
    /// </summary>
    public class CartModuleAppService : BaseAppService<OrderCart>
    {
        private static string DefuleDomain = ConfigurationManager.AppSettings["DefuleDomain"];
        private readonly IOrderCartRepository _repository;

        private readonly IOrderCartDetailRepository _iOrderCartDetailRepository;

        private readonly ICourseInfoRepository _iCourseInfoRepository;

        private readonly ICoursePackcourseInfoRepository _iCoursePackcourseInfoRepository;

        private readonly IOrderSheetRepository _orderSheetRepository;

        private readonly IOrderDetailRepository _orderDetailRepository;

        private readonly IMyCourseRepository _iMyCourseRepository;

        private readonly ICourseFaceToFaceRepository _courseFaceToFaceRepository;

        private readonly ICourseInfoRepository _courseInfoRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="iOrderCartDetailRepository"></param>
        /// <param name="iCourseInfoRepository"></param>
        /// <param name="iCoursePackcourseInfoRepository"></param>
        /// <param name="orderSheetRepository"></param>
        /// <param name="orderDetailRepository"></param>
        /// <param name="iMyCourseRepository"></param>
        /// <param name="courseFaceToFaceRepository"></param>
        public CartModuleAppService(IOrderCartRepository repository, IOrderCartDetailRepository iOrderCartDetailRepository, ICourseInfoRepository iCourseInfoRepository, ICoursePackcourseInfoRepository iCoursePackcourseInfoRepository, IOrderSheetRepository orderSheetRepository, IOrderDetailRepository orderDetailRepository, IMyCourseRepository iMyCourseRepository, ICourseFaceToFaceRepository courseFaceToFaceRepository, ICourseInfoRepository courseInfoRepository) : base(repository)
        {
            _repository = repository;
            _iOrderCartDetailRepository = iOrderCartDetailRepository;
            _iCourseInfoRepository = iCourseInfoRepository;
            _iCoursePackcourseInfoRepository = iCoursePackcourseInfoRepository;
            _orderSheetRepository = orderSheetRepository;
            _orderDetailRepository = orderDetailRepository;
            _iMyCourseRepository = iMyCourseRepository;
            _courseFaceToFaceRepository = courseFaceToFaceRepository;
            _courseInfoRepository = courseInfoRepository;
        }

        /// <summary>
        /// 订单查询
        /// </summary>
        /// <param name="registerUserId"></param>
        /// <returns></returns>
        public List<CartModelOutput> CartInfoList(string registerUserId)
        {
            string sql = " select CourseType,Id,OrderNo,RegisterUserId,CourseId,FavourablePrice,Num,Amount,CourseName,'" + DefuleDomain + "/'+CourseIamge CourseIamge from v_CartInfo where RegisterUserId = @RegisterUserId ";
            var dynamicparameters = new DynamicParameters();
            dynamicparameters.Add(":RegisterUserId", registerUserId, DbType.String);

            var list = Db.QueryList<CartModelOutput>(sql, dynamicparameters);

            return list;
        }

        /// <summary>
        /// 删除订单明细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut DelCartDetail(CartModelInput input)
        {
            const string sql = " delete t_Order_CartDetail where OrderNo = @Id ";
            var dynamicparameters = new DynamicParameters();
            dynamicparameters.Add(":Id", input.Id, DbType.String);
            var count = Db.ExecuteNonQuery(sql, dynamicparameters);
            if (count > 0)
            {
                return new MessagesOutPut { Success = true, Message = "删除成功" };
            }
            else
            {
                return new MessagesOutPut { Success = false, Message = "删除失败" };
            }
        }

        /// <summary>
        /// 添加到购物车
        /// </summary>
        /// <returns></returns>
        public MessagesOutPut Add(OrderCartDetailInput input, string userId)
        {
            if (string.IsNullOrWhiteSpace(input.CourseId) || input.CourseType == null)
            {
                return new MessagesOutPut { Success = false, Message = "加入购物车失败" };
            }
            OrderCartDetail model;
            var strSql = @"  SELECT a.Id FROM t_Order_CartDetail a LEFT JOIN t_Order_Cart b ON a.OrderNo=b.Id  where 1=1 and a.CourseId=@CourseId and b.RegisterUserId=@userId  ";
            var dy = new DynamicParameters();
            dy.Add(":CourseId", input.CourseId, DbType.String);
            dy.Add(":userId", userId, DbType.String);
            //判断是否在购物车存在
            if (Db.QueryFirstOrDefault<OrderCartDetail>(strSql, dy) != null)
            {
                //model = Db.QueryFirstOrDefault<OrderCartDetail>(strSql, dy);
                //model.Num += 1;
                //model.Amount += model.FavourablePrice;
                //_iOrderCartDetailRepository.Update(model);
                return new MessagesOutPut { Success = false, Message = "加入购物车失败! 该课程已加入购物车!" };
            }
            else
            {
                model = new OrderCartDetail
                {
                    CourseId = input.CourseId,
                    CourseType = input.CourseType,
                    Num = 1,
                    Id = Guid.NewGuid().ToString(),
                };
                if (input.CourseType == 0)
                {
                    var courseInfo = _iCourseInfoRepository.Get(input.CourseId);
                    if (courseInfo == null)
                    {
                        return new MessagesOutPut { Success = false, Message = "加入购物车失败" };
                    }
                    if (courseInfo.State == 0)
                    {
                        return new MessagesOutPut { Success = false, Message = "加入购物车失败,该点播课已经下架!" };
                    }
                    model.FavourablePrice = courseInfo.FavourablePrice;
                    model.Price = courseInfo.Price;
                    model.Amount = courseInfo.FavourablePrice;
                    if (courseInfo.ValidityEndDate <= DateTime.Now)
                    {
                        return new MessagesOutPut { Success = false, Message = "加入购物车失败,该点播课已经失效!" };
                    }
                }
                else if (input.CourseType == 1)
                {
                    var coursePackcourseInfo = _iCoursePackcourseInfoRepository.Get(input.CourseId);
                    if (coursePackcourseInfo == null)
                    {
                        return new MessagesOutPut { Success = false, Message = "加入购物车失败" };
                    }
                    if (coursePackcourseInfo.State == 0)
                    {
                        return new MessagesOutPut { Success = false, Message = "加入购物车失败,该网络课已经下架!" };
                    }
                    model.FavourablePrice = coursePackcourseInfo.FavourablePrice;
                    model.Price = coursePackcourseInfo.Price;
                    model.Amount = coursePackcourseInfo.FavourablePrice;
                }
                else if (input.CourseType == 2)
                {
                    var courseFaceToFace = _courseFaceToFaceRepository.Get(input.CourseId);
                    if (courseFaceToFace == null)
                    {
                        return new MessagesOutPut { Success = false, Message = "加入购物车失败" };
                    }
                    if (courseFaceToFace.State == 0)
                    {
                        return new MessagesOutPut { Success = false, Message = "加入购物车失败,该面授课已经下架!" };
                    }
                    model.FavourablePrice = courseFaceToFace.FavourablePrice;
                    model.Price = courseFaceToFace.Price;
                    model.Amount = courseFaceToFace.FavourablePrice;
                }
                if (model.Amount == 0)
                {
                    var orderSheet = new OrderSheet();
                    orderSheet.Id = Guid.NewGuid().ToString();
                    orderSheet.OrderNo = DateTime.Now.ToString("yyyyMMddHHmmssfff") + RandomHelper.GetRandom(3, 1)[0];
                    orderSheet.RegisterUserId = userId;
                    orderSheet.AddTime = DateTime.Now;
                    orderSheet.OrderAmount = model.Amount.Value;
                    orderSheet.State = (int)OrderState.PaymentHasBeen;
                    orderSheet.FactPayAmount = 0;
                    orderSheet.OrderIp = GetAddressIp();
                    orderSheet.SetOrderTime = null;
                    orderSheet.OrderType = (int)OrderType.Web;
                    orderSheet.PayType = "";
                    _orderSheetRepository.Insert(orderSheet);
                    _orderDetailRepository.Insert(new OrderDetail
                    {
                        Amount = model.Amount.Value,
                        CourseId = model.CourseId,
                        CourseType = model.CourseType,
                        FavourablePrice = model.Amount,
                        Id = Guid.NewGuid().ToString(),
                        Num = 1,
                        OrderNo = orderSheet.Id,
                        Price = model.Price ?? 0
                    });

                    var dyCourse = new DynamicParameters();
                    dyCourse.Add(":CourseId", input.CourseId, DbType.String);
                    dyCourse.Add(":UserId", userId, DbType.String);
                    var myCourse =
                        Db.QueryFirstOrDefault<MyCourse>(
                            "select * from t_My_Course where CourseId=@CourseId and UserId=@UserId ", dyCourse);
                    if (myCourse == null)
                    {
                        if (model.Amount == 0)
                        {
                            var courseMode = _courseInfoRepository.Get(model.CourseId);
                            _iMyCourseRepository.Insert(new MyCourse
                            {
                                AddTime = DateTime.Now,
                                BeginTime = DateTime.Now,
                                CourseId = input.CourseId,
                                EndTime = courseMode.ValidityEndDate.HasValue ? courseMode.ValidityEndDate : DateTime.Now.AddYears(100),
                                Id = Guid.NewGuid().ToString(),
                                UserId = userId,
                                CourseType = input.CourseType,
                                OrderId = orderSheet.Id
                            });

                            StringBuilder sqlList = new StringBuilder();
                            var linkCourseSql = $"select * from t_Course_Info where 1=1 and IsDelete=0  and Type=1 and LinkCourse='{input.CourseId}'";
                            var linkCourseList = Db.QueryList<CourseInfo>(linkCourseSql);
                            foreach (var itemLinkCourse in linkCourseList)
                            {
                                var courseMode1 = _courseInfoRepository.Get(itemLinkCourse.Id);
                                var endTime1 = courseMode1.ValidityEndDate.HasValue
                                    ? courseMode1.ValidityEndDate
                                    : DateTime.Now.AddYears(100);
                                //判断是否购买过该课程
                                var strSq2 = @" SELECT * FROM dbo.t_My_Course WHERE CourseId=@CourseId AND UserId =@UserId ";
                                var dy2 = new DynamicParameters();
                                dy2.Add(":CourseId", itemLinkCourse.Id, DbType.String);
                                dy2.Add(":UserId", userId, DbType.String);
                                var model2 = Db.QueryFirstOrDefault<MyCourse>(strSq2, dy2);
                                if (model2 == null)
                                {
                                    sqlList.AppendFormat(" insert into t_My_Course(Id,UserId,CourseId,AddTime,BeginTime,EndTime,OrderId,CourseType)values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                                        Guid.NewGuid(), userId, itemLinkCourse.Id, DateTime.Now, DateTime.Now, endTime1, model.Id, 0);
                                }
                                else
                                {
                                    if (model2.EndTime != null)
                                        model2.EndTime = endTime1;
                                    _iMyCourseRepository.Update(model2);
                                }
                                if (sqlList.Length > 0)
                                {
                                    Db.ExecuteNonQuery(sqlList.ToString(), null);
                                }
                            }
                        }
                    }
                }
                else
                {
                    var orderNo = DateTime.Now.ToString("yyyyMMddHHmmssfff") + RandomHelper.GetRandom(3, 1)[0].ToString();
                    var orderCartId = _repository.InsertGetId(new OrderCart
                    {
                        Id = Guid.NewGuid().ToString(),
                        RegisterUserId = userId,
                        OrderNo = orderNo
                    });
                    model.OrderNo = orderCartId;
                    _iOrderCartDetailRepository.Insert(model);
                }
            }
            return new MessagesOutPut { Success = true, Message = "加入购物车成功" };
        }

        /// <summary>
        /// 删除购物车明细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut Delete(IdInputIds input)
        {
            try
            {
                OrderCartDetail model;
                foreach (var item in input.Ids.Split(','))
                {
                    model = new OrderCartDetail();
                    model.Id = item;
                    _iOrderCartDetailRepository.Delete(model);
                }
                return new MessagesOutPut { Success = true, Message = "删除成功" };
            }
            catch
            {
                return new MessagesOutPut { Success = false, Message = "删除失败" };
            }
        }

        /// <summary>
        /// 获得指定课程信息以及购物车数量
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CourseInfo GetCourseInfoByOneAndUserCartCount(string id, string userId)
        {
            var courseInfo = _iCourseInfoRepository.Get(id);
            const string sql = @" SELECT SUM(b.Num) 
                                  FROM    t_Order_Cart a
                                          INNER JOIN t_Order_CartDetail b ON a.Id = b.OrderNo
                                  WHERE   RegisterUserId = @RegisterUserId
                                  GROUP BY a.RegisterUserId; ";
            var parament = new DynamicParameters();
            parament.Add(":RegisterUserId", userId, DbType.String);
            courseInfo.ViewCount = Db.ExecuteScalar<int>(sql, parament);
            return courseInfo;
        }

        /// <summary>
        /// 获得指定课程信息以及购物车数量
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CoursePackcourseInfo GetPackInfoByOneAndUserCartCount(string id, string userId)
        {
            var courseInfo = _iCoursePackcourseInfoRepository.Get(id);
            const string sql = @" SELECT SUM(b.Num) 
                                  FROM    t_Order_Cart a
                                          INNER JOIN t_Order_CartDetail b ON a.Id = b.OrderNo
                                  WHERE   RegisterUserId = @RegisterUserId
                                  GROUP BY a.RegisterUserId; ";
            var parament = new DynamicParameters();
            parament.Add(":RegisterUserId", userId, DbType.String);
            courseInfo.ViewCount = Db.ExecuteScalar<int>(sql, parament);
            return courseInfo;
        }



        #region 未登录查询购物车信息
        /// <summary>
        /// 未登录情况下的购物车信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<CartModelOutput> CartList(IdInputIds input)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(" SELECT Id,CourseName,'" + DefuleDomain + "/'+Courseiamge CourseIamge,FavourablePrice,1 Num,FavourablePrice Amount FROM V_CourseInfo where Id in( ");
            foreach (string item in input.Ids.Split(','))
            {
                sql.AppendFormat("'{0}',", item.Split('@')[0]);
            }
            var list = Db.QueryList<CartModelOutput>(sql.ToString().TrimEnd(',') + " )");
            return list;
        }

        /// <summary>
        /// 未登录之前写入购物车登录后写入购物车表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public MessagesOutPut CartAdd(OrderCartDetailInput input, string userId)
        {
            string courserId = string.Empty;
            int courseType = 0;
            string groupGid = string.Empty;
            foreach (var item in input.CourseId.Split(','))
            {
                courserId = item.Split('@')[0];
                courseType = Convert.ToInt32(item.Split('@')[1]);

                if (string.IsNullOrWhiteSpace(courserId) || courseType == null)
                {
                    continue;
                }
                OrderCartDetail model;
                var strSql = @"  SELECT a.* FROM t_Order_CartDetail a LEFT JOIN t_Order_Cart b ON a.OrderNo=b.Id  where 1=1 and a.CourseId=@CourseId and b.RegisterUserId=@userId ";
                var dy = new DynamicParameters();
                dy.Add(":CourseId", courserId, DbType.String);
                dy.Add(":userId", userId, DbType.String);
                //判断是否在购物车存在
                if (Db.QueryFirstOrDefault<OrderCartDetail>(strSql, dy) != null)
                {
                    if (string.IsNullOrEmpty(groupGid))
                    {
                        groupGid = Guid.NewGuid().ToString();
                    }

                    model = Db.QueryFirstOrDefault<OrderCartDetail>(strSql, dy);
                    model.Num = 1;
                    model.Amount += model.FavourablePrice;
                    model.NologinGroup = groupGid;
                    _iOrderCartDetailRepository.Update(model);
                    continue;
                }
                else
                {
                    model = new OrderCartDetail
                    {
                        CourseId = courserId,
                        CourseType = courseType,
                        Num = 1,
                        Id = Guid.NewGuid().ToString(),
                    };
                    if (courseType == 0)
                    {
                        var courseInfo = _iCourseInfoRepository.Get(courserId);
                        if (courseInfo == null)
                        {
                            continue;
                        }
                        if (courseInfo.State == 0)
                        {
                            continue;
                        }
                        model.FavourablePrice = courseInfo.FavourablePrice;
                        model.Price = courseInfo.Price;
                        model.Amount = courseInfo.FavourablePrice;
                    }
                    else if (courseType == 1)
                    {
                        var coursePackcourseInfo = _iCoursePackcourseInfoRepository.Get(courserId);
                        if (coursePackcourseInfo == null)
                        {
                            continue;
                        }
                        if (coursePackcourseInfo.State == 0)
                        {
                            continue;
                        }
                        model.FavourablePrice = coursePackcourseInfo.FavourablePrice;
                        model.Price = coursePackcourseInfo.Price;
                        model.Amount = coursePackcourseInfo.FavourablePrice;
                    }
                    else if (courseType == 2)
                    {
                        var courseFaceToFace = _courseFaceToFaceRepository.Get(courserId);
                        if (courseFaceToFace == null)
                        {
                            continue;
                        }
                        if (courseFaceToFace.State == 0)
                        {
                            continue;
                        }
                        model.FavourablePrice = courseFaceToFace.FavourablePrice;
                        model.Price = courseFaceToFace.Price;
                        model.Amount = courseFaceToFace.FavourablePrice;
                    }
                    #region 订单或购物车主表写入
                    if (model.Amount == 0)
                    {
                        var orderSheet = new OrderSheet();
                        orderSheet.Id = Guid.NewGuid().ToString();
                        orderSheet.OrderNo = DateTime.Now.ToString("yyyyMMddHHmmssfff") + RandomHelper.GetRandom(3, 1)[0];
                        orderSheet.RegisterUserId = userId;
                        orderSheet.AddTime = DateTime.Now;
                        orderSheet.OrderAmount = model.Amount.Value;
                        orderSheet.State = (int)OrderState.PaymentHasBeen;
                        orderSheet.FactPayAmount = 0;
                        orderSheet.OrderIp = GetAddressIp();
                        orderSheet.OrderType = (int)OrderType.Web;
                        orderSheet.PayType = "";
                        _orderSheetRepository.Insert(orderSheet);
                        _orderDetailRepository.Insert(new OrderDetail
                        {
                            Amount = model.Amount.Value,
                            CourseId = model.CourseId,
                            CourseType = model.CourseType,
                            FavourablePrice = model.Amount,
                            Id = Guid.NewGuid().ToString(),
                            Num = 1,
                            OrderNo = orderSheet.Id,
                            Price = model.Price ?? 0
                        });

                        var dyCourse = new DynamicParameters();
                        dyCourse.Add(":CourseId", courserId, DbType.String);
                        dyCourse.Add(":UserId", userId, DbType.String);
                        var myCourse =
                            Db.QueryFirstOrDefault<MyCourse>(
                                "select * from t_My_Course where CourseId=@CourseId and UserId=@UserId ", dyCourse);
                        if (myCourse == null)
                        {
                            if (model.Amount == 0)
                            {
                                _iMyCourseRepository.Insert(new MyCourse
                                {
                                    AddTime = DateTime.Now,
                                    BeginTime = DateTime.Now,
                                    CourseId = courserId,
                                    EndTime = DateTime.Now.AddYears(100),
                                    Id = Guid.NewGuid().ToString(),
                                    UserId = userId,
                                    CourseType = courseType,
                                    OrderId = orderSheet.Id
                                });
                            }

                        }
                        if (string.IsNullOrEmpty(groupGid))
                        {
                            groupGid = string.Empty;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(groupGid))
                        {
                            groupGid = Guid.NewGuid().ToString();
                        }

                        var orderNo = DateTime.Now.ToString("yyyyMMddHHmmssfff") + RandomHelper.GetRandom(3, 1)[0].ToString();
                        var orderCartId = _repository.InsertGetId(new OrderCart
                        {
                            Id = Guid.NewGuid().ToString(),
                            RegisterUserId = userId,
                            OrderNo = orderNo
                        });
                        model.OrderNo = orderCartId;
                        model.NologinGroup = groupGid;
                        _iOrderCartDetailRepository.Insert(model);
                    }
                    #endregion
                }

            }
            return new MessagesOutPut { Success = true, Message = groupGid };
        }



        #endregion


        #region app端-服务
        /// <summary>
        /// app微信支付-获取签名
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut GetWikiPaySign(SheetModelInput input)
        {
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                WxPayData data = new WxPayData();
                StringBuilder str = new StringBuilder();
                str.AppendFormat("appid={0}", ToAppConfig.appid);
                str.AppendFormat("&attach={0}", "微信支付");
                str.AppendFormat("&body={0}", "中国考试在线-职称经济师课程");
                str.AppendFormat("&detail={0}", "中国考试在线");
                str.AppendFormat("&fee_type={0}", "CNY");
                str.AppendFormat("&mch_id={0}", ToAppConfig.mch_id);
                str.AppendFormat("&nonce_str={0}", WxPayApi.GenerateNonceStr());
                str.AppendFormat("&notify_url={0}", WxPayConfig.NOTIFY_URL);
                str.AppendFormat("&out_trade_no={0}", input.OrderNo);
                str.AppendFormat("&spbill_create_ip={0}", WxPayConfig.IP);
                str.AppendFormat("&total_fee={0}", Convert.ToInt32(input.OrderAmount).ToString());
                str.AppendFormat("&trade_type={0}", "APP");
                str.AppendFormat("&key={0}", ToAppConfig.appkey);

                Dictionary<string, string> list = new Dictionary<string, string>();
                list.Add("appid", ToAppConfig.appid);
                list.Add("secret", ToAppConfig.secret);
                list.Add("nonce_str", WxPayApi.GenerateNonceStr());
                list.Add("body", "职称经济师课程");
                list.Add("out_trade_no", input.OrderNo);
                list.Add("total_fee", Convert.ToInt32(input.OrderAmount).ToString());
                list.Add("spbill_create_ip", WxPayConfig.IP);
                list.Add("notify_url", WxPayConfig.NOTIFY_URL);
                list.Add("trade_type", "APP");
                list.Add("sign", AppMakeSign(str.ToString()));
                list.Add("key", ToAppConfig.appkey);
                list.Add("partnerid", ToAppConfig.mch_id);
                return new MessagesOutPut { Success = true, Message = js.Serialize(list) };
            }
            catch (Exception)
            {
                return new MessagesOutPut { Success = false, Message = "签名失败" };
            }
        }

        /// <summary>
        /// 获得appid以及AppSecret
        /// </summary>
        /// <returns></returns>
        public MessagesOutPut GetWikiLoginConfig()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();

            Dictionary<string, string> list = new Dictionary<string, string>();
            list.Add("appid", ToAppConfig.appid);
            list.Add("secret", ToAppConfig.secret);
            return new MessagesOutPut { Success = false, Message = js.Serialize(list) };
        }

        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="sign"></param>
        /// <returns></returns>
        public string AppMakeSign(string sign)
        {
            //MD5加密
            return Md5.GetMd5(sign).ToString().ToUpper();
            //return md5.ToString();



            //var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(sign));
            //var sb = new StringBuilder();
            //foreach (byte b in bs)
            //{
            //    sb.Append(b.ToString("x2"));
            //}
            ////所有字符转为大写
            //return sb.ToString().ToUpper();
        }
        #endregion

    }
}
