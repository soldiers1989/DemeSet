using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Application.WebApiAppService.CourseModule;
using ZF.Application.WebApiDto.UserDiscountCardModule;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;

namespace ZF.Application.WebApiAppService.UserDiscountCardModule
{
    public class UserDiscountCardAppService : BaseAppService<User_Discount_Card>
    {
        private readonly IUserDiscountCardRepository _repository;
        private readonly CourseInfoAppService _courseInfoAppService;


        public UserDiscountCardAppService(IUserDiscountCardRepository repository, CourseInfoAppService courseInfoAppService) : base(repository)
        {
            _repository = repository;
            _courseInfoAppService = courseInfoAppService;
        }


        /// <summary>
        /// 添加学习卡
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut AddCard(UserDiscountCardModelInput input)
        {
            var sql = "SELECT COUNT(1) FROM dbo.t_User_Discount_Card WHERE UserId=@UserId AND CardId=@CardId ";
            var dy = new DynamicParameters();
            dy.Add(":UserId", input.UserId, DbType.String);
            dy.Add(":CardId", input.CardId, DbType.String);
            var result = Db.ExecuteScalar<int>(sql, dy);
            if (result > 0)
            {
                return new MessagesOutPut { Success = true, Message = "您已领取该学习卡" };
            }
            else
            {
                User_Discount_Card model = input.MapTo<User_Discount_Card>();
                model.CardId = input.CardId;
                model.Id = Guid.NewGuid().ToString();
                model.UserId = input.UserId;
                model.AddTime = DateTime.Now;
                model.Type = input.Type;
                model.IfUse = 0;
                _repository.InsertGetId(model);
                return new MessagesOutPut { Success = true, Message = "领取成功" };
            }
        }

        /// <summary>
        /// 我的学习卡/全部
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<UserDiscountCardModelOutput> GetMyCard(UserDiscountCardModelInput input)
        {
            var sql = " select * from( SELECT  CASE WHEN b.EndDate<GETDATE() THEN 2 ELSE a.IfUse END ifUse,b.* FROM dbo.t_User_Discount_Card a LEFT JOIN dbo.t_Discount_Card b ON a.CardId=b.CardCode WHERE a.UserId=@UserId ) a order by ifUse ";
            var dy = new DynamicParameters();
            dy.Add(":UserId", input.UserId, DbType.String);
            var list = Db.QueryList<UserDiscountCardModelOutput>(sql, dy);
            foreach (var item in list)
            {
                var targetCourseName = "";
                var target = item.TargetCourse;
                if (!string.IsNullOrEmpty(target))
                {
                    var arr = target.Split(',');
                    for (var i = 0; i < arr.Length; i++)
                    {
                        var model = _courseInfoAppService.Get(arr[i]);
                        if (model != null)
                        {
                            targetCourseName += "[" + model.CourseName + "]，";
                        }
                    }
                    item.TargetCourse = targetCourseName.TrimEnd('，');
                }
            }
            return list;
        }

        /// <summary>
        /// 是否存在该学习卡
        /// </summary>
        /// <param name="CardId"></param>
        /// <returns></returns>
        public MessagesOutPut IfExistAndExpair(string CardId)
        {
            var sql = " select count(1) from t_Discount_Card where CardCode=@CardId ";
            var dy = new DynamicParameters();
            dy.Add(":CardId", CardId, DbType.String);
            var result = Db.ExecuteScalar<int>(sql, dy) > 0;
            if (result)
            {
                sql += " and ISNULL(BeginDate,'0001-01-01')<=GETDATE() AND ISNULL(EndDate,'9999-12-31')>=GETDATE() ";
                result = Db.ExecuteScalar<int>(sql, dy) > 0;
                if (result)
                {
                    return new MessagesOutPut { Success = true };
                }
                else
                {
                    return new MessagesOutPut { Success = false, Message = "该学习卡已过期,不可领取!" };
                }
            }
            else
            {
                return new MessagesOutPut { Message = "该学习卡不存在，请确认后重新输入!", Success = false };
            }

        }

        /// <summary>
        /// 获取用户学习卡
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<UserDiscountCardModelOutput> GetList(UserDiscountCardModelInput input)
        {
            var sql = " SELECT a.ifUse,b.Id,b.Amount,b.CardCode,b.BeginDate,CONVERT(varchar(100),b.EndDate, 23)EndDate ,b.TargetCourse,b.CardName FROM dbo.t_User_Discount_Card a LEFT JOIN dbo.t_Discount_Card b ON a.CardId=b.CardCode WHERE a.UserId=@UserId  ";
            var dy = new DynamicParameters();
            dy.Add(":UserId", input.UserId, DbType.String);

            if (input.IfExpair == 1)
            {
                sql += " AND a.IfUse=0 and isnull(b.EndDate,'9999-12-31')<getDate()";
            }
            else
            {
                sql += " AND a.IfUse=@IfUse and  isnull(b.EndDate,'9999-12-31')>=getDate() and isnull(b.BeginDate,'0001-01-01')<=getDate() ";
                dy.Add(":IfUse", input.IfUse, DbType.Int16);
            }
            var list = Db.QueryList<UserDiscountCardModelOutput>(sql, dy);
            foreach (var item in list)
            {
                var targetCourseName = "";
                var target = item.TargetCourse;
                if (!string.IsNullOrEmpty(target))
                {
                    var arr = target.Split(',');
                    for (var i = 0; i < arr.Length; i++)
                    {
                        var model = _courseInfoAppService.Get(arr[i]);
                        if (model != null)
                        {
                            targetCourseName += "[" + model.CourseName + "]，";
                        }
                    }
                    item.TargetCourse = targetCourseName.TrimEnd('，');
                }
            }
            return list;
        }


        /// <summary>
        /// 获取可使用的所有卡号
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public List<User_Discount_Card> GetCardList(string userId, string cardId)
        {
            return Db.QueryList<User_Discount_Card>($"select * from t_User_Discount_Card where UserId='{userId}' and CardId='{cardId}' and IfUse=0");
        }


        /// <summary>
        /// 获取可使用的学习卡
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="courseId"></param>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public List<UserDiscountCardModelOutput> GetUseCard(string userid, string courseId, string orderNo)
        {
            var count = 4;
            var OrderSheet =
                Db.QueryFirstOrDefault<OrderSheet>($" SELECT * FROM dbo.t_Order_Sheet WHERE OrderNo='{orderNo}'", null);
            if (OrderSheet != null)
            {
                if (OrderSheet.OrderAmount >= 600)
                {
                    count = 8;
                }
            }
            var sql = @"SELECT  b.CardName ,COUNT(1) Count, SUM(ISNULL(b.Amount, 0))/COUNT(1) dj,
        SUM(ISNULL(b.Amount, 0)) Amount ,
        b.CardCode
FROM    dbo.t_User_Discount_Card a
        LEFT JOIN dbo.t_Discount_Card b ON a.CardId = b.CardCode
WHERE   a.IfUse = 0
        AND ISNULL(b.EndDate, '9999-12-31') >= GETDATE()
        AND ISNULL(b.BeginDate, '0001-01-01') <= GETDATE()
        AND ( CHARINDEX(@CourseId, b.TargetCourse) > 0
              OR ISNULL(b.TargetCourse, '') = ''
            )
        AND a.UserId = @UserId
        AND a.Id NOT IN (
        SELECT  ISNULL(b.CardId, '')
        FROM    dbo.t_Order_Sheet a
                LEFT JOIN dbo.t_Order_Card b ON a.OrderNo = b.OrderNo
        WHERE   a.RegisterUserId = @UserId
                AND b.[State] = 1 )
GROUP BY b.CardName ,
        b.CardCode,b.Amount  ORDER BY b.Amount DESC  ";
            var dy = new DynamicParameters();
            dy.Add(":CourseId", courseId, DbType.String);
            dy.Add(":UserId", userid, DbType.String);
            var model = Db.QueryList<UserDiscountCardModelOutput>(sql, dy);

            foreach (var item in model)
            {
                item.Amount = 0;
                if (count > 0)
                {
                    if (item.Count > count)
                    {
                        item.Amount = item.dj * count;
                        item.Count = count;
                        count = 0;
                    }
                    else
                    {
                        item.Amount = item.dj * item.Count;
                        count = count - item.Count;
                    }
                }
            }
            foreach (var item1 in model)
            {
                if (item1.Amount <= 0)
                {
                    model.Remove(item1);
                }
            }
            return model;
        }
    }
}
